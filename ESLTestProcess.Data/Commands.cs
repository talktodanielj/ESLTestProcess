using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public static class Commands
    {
        public static readonly byte[] REQUEST_BEGIN_TEST = { 0x02, 0x60, 0x03 };
        public static readonly byte[] REQUEST_NODE_ID = { 0x02, 0x61, 0x03 };
        //public static readonly byte[] REQUEST_CHIP_IDS                  = { 0x02, 0x62, 0x03 };
        public static readonly byte[] REQUEST_BATTERY_LEVEL = { 0x02, 0x63, 0x03 };
        public static readonly byte[] REQUEST_TEMPERATURE_LEVEL = { 0x02, 0x64, 0x03 };
        public static readonly byte[] REQUEST_START_FLASH_GREEN_LED = { 0x02, 0x65, 0x03 };
        public static readonly byte[] REQUEST_START_FLASH_RED_LED = { 0x02, 0x66, 0x03 };
        public static readonly byte[] REQUEST_START_BUTTON_TEST = { 0x02, 0x67, 0x03 };
        public static readonly byte[] REQUEST_STOP_BUTTON_TEST = { 0x02, 0x68, 0x03 };
        public static readonly byte[] REQUEST_START_ACCELEROMETER_TEST = { 0x02, 0x69, 0x03 };
        public static readonly byte[] REQUEST_START_PIEZO_TEST = { 0x02, 0x70, 0x03 };
        public static readonly byte[] REQUEST_HUB_ID = { 0x02, 0x71, 0x03 };
        public static readonly byte[] REQUEST_SET_NODE_ID = { 0x02, 0x72, 0x00, 0x00, 0x00, 0x03 };
        public static readonly byte[] REQUEST_REED_SWITCH_TEST = { 0x02, 0x73, 0x03 };
        public static readonly byte[] REQUEST_SET_HUB_ID = { 0x02, 0x74, 0x00, 0x00, 0x03 };
        public static readonly byte[] REQUEST_DUMP_EPROM_TO_CONSOLE = { 0x02, 0x75, 0x03 };
        public static readonly byte[] REQUEST_RTC_VALUE = { 0x02, 0x76, 0x03 };

        //"YYMMDDHHMMSS"
        public static readonly byte[] REQUEST_SET_RTC_VALUE = { 0x02, 0x77, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 };
        public static readonly byte[] REQUEST_GET_BGRSSI_VALUE = { 0x02, 0x78, 0x03 };
        public static readonly byte[] REQUEST_CAPTURE_HUB = { 0x02, 0x79, 0x03 };
    }
}
