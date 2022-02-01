namespace pvblocks_api.Model
{
    public class IrradianceSensorOptions
    {
        public string Serial { get; set; } = "";
        public string IrradianceType { get; set; } = "";
        public string Model { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string SensorType { get; set; } = "";
        public double TiltAngle { get; set; } = 45;
        public double Azimuth { get; set; } = 180;
    }
}
