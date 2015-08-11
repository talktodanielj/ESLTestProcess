using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public static class TestParameters
    {
        public const string PCB_ID = "pcb_id";
        public const string EPROM_ID            = "eprom_id";
        public const string ACCELEROMETER_ID    = "accelerometer_id";
        public const string PIC24_ID            = "pic24_id";
        public const string TRANSCEVEIER_ID     = "transceveier_id";
        public const string BATTERY_VOLTAGE     = "battery_voltage";
        public const string TEMPERATURE_READING = "temperature_reading";

        public const string ACCELEROMETER_X_BASE     = "accelerometer_x_base";
        public const string ACCELEROMETER_Y_BASE = "accelerometer_y_base";
        public const string ACCELEROMETER_Z_BASE = "accelerometer_z_base";

        public const string ACCELEROMETER_X_LONG_EDGE = "accelerometer_x_long_edge";
        public const string ACCELEROMETER_Y_LONG_EDGE = "accelerometer_y_long_edge";
        public const string ACCELEROMETER_Z_LONG_EDGE = "accelerometer_z_long_edge";

        public const string ACCELEROMETER_X_SHORT_EDGE = "accelerometer_x_short_edge";
        public const string ACCELEROMETER_Y_SHORT_EDGE = "accelerometer_y_short_edge";
        public const string ACCELEROMETER_Z_SHORT_EDGE = "accelerometer_z_short_edge";


        public static string TRANS_RSSI         = "transceveier_rssi";
        public static string TRANS_MSG_TX       = "transceveier_tx";
        public static string TRANS_MSG_RX       = "transceveier_rx";

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
