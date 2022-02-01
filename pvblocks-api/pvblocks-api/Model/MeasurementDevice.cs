using System.Collections.Generic;

namespace pvblocks_api.Model
{
    /// <summary>
    /// Device that connects to a connector in a PvBlock. Has sensors that measure things. 
    /// </summary>
    public class MeasurementDevice
    {
        public int Id { get; set; }

        /// <summary>
        /// PvBlock that this device is connected to
        /// </summary>
        public int PvBlockId { get; set; }
        /// <summary>
        /// Connector that this device is connected on
        /// </summary>
        public int Connector { get; set; } = 0;

        public PvBlock PvBlock { get; set; } = null!;

        public List<Sensor> Sensors { get; set; } = null!;
    }
}