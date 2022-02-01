namespace pvblocks_api.Model
{
    /// <summary>
    /// Attach a sensor to a Pv Device
    /// </summary>
    public class AttachedSensor
    {
        public int Id { get; set; }
        /// <summary>
        /// Sensor that is attached to a Pv Device
        /// </summary>
        public int SensorId { get; set; }
        /// <summary>
        /// Pv Device the sensor is attached to
        /// </summary>
        public int PvDeviceId { get; set; }

        public Sensor Sensor { get; set; } = null!;
        public PvDevice PvDevice { get; set; } = null!;
    }
}