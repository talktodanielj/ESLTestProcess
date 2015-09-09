﻿using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESLTestProcess
{
    public partial class Main
    {
        private void wizardPageTransceiver_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblTransceiverTest.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Background RSSI", TestParameters.RF_BGR_RSSI));
                _testParameters.Add(new Tuple<string, string>("HUB Acknowledgment", TestParameters.RF_HUB_ACK));
                _testParameters.Add(new Tuple<string, string>("Ack RSSI value", TestParameters.RF_ACK_RSSI));

                _activeTblLayoutPanel = tblTransceiverTest;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPageTransceiver_Enter(object sender, EventArgs e)
        {
            _testExpired = false;
            _activeTblLayoutPanel = tblTransceiverTest;
            _rssiStage = 0;

            ResetTestParameter(TestParameters.RF_BGR_RSSI);
            ResetTestParameter(TestParameters.RF_HUB_ACK);
            ResetTestParameter(TestParameters.RF_ACK_RSSI);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageTransceiver_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
        }

        int _rssiStage = 0;
        string _initialRssi = "0";

        void wizardPageTransceiver_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            byte[] rawData;
            response testResponse = null;

            switch (e.ResponseId)
            {
                case Parameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
                    break;

                case Parameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_GET_BGRSSI_VALUE);
                    break;

                case Parameters.TEST_ID_GET_BGRSSI_VALUE:

                    if (_rssiStage == 0)
                    {
                        string initialRssi = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8] }).Trim();

                        testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.RF_BGR_RSSI);
                        testResponse.response_raw = e.RawData;
                        testResponse.response_value = initialRssi;
                        testResponse.response_outcome = (Int16)TestStatus.Pass;

                        TestResponseHandler(null, new TestResponseEventArgs
                        {
                            Parameter = testResponse.response_parameter,
                            Status = (TestStatus)testResponse.response_outcome,
                            Value = testResponse.response_value,
                            RawValue = testResponse.response_raw
                        });
                        CommunicationManager.Instance.SendCommand(Parameters.REQUEST_CAPTURE_HUB);
                    }
                    else
                    {
                        string ackRssiData = new string(new[] { (char)e.RawData[9], (char)e.RawData[10], (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13], (char)e.RawData[14], (char)e.RawData[15] }).Trim();

                        var rssi = int.Parse(_initialRssi);
                        var ackRssi = int.Parse(ackRssiData);

                        var responseOutcome = (Int16)TestStatus.Unknown;

                        if ((rssi - ackRssi) > -70)
                        {
                            responseOutcome = (Int16)TestStatus.Pass;
                        }
                        else
                        {
                            responseOutcome = (Int16)TestStatus.Fail;
                        }

                        testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.RF_ACK_RSSI);
                        testResponse.response_raw = e.RawData;
                        testResponse.response_value = ackRssiData;
                        testResponse.response_outcome = responseOutcome;

                        TestResponseHandler(null, new TestResponseEventArgs
                        {
                            Parameter = testResponse.response_parameter,
                            Status = (TestStatus)testResponse.response_outcome,
                            Value = testResponse.response_value,
                            RawValue = testResponse.response_raw
                        });
                    }
                    break;

                case Parameters.TEST_ID_CAPTURE_HUB:

                    _rssiStage = 1;

                    string hubAck = new string(new[] 
                    { 
                        (char)e.RawData[2],(char)e.RawData[3],(char)e.RawData[4],(char)e.RawData[5],(char)e.RawData[6],
                        (char)e.RawData[7],(char)e.RawData[8],(char)e.RawData[9],(char)e.RawData[10],(char)e.RawData[11], 
                        (char)e.RawData[12],(char)e.RawData[13],(char)e.RawData[14],(char)e.RawData[15],(char)e.RawData[16],
                        (char)e.RawData[17],(char)e.RawData[18],(char)e.RawData[19],(char)e.RawData[20],(char)e.RawData[21],
                        (char)e.RawData[22],(char)e.RawData[23]
                    }).Trim();

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.RF_HUB_ACK);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = hubAck;
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_GET_BGRSSI_VALUE);
                    break;

                default:
                    _log.Info("Unexpected test response");
                    break;
            }
        }

        private void wizardPageTransceiver_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageTransceiver_ProcessResponseEventHandler;
        }
    }
}
