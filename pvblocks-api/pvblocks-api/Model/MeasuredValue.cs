using System.Text.Json;

namespace pvblocks_api.Model
{
    /// <summary>
    /// Measured Value that is measured for a measurement in a pipeline
    /// </summary>
    public class MeasuredValue
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Specific measurement this value was measured for 
        /// </summary>
        public int MeasurementId { get; set; }
        /// <summary>
        /// Sensor this value was measured on
        /// </summary>
        public int SensorId { get; set; }

        /// <summary>
        /// The measured value
        /// </summary>
       
        public JsonDocument Value { get; set; } = null!;

        public Measurement Measurement { get; set; } = null!;
        public Sensor Sensor { get; set; } = null!;
    }
}