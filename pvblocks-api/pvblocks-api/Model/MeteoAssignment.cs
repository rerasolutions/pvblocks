using System.Collections.Generic;

namespace pvblocks_api.Model
{

    public class MeteoAssignment
    {
        public Dictionary<MeteoDevice, int> MeteoSensors { get; set; } = new Dictionary<MeteoDevice, int>();
    }
}
