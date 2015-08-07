using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public static class TestParameters
    {
        public const string EPROM_ID = "eprom_id";
        public const string ACCELEROMETER_ID = "accelerometer_id";
        public const string PIC24_ID = "pic24_id";
        public const string TRANSCEVEIER_ID = "transceveier_id";
        public const string BATTERY_VOLTAGE = "battery_voltage";
        public const string TEMPERATURE_READING = "temperature_reading";

        public const string ACCELEROMETER_X = "accelerometer_x";
        public const string ACCELEROMETER_Y = "accelerometer_y";
        public const string ACCELEROMETER_Z = "accelerometer_z";

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
