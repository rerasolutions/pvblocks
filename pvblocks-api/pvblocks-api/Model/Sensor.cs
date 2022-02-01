using System.Collections.Generic;
using System.Text.Json;

namespace pvblocks_api.Model
{
    /// <summary>
    /// Sensor that measures data and can be calibrated
    /// </summary>
    public class Sensor
    {
        public int Id { get; set; }

        /// <summary>
        /// Indicates if the sensor is being used by the application
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Measurement device that this sensor is on
        /// </summary>
        public int MeasurementDeviceId { get; set; }
        /// <summary>
        /// Name of the sensor
        /// Also used as measurementtype:
        /// temperature, irradiance, voltage, current, power
        /// spectrum, image
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Description for the sensor
        /// </summary>
        public string Description { get; set; } = null!;
        /// <summary>
        /// Unit of the measured value.
        /// Using the UnitsNet library
        /// </summary>
        public string Unit { get; set; } = null!;
        /// <summary>
        /// Calibration data for this sensor
        /// </summary>
        public JsonDocument Calibration { get; set; } = null!;
        /// <summary>
        /// Configuration data for this sensor
        /// </summary>
        public JsonDocument Options { get; set; } = null!;

        public MeasurementDevice MeasurementDevice { get; set; } = null!;
        
        public List<AttachedSensor> AttachedSensors { get; set; } = null!;
    }
}