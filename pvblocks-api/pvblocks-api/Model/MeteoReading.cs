using System.Collections.Generic;

namespace pvblocks_api.Model
{
    public class MeteoReading
    {
        public string Timestamp { get; set; }

        public Dictionary<MeteoDevice, double> MeteoSensors { get; set; } = new Dictionary<MeteoDevice, double>();
    }
}
