using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public static class TestViewParameters
    {
        public const string NODE_ID = "node_id";
        public const string HUB_ID     = "hub_id";
        public const string BATTERY_VOLTAGE     = "battery_voltage";
        public const string TEMPERATURE_READING = "temperature_reading";

        public const string LED_GREEN_FLASH = "led_green_flash";
        public const string LED_RED_FLASH = "led_red_flash";
        
        public const string ACCELEROMETER_X_BASE = "accelerometer_x_base";
        public const string ACCELEROMETER_Y_BASE = "accelerometer_y_base";
        public const string ACCELEROMETER_Z_BASE = "accelerometer_z_base";

        public const string ACCELEROMETER_X_LONG_EDGE = "accelerometer_x_long_edge";
        public const string ACCELEROMETER_Y_LONG_EDGE = "accelerometer_y_long_edge";
        public const string ACCELEROMETER_Z_LONG_EDGE = "accelerometer_z_long_edge";

        public const string ACCELEROMETER_X_SHORT_EDGE = "accelerometer_x_short_edge";
        public const string ACCELEROMETER_Y_SHORT_EDGE = "accelerometer_y_short_edge";
        public const string ACCELEROMETER_Z_SHORT_EDGE = "accelerometer_z_short_edge";

        public static string RF_ACK_RSSI    = "rf_ack_rssi";
        public static string RF_BGR_RSSI    = "rf_bgr_rssi";
        public static string RF_HUB_ACK     = "rf_hub_ack";

        public static string PIEZO_TEST = "piezo_test";
        public static string REED_TEST = "reed_test";
        public static string RTC_SET = "rtc_set";
        public static string RTC_GET = "rtc_get";

        public static string KEY_ENT = "key_ent";
        public static string KEY_1_6 = "key_1_6";
        public static string KEY_2_7 = "key_2_7";
        public static string KEY_3_8 = "key_3_8";
        public static string KEY_4_9 = "key_4_9";
        public static string KEY_5_0 = "key_5_0";

        public static string RUN_CURRENT = "run_current";
        public static string SLEEP_CURRENT = "sleep_current";
        public static string VOLTAGE_SUPPLY = "voltage_supply";
        public static string EXT_SK3_TEST1 = "ext_sk3_test_1";
        
        public static string GetStatusName(string parameter)
        {
            return parameter + "_status";
        }

        public static string GetIconName(string parameter)
        {
            return parameter + "_icon";
        }
    }
}
