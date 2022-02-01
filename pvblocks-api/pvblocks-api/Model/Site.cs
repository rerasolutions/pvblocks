namespace pvblocks_api.Model
{
    public class Site
    {
        public int ID { get; set; }
        public string Name { get; set; } = "New Site";
        public string TimeZone { get; set; } = "Europe/Berlin";
        public string Description { get; set; }
        public double Latitude { get; set; } = 51.825;
        public double Longitude { get; set; } = 5.866;
        public double Elevation { get; set; } = 17.0;
        public string Owner { get; set; }
        public string Phonenumber{ get; set; }
        public string Email { get; set; }
    }
}
