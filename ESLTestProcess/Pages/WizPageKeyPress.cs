﻿using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESLTestProcess
{
    public partial class Main : Form
    {
        int _activeKey = -1;
        bool _altColour = false;
        bool _gotBeginTestResult = false;

        private void FlashColourCallback(object sender)
        {
            if (_altColour)
                SetKeyColour(Color.YellowGreen, _activeKey);
            else
                SetKeyColour(Color.ForestGreen, _activeKey);

            _altColour = !_altColour;
        }

        private void SetKeyColour(Color activeColour, int selectedKey)
        {
            Button btnRef = null;
            switch (selectedKey)
            {
                case KEY_ENT:
                    btnRef = this.btnEnt;
                    break;
                case KEY_1_6:
                    btnRef = btn1_6;
                    break;
                case KEY_2_7:
                    btnRef = btn2_7;
                    break;
                case KEY_3_8:
                    btnRef = btn3_8;
                    break;
                case KEY_4_9:
                    btnRef = btn4_9;
                    break;
                case KEY_5_0:
                    btnRef = btn5_0;
                    break;
                default:
                    break;
            }
            if (btnRef != null)
                btnRef.BackColor = activeColour;
        }

        Task _commandTask;
        bool _endTask = false;

        private void wizardPageKeyPress_Enter(object sender, EventArgs e)
        {
            SetKeyColour(Color.ForestGreen, KEY_ENT);
            SetKeyColour(Color.ForestGreen, KEY_1_6);
            SetKeyColour(Color.ForestGreen, KEY_2_7);
            SetKeyColour(Color.ForestGreen, KEY_3_8);
            SetKeyColour(Color.ForestGreen, KEY_4_9);
            SetKeyColour(Color.ForestGreen, KEY_5_0);
            
            _flashColourTimer.Change(0, 500);
            _byteStreamHandler.ProcessResponseEventHandler += wizardPageKeyPress_ProcessResponseEventHandler;

            _activeKey = KEY_ENT;
            _gotBeginTestResult = false;
             _endTask = false;

             CommunicationManager.Instance.SendCommand(Parameters.REQUEST_NODE_ID);

            //_commandTask = new Task(() =>
            //{
            //    while (true)
            //    {
            //        if (!_gotBeginTestResult)
            //        {
            //            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_NODE_ID);
            //        }

            //        if (_endTask)
            //            break;

            //        System.Threading.Thread.Sleep(2000);
            //    }
            //});

            //_commandTask.Start();
        }

        private const int KEY_ENT = 2;
        private const int KEY_1_6 = 1;
        private const int KEY_2_7 = 32;
        private const int KEY_3_8 = 16;
        private const int KEY_4_9 = 8;
        private const int KEY_5_0 = 4;
        private const int KEY_END_TEST = 100;

        void wizardPageKeyPress_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            _gotBeginTestResult = true;

            if (e.ResponseId == Parameters.PARSE_ERROR)
            {
                _log.Info("Got a parse error");
                //_gotBeginTestResult = false;

                CommunicationManager.Instance.SendCommand(Parameters.REQUEST_NODE_ID);

                //byte[] commandBytes = Parameters.REQUEST_START_BUTTON_TEST;
                //commandBytes[2] = 10; // Give 10 seconds to complete the test
                //commandBytes[3] = (byte)KEY_5_0; // The final key in the sequence that indicates the test should stop
                //CommunicationManager.Instance.SendCommand(commandBytes);

            }
            else if (e.ResponseId == Parameters.TEST_ID_NODE_ID)
            {
                _log.Info("Got begin test command");
                Console.WriteLine("Sending keypress test command");

                byte[] commandBytes = Parameters.REQUEST_START_BUTTON_TEST;
                commandBytes[2] = 10; // Give 10 seconds to complete the test
                commandBytes[3] = (byte)KEY_5_0; // The final key in the sequence that indicates the test should stop
                CommunicationManager.Instance.SendCommand(commandBytes);

            }
            else if (e.ResponseId == Parameters.TEST_END)
            {
                _log.Info("Detected end of test");

                if (_activeKey == KEY_END_TEST)
                {
                    //_commandTask.Wait();
                    //_endTask = true;
                }
                else
                {
                    // Try the test again
                    _gotBeginTestResult = false;
                }
            }
            else if (e.ResponseId == Parameters.TEST_ID_BUTTON_TEST)
            {
                _gotBeginTestResult = true;

                var currentKey = _activeKey;

                switch (e.RawData[2])
                {
                    case KEY_1_6:
                        _log.Info("Key 1/6");
                        _activeKey = KEY_2_7;
                        break;
                    case KEY_ENT:
                        _log.Info("Key ENT");
                        _activeKey = KEY_1_6;
                        break;
                    case KEY_5_0:
                        _log.Info("Key 5/0");
                        _activeKey = KEY_END_TEST;
                        break;
                    case KEY_4_9:
                        _log.Info("Key 4/9");
                        _activeKey = KEY_5_0;
                        break;
                    case KEY_3_8:
                        _log.Info("Key 3/8");
                        _activeKey = KEY_4_9;
                        break;
                    case KEY_2_7:
                        _log.Info("Key 2/7");
                        _activeKey = KEY_3_8;
                        break;
                    default:
                        _log.Info("Invalid key code");
                        break;
                }

                SetKeyColour(Color.YellowGreen, currentKey);
            }
            else
            {
                _log.InfoFormat("Response id is {0}", (int)e.ResponseId);
            }
        }

        private void wizardPageKeyPress_Leave(object sender, EventArgs e)
        {
            _endTask = true;
            //_commandTask.Wait();
            //_commandTask.Dispose();
            //_commandTask = null;

            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageKeyPress_ProcessResponseEventHandler;
        }
    }
}
