using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public static class Responses
    {
        public static readonly byte[] RESPONSE_BEGIN_TEST = { 0x02, 0x60, (byte)'B', (byte)'E', (byte)'G', (byte)'I', (byte)'N', 0x03 };
        public static readonly byte[] RESPONSE_UNIT_ID = { 0x02, 0x61, 0x03 };
        public static readonly byte[] RESPONSE_CHIP_IDS = { 0x02, 0x62, 0x03 };
        public static readonly byte[] RESPONSE_BATTERY_LEVEL = { 0x02, 0x63, 0x03 };
        public static readonly byte[] RESPONSE_TEMPERATURE_LEVEL = { 0x02, 0x64, 0x03 };

        public static readonly byte[] RESPONSE_NOT_RECOGNISED = { 0x02, 0x70, 0x03 };
    }
}
