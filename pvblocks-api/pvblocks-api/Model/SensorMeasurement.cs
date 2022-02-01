using System;
using System.Collections.Generic;

namespace pvblocks_api.Model
{
    public class SensorMeasurement
    {
        public DateTime Timestamp { get; set; }
        public Dictionary<string, double> Readings { get; set; } = new Dictionary<string, double>();

    }
}
