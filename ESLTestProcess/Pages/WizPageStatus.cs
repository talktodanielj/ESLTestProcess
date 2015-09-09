using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESLTestProcess
{
    public partial class Main : Form
    {

        private void ResetTestParameter(string testParameter)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            if (testRun != null)
            {
                var testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == testParameter);
                if (testResponse != null)
                {
                    testResponse.response_outcome = (Int16)TestStatus.Unknown;
                    testResponse.response_raw = new byte[0];
                    testResponse.response_value = "Unknown";

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                }
            }
        }

        private enum STATUS_CHECK_STAGE
        {
            BEGIN,
            NODE_ID,
            HUB_ID,
            BATT_LEVEL,
            TEMP_LEVEL,
        }

        private STATUS_CHECK_STAGE _currentStage;


        private void wizardPageResultsStatus_Enter(object sender, EventArgs e)
        {
            _testExpired = false;

            ResetTestParameter(TestParameters.BATTERY_VOLTAGE);
            ResetTestParameter(TestParameters.TEMPERATURE_READING);
            ResetTestParameter(TestParameters.NODE_ID);
            ResetTestParameter(TestParameters.HUB_ID);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageResultsStatus_ProcessResponseEventHandler;

            _currentStage = STATUS_CHECK_STAGE.BEGIN;

            _endTask = false;

            _commandTask = new Task(() =>
            {
                while (true)
                {
                    if (_endTask)
                        break;

                    switch (_currentStage)
                    {
                        case STATUS_CHECK_STAGE.BEGIN:
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
                            break;

                        case STATUS_CHECK_STAGE.NODE_ID:
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_NODE_ID);
                            break;

                        case STATUS_CHECK_STAGE.HUB_ID:
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_HUB_ID);
                            break;

                        case STATUS_CHECK_STAGE.BATT_LEVEL:
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BATTERY_LEVEL);
                            break;

                        case STATUS_CHECK_STAGE.TEMP_LEVEL:
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_TEMPERATURE_LEVEL);
                            break;
                        default:
                            break;
                    }

                    System.Threading.Thread.Sleep(500);
                }
            });

            _commandTask.Start();

            _timeOutTimer.Change(5000, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
        }

        void wizardPageResultsStatus_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {

            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            byte[] rawData;
            response testResponse = null;

            switch (e.ResponseId)
            {
                case Parameters.PARSE_ERROR:

                    _log.Info("Got a parse error");
                    _currentStage = STATUS_CHECK_STAGE.BEGIN;
                    break;

                case Parameters.TEST_ID_BEGIN_TEST:

                    _currentStage = STATUS_CHECK_STAGE.NODE_ID;
                    _log.Info("Got begin test command");
                    break;

                case Parameters.TEST_ID_NODE_ID:
                    _currentStage = STATUS_CHECK_STAGE.HUB_ID;

                    string nodeId = new string(new[] { (char)e.RawData[3], (char)e.RawData[2], (char)e.RawData[5], (char)e.RawData[4], (char)e.RawData[7], (char)e.RawData[6] });

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.NODE_ID);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = nodeId;
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });

                    break;
                case Parameters.TEST_ID_HUB_ID:
                    _currentStage = STATUS_CHECK_STAGE.BATT_LEVEL;

                    string hubId = new string(new[] { (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[2], (char)e.RawData[3] });

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.HUB_ID);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = hubId;
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });


                    break;
                case Parameters.TEST_ID_BATTERY_LEVEL:
                    _currentStage = STATUS_CHECK_STAGE.TEMP_LEVEL;

                    rawData = e.RawData.Skip(2).Take(3).Reverse().ToArray();
                    string batteryData = new string(new[] { (char)rawData[0], (char)rawData[1], (char)rawData[2] });
                    int batLevel10mV = int.Parse(batteryData);
                    double batteryLevel = batLevel10mV / 100.0;

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.BATTERY_VOLTAGE);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = string.Format("{0:0.00}V", batteryLevel);
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });

                    _log.Info("Got battery level result");
                    break;
                case Parameters.TEST_ID_TEMPERATURE_LEVEL:

                    _endTask = true;

                    rawData = e.RawData.Skip(2).Take(4).ToArray();
                    string temperatureData = new string(new[] { (char)rawData[2], (char)rawData[3] });
                    int temperature = int.Parse(temperatureData);

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.TEMPERATURE_READING);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = string.Format("{0:0.0}C", temperature);
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });

                    _log.Info("Got temerature level");
                    break;
                default:
                    _log.Info("Unexpected test response");
                    break;
            }
        }

        private void wizardPageResultsStatus_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tbllnitialStatus.RowCount == 1)
            {

                ProcessControl.Instance.BeginNewTestRun();

                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Node Id", TestParameters.NODE_ID));
                _testParameters.Add(new Tuple<string, string>("Hub Id", TestParameters.HUB_ID));
                _testParameters.Add(new Tuple<string, string>("Battery Volatge", TestParameters.BATTERY_VOLTAGE));
                _testParameters.Add(new Tuple<string, string>("Temperature", TestParameters.TEMPERATURE_READING));

                _activeTblLayoutPanel = tbllnitialStatus;
                GenerateTable(_testParameters.ToArray());
            }

        }

        private void wizardPageResultsStatus_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;

            _endTask = true;
            _commandTask.Wait();
            _commandTask.Dispose();
            _commandTask = null;

            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageResultsStatus_ProcessResponseEventHandler;
        }

    }
}
