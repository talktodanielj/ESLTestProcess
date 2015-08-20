using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public static class Commands
    {
        public static readonly byte[] REQUEST_BEGIN_TEST                = { 0x02, 0x60, 0x03 };
        public static readonly byte[] REQUEST_UNIT_ID                   = { 0x02, 0x61, 0x03 };
        public static readonly byte[] REQUEST_CHIP_IDS                  = { 0x02, 0x62, 0x03 };
        public static readonly byte[] REQUEST_BATTERY_LEVEL             = { 0x02, 0x63, 0x03 };
        public static readonly byte[] REQUEST_TEMPERATURE_LEVEL         = { 0x02, 0x64, 0x03 };
        public static readonly byte[] REQUEST_START_FLASH_GREEN_LED     = { 0x02, 0x65, 0x03 };
        public static readonly byte[] REQUEST_START_FLASH_RED_LED       = { 0x02, 0x66, 0x03 };
        public static readonly byte[] REQUEST_START_BUTTON_TEST         = { 0x02, 0x67, 0x03 };
        public static readonly byte[] REQUEST_STOP_BUTTON_TEST          = { 0x02, 0x68, 0x03 };
        public static readonly byte[] REQUEST_START_ACCELEROMETER_TEST  = { 0x02, 0x69, 0x03 };
        public static readonly byte[] REQUEST_START_PIEZO_TEST          = { 0x02, 0x70, 0x03 };

    }
}
