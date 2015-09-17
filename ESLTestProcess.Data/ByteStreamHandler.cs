using log4net;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


namespace ESLTestProcess.Data
{

    public sealed class ByteStreamHandler
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Task _queueProcessorTask;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken _cancellationToken;

        public ByteStreamHandler()
        {
            // Start the thread that will process the incoming data
            _cancellationToken = _cancellationTokenSource.Token;
            _queueProcessorTask = new Task(() => ProcessQueue(_cancellationToken), _cancellationToken, TaskCreationOptions.LongRunning);
            _queueProcessorTask.Start();
        }

        // The ConcurrentQueue is apparently thread safe
        private ConcurrentQueue<byte> _byteQueue = new ConcurrentQueue<byte>();
        
        public void AddToBytesQueue(byte[] byteStream)
        {
            foreach (byte rxByte in byteStream)
            {
                _byteQueue.Enqueue(rxByte);
            }
        }

        public bool IsProcessQueueEmpty()
        {
            return _byteQueue.IsEmpty;// &!_processingData;
        }

        public int ByteQueueSize { get { return _byteQueue.Count; } }


        public async void ProcessQueue(CancellationToken cancellationToken)
        {
            try
            {
                // Check the task has not already been cancelled
                if (cancellationToken.IsCancellationRequested)
                    return;

                byte rxByte;
                int byteCount = 0;
                int expectedLength = 0;

                using (MemoryStream responseMemoryStream = new MemoryStream())
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (_byteQueue.TryDequeue(out rxByte))
                        {
                            byteCount++;
                            responseMemoryStream.WriteByte(rxByte);
                            if (byteCount == 1)
                            {
                                if (rxByte != 0x02)
                                {
                                    byteCount = 0;
                                    _log.ErrorFormat("Expected STX byte but got 0x{0}. Skipping byte", rxByte.ToString("X2"));
                                }
                            }
                            else if (byteCount == 2)
                            {
                                expectedLength = GetResponseLength(rxByte);

                                if (expectedLength == 0) // Response was not found
                                    byteCount = 0;
                            }
                            else if (byteCount > 0 && byteCount == expectedLength)
                            {
                                if (rxByte == 0x03)
                                {

                                    if (responseMemoryStream.Length != expectedLength)
                                        _log.Error("Unexpected state");

                                    // Process the reponse
                                    ProcessResponseData(responseMemoryStream.ToArray());
                                }
                                else
                                {
                                    _log.ErrorFormat("Expected ETX byte but got 0x{0}", rxByte.ToString("X2"));
                                }
                                byteCount = 0;
                            }

                            if (byteCount == 0)
                                responseMemoryStream.SetLength(0);
                        }
                        else
                        {
                            // Pause the thread but allow cancellation
                            // TODO: decrease the delay once cancellation has been tested
                            await Task.Delay(10, cancellationToken);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Swallow this execption
            }
            catch (Exception)
            {
                throw;
            }
        }

        public class ProcessResponseEventArgs : EventArgs
        {
            public byte ResponseId { get; set; }
            public byte[] RawData { get; set; }
        }

        public event EventHandler<ProcessResponseEventArgs> ProcessResponseEventHandler;

        private void ProcessResponseData(byte[] responseData)
        {
            if (ProcessResponseEventHandler != null)
                ProcessResponseEventHandler(null, new ProcessResponseEventArgs
                {
                    ResponseId= responseData[1], 
                    RawData = responseData
                });
        }

        private int GetResponseLength(byte rxByte)
        {
            var response = TestParameters.ResponseValues.FirstOrDefault(r => r.TestId == rxByte);

            if (response == null)
                return 0;  // Indicates not found...

            return response.ExpectedLength;
        }

        internal bool Stop()
        {
            _cancellationTokenSource.Cancel();
            //Thread.Sleep(1000);
            _queueProcessorTask.Wait();
            _cancellationTokenSource.Dispose();
            _queueProcessorTask.Dispose();
            _queueProcessorTask = null;
            return true;
        }
    }
}


