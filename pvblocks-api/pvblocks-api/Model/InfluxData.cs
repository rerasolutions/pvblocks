namespace pvblocks_api.Model
{
    public class InfluxData
    {
        public string Host { get; set; } = "";
        public string Organisation { get; set; } = "";
        public string Bucket { get; set; } = "";
        public string Token { get; set; } = "";
        public bool UseSsl { get; set; }
        public int Port { get; set; }
        public bool Enabled { get; set; } 
    }
}
