using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public static class Commands
    {
        public static readonly byte[] REQUEST_BEGIN_TEST        = { 0x02, 0x60, 0x03 };
        public static readonly byte[] REQUEST_UNIT_ID           = { 0x02, 0x61, 0x03 };
        public static readonly byte[] REQUEST_CHIP_IDS          = { 0x02, 0x62, 0x03 };
        public static readonly byte[] REQUEST_BATTERY_LEVEL     = { 0x02, 0x63, 0x03 };
        public static readonly byte[] REQUEST_TEMPERATURE_LEVEL = { 0x02, 0x64, 0x03 };

    }
}
