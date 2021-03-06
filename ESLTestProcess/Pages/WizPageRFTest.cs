﻿using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                _testParameters.Add(new Tuple<string, string>("Background RSSI", TestViewParameters.RF_BGR_RSSI));
                _testParameters.Add(new Tuple<string, string>("HUB Acknowledgment", TestViewParameters.RF_HUB_ACK));
                _testParameters.Add(new Tuple<string, string>("Ack RSSI value", TestViewParameters.RF_ACK_RSSI));

                _activeTblLayoutPanel = tblTransceiverTest;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPageTransceiver_Enter(object sender, EventArgs e)
        {
            _testExpired = false;
            stepWizardControl1.SelectedPage.AllowNext = false;

            AddRetestLabelToWizard(wizardPageTransceiver);

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Background RSSI", TestViewParameters.RF_BGR_RSSI));
            _testParameters.Add(new Tuple<string, string>("HUB Acknowledgment", TestViewParameters.RF_HUB_ACK));
            _testParameters.Add(new Tuple<string, string>("Ack RSSI value", TestViewParameters.RF_ACK_RSSI));

            _activeTblLayoutPanel = tblTransceiverTest;
            _rssiStage = 0;

            ResetTestParameter(TestViewParameters.RF_BGR_RSSI);
            ResetTestParameter(TestViewParameters.RF_HUB_ACK);
            ResetTestParameter(TestViewParameters.RF_ACK_RSSI);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageTransceiver_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
            _timeOutTimer.Change(10000, Timeout.Infinite);
        }

        int _rssiStage = 0;
        
        void wizardPageTransceiver_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            byte[] rawData;
            response testResponse = null;

            switch (e.ResponseId)
            {
                case TestParameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                    break;

                case TestParameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_GET_BGRSSI_VALUE);
                    break;

                case TestParameters.TEST_ID_GET_BGRSSI_VALUE:

                    if (_rssiStage == 0)
                    {
                        string initialRssi = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8] }).Trim();

                        int bkgrRssi = int.Parse(initialRssi.Trim());

                        int bkgrRssiMax = int.Parse(ConfigurationManager.AppSettings["rssi_bkgr_max"]);
                        int bkgrRssiMin = int.Parse(ConfigurationManager.AppSettings["rssi_bkgr_min"]);
                        
                        if(bkgrRssi >= bkgrRssiMax && bkgrRssi <= bkgrRssiMin)
                            SetTestResponse(initialRssi, TestViewParameters.RF_BGR_RSSI, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(initialRssi, TestViewParameters.RF_BGR_RSSI, e.RawData, TestStatus.Fail);

                        CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_CAPTURE_HUB);
                    }
                    else
                    {
                        string ackRssiData = new string(new[] { (char)e.RawData[9], (char)e.RawData[10], (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13], (char)e.RawData[14], (char)e.RawData[15] }).Trim();

                        var ackRssi = int.Parse(ackRssiData.Trim());

                        int ackRssiMax = int.Parse(ConfigurationManager.AppSettings["rssi_ack_max"]);
                        int ackRssiMin = int.Parse(ConfigurationManager.AppSettings["rssi_ack_min"]);

                        if (ackRssi >= ackRssiMax && ackRssi <= ackRssiMin)
                            SetTestResponse(ackRssiData, TestViewParameters.RF_ACK_RSSI, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(ackRssiData, TestViewParameters.RF_ACK_RSSI, e.RawData, TestStatus.Fail);

                    }
                    break;

                case TestParameters.TEST_ID_CAPTURE_HUB:

                    _rssiStage = 1;

                    string hubAck = new string(new[] 
                    { 
                        (char)e.RawData[2],(char)e.RawData[3],(char)e.RawData[4],(char)e.RawData[5],(char)e.RawData[6],
                        (char)e.RawData[7],(char)e.RawData[8],(char)e.RawData[9],(char)e.RawData[10],(char)e.RawData[11], 
                        (char)e.RawData[12],(char)e.RawData[13],(char)e.RawData[14],(char)e.RawData[15],(char)e.RawData[16],
                        (char)e.RawData[17],(char)e.RawData[18],(char)e.RawData[19],(char)e.RawData[20],(char)e.RawData[21],
                        (char)e.RawData[22],(char)e.RawData[23]
                    }).Trim();

                    SetTestResponse(hubAck, TestViewParameters.RF_HUB_ACK, e.RawData, TestStatus.Pass);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_GET_BGRSSI_VALUE);
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
            ProcessControl.Instance.SaveTestSession();
            RemoveRetestLabelFromWizard(wizardPageTransceiver);
        }


        private void wizardPageTransceiver_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            wizardPageTransceiver_Leave(sender, e);
        }
    }
}
