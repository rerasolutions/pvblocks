namespace pvblocks_api.Model
{
    public class IvMeasurement
    {
        public IvData IvData { get; set; }
        public IvParameters IvParameters { get; set; }

        public IvMeasurement()
        {
            IvData = new IvData();
            IvParameters = new IvParameters();
        }
    }
}
