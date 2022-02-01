using System.Text.Json;

namespace pvblocks_api.Model
{
    /// <summary>
    /// Stores a measurement with all values calibrated. One calibrated measurement will be stored per measurement and
    /// pv device.
    /// </summary>
    public class CalibratedMeasurement
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Photovoltaic Device that the measurement was made for
        /// </summary>
        public int PvDeviceId { get; set; }
        /// <summary>
        /// The specific measurement that this data was made on
        /// </summary>
        public int MeasurementId { get; set; }

        /// <summary>
        /// List of the calibrated measurement data
        /// </summary>
        public JsonDocument CalibratedMeasurementData { get; set; } = null!;

        public PvDevice PvDevice { get; set; } = null!;
        public Measurement Measurement { get; set; } = null!;
    }
}