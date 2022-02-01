namespace pvblocks_api.Model
{
    public class MsXxReading
    {
        public int NodeId { get; set; }
        public double Irradiance { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Xtilt { get; set; }
        public double Ytilt { get; set; }

    }
}
