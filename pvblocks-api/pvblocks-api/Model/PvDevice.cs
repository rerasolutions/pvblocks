using System.Collections.Generic;

namespace pvblocks_api.Model
{
    /// <summary>
    /// Photovoltaic device in the system. Measurements can be connected to this device
    /// </summary>
    public class PvDevice
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Name of this Pv Device
        /// </summary>
        public string Name { get; set; } = "PvDevice";

        /// <summary>
        /// Indicates if this is the meteo system
        /// </summary>
        public bool IsMeteoSystem { get; set; } = false;

        /// <summary>
        /// Serial number of this Pv Device
        /// </summary>
        public string Serial { get; set; } = "";

        /// <summary>
        /// Manufacturer of this Pv Device
        /// </summary>
        public string Manufacturer { get; set; } = "";
        /// <summary>
        /// Identification code used by manufacturer of this Pv Device
        /// </summary>
        public string ManufacturerCode { get; set; } = "";
        /// <summary>
        /// Solar cell material of this Pv Device
        /// </summary>
        public string Material { get; set; } = "";

        /// <summary>
        /// Set to indicate bifacial
        /// </summary>
        public bool? IsBiFacial { get; set; } = false;
        /// <summary>
        /// Open circuit voltage of this Pv Device
        /// </summary>
        public double? Voc { get; set; }
        /// <summary>
        /// Shortcircuit current of this Pv Device
        /// </summary>
        public double? Isc { get; set; }
        /// <summary>
        /// Peak power at STC of this Pv Device
        /// </summary>
        public double? Power { get; set; }
        /// <summary>
        /// Temperature coefficient of the Isc
        /// </summary>
        public double? Alpha { get; set; }
        /// <summary>
        /// Temperature coefficient of the Voc
        /// </summary>
        public double? Beta { get; set; }
        /// <summary>
        /// Temperature coefficient of Pmax
        /// </summary>
        public double? TemperatureCoefficient { get; set; }
        /// <summary>
        /// Total area of the module, including frame
        /// </summary>
        public double Area { get; set; }
        /// <summary>
        /// amount of single cells used in the module
        /// </summary>
        public int? CellCount { get; set; }
        /// <summary>
        /// SensorId to be used for STC/NOCT corrections
        /// </summary>
        public int? IrradianceId { get; set; }
        /// <summary>
        /// SensorId to be used for STC/NOCT corrections
        /// </summary>
        public int? TemperatureId { get; set; }
        /// <summary>
        /// amount of string that are used for the single cells
        /// </summary>
        public int? StringCount { get; set; }
        /// <summary>
        /// Flag to set if Pv Device is deleted
        /// </summary>
        public bool Deleted { get; set; } = false;
        
        /// <summary>
        /// Sensors attached to this Pv Device
        /// </summary>
        public List<AttachedSensor> AttachedSensors { get; set; } = null!;
    }
}