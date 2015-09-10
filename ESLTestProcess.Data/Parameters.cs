using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public class Response
    {
        public byte TestId;
        public int ExpectedLength;
    }

    public static class Parameters
    {
        public const byte TEST_ID_BEGIN_TEST = 0x60;
        public const byte TEST_ID_NODE_ID = 0x61;
        public const byte TEST_ID_BATTERY_LEVEL = 0x63;
        public const byte TEST_ID_TEMPERATURE_LEVEL = 0x64;
        public const byte TEST_ID_START_FLASH_GREEN_LED = 0x65;
        public const byte TEST_ID_START_FLASH_RED_LED = 0x66;
        public const byte TEST_ID_BUTTON_TEST = 0x67;
        public const byte TEST_ID_START_ACCELEROMETER_TEST = 0x69;
        public const byte TEST_ID_START_PIEZO_TEST = 0x70;
        public const byte TEST_ID_HUB_ID = 0x71;
        public const byte TEST_ID_SET_NODE_ID = 0x72;
        public const byte TEST_ID_REED_SWITCH_TEST = 0x73;
        public const byte TEST_ID_SET_HUB_ID = 0x74;
        public const byte TEST_ID_DUMP_EPROM_TO_CONSOLE = 0x75;
        public const byte TEST_ID_RTC_VALUE = 0x76;
        public const byte TEST_ID_SET_RTC_VALUE = 0x77;
        public const byte TEST_ID_GET_BGRSSI_VALUE = 0x78;
        public const byte TEST_ID_CAPTURE_HUB = 0x79;

        public const byte TEST_END = 0x50;
        public const byte PARSE_ERROR = 0x51;

        public static readonly byte[] REQUEST_BEGIN_TEST = { 0x02, TEST_ID_BEGIN_TEST, 0x03 };
        public static readonly byte[] REQUEST_NODE_ID = { 0x02, TEST_ID_NODE_ID, 0x03 };
        public static readonly byte[] REQUEST_BATTERY_LEVEL = { 0x02, TEST_ID_BATTERY_LEVEL, 0x03 };
        public static readonly byte[] REQUEST_TEMPERATURE_LEVEL = { 0x02, TEST_ID_TEMPERATURE_LEVEL, 0x03 };
        public static readonly byte[] REQUEST_START_FLASH_GREEN_LED = { 0x02, TEST_ID_START_FLASH_GREEN_LED, 0x03 };
        public static readonly byte[] REQUEST_START_FLASH_RED_LED = { 0x02, TEST_ID_START_FLASH_RED_LED, 0x03 };
                                                                                                             // Test duration, Expected Key   
        public static readonly byte[] REQUEST_START_BUTTON_TEST = { 0x02, TEST_ID_BUTTON_TEST, 0x02, 0x00, 0x03 };

        public static readonly byte[] REQUEST_START_ACCELEROMETER_TEST = { 0x02, TEST_ID_START_ACCELEROMETER_TEST, 0x05, 0x03 }; // Test duration default 5 seconds
        public static readonly byte[] REQUEST_START_PIEZO_TEST = { 0x02, TEST_ID_START_PIEZO_TEST, 0x05, 0x03 }; //Test duration default 5 seconds
        public static readonly byte[] REQUEST_HUB_ID = { 0x02, TEST_ID_HUB_ID, 0x03 };
        public static readonly byte[] REQUEST_SET_NODE_ID = { 0x02, TEST_ID_SET_NODE_ID, 0x00, 0x00, 0x00, 0x03 };
        public static readonly byte[] REQUEST_REED_SWITCH_TEST = { 0x02, TEST_ID_REED_SWITCH_TEST, 0x05, 0x03 }; //Test duration default 5 seconds
        public static readonly byte[] REQUEST_SET_HUB_ID = { 0x02, TEST_ID_SET_HUB_ID, 0x00, 0x00, 0x03 };
        public static readonly byte[] REQUEST_DUMP_EPROM_TO_CONSOLE = { 0x02, TEST_ID_DUMP_EPROM_TO_CONSOLE, 0x03 };
        public static readonly byte[] REQUEST_RTC_VALUE = { 0x02, TEST_ID_RTC_VALUE, 0x03 };

        //"YYMMDDHHMMSS"
        public static readonly byte[] REQUEST_SET_RTC_VALUE = { 0x02, TEST_ID_SET_RTC_VALUE, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 };
        public static readonly byte[] REQUEST_GET_BGRSSI_VALUE = { 0x02, TEST_ID_GET_BGRSSI_VALUE, 0x03 };
        public static readonly byte[] REQUEST_CAPTURE_HUB = { 0x02, TEST_ID_CAPTURE_HUB, 0x03 };

        public static Response[] ResponseValues = 
        {
            new Response{ TestId = TEST_END, ExpectedLength = 3},
            new Response{ TestId = PARSE_ERROR, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_BEGIN_TEST, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_NODE_ID, ExpectedLength = 9},
            new Response{ TestId = TEST_ID_BATTERY_LEVEL, ExpectedLength = 7},
            new Response{ TestId = TEST_ID_TEMPERATURE_LEVEL, ExpectedLength = 7},
            new Response{ TestId = TEST_ID_START_FLASH_GREEN_LED, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_START_FLASH_RED_LED, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_BUTTON_TEST, ExpectedLength = 4},
            new Response{ TestId = TEST_ID_START_ACCELEROMETER_TEST, ExpectedLength = 27},
            new Response{ TestId = TEST_ID_START_PIEZO_TEST, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_HUB_ID, ExpectedLength = 7},
            new Response{ TestId = TEST_ID_SET_NODE_ID, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_REED_SWITCH_TEST, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_SET_HUB_ID, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_DUMP_EPROM_TO_CONSOLE, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_RTC_VALUE, ExpectedLength = 20},
            new Response{ TestId = TEST_ID_SET_RTC_VALUE, ExpectedLength = 3},
            new Response{ TestId = TEST_ID_GET_BGRSSI_VALUE, ExpectedLength = 17},
            new Response{ TestId = TEST_ID_CAPTURE_HUB, ExpectedLength = 27}
        };
    }
}
