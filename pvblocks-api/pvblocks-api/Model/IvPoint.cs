namespace pvblocks_api.Model
{ 
    public class IvPoint
    {
        public double Voltage { get; set; }
        public double Current { get; set; }

        public IvPoint(double voltage, double current)
        {
            Voltage = voltage;
            Current = current;
        }

    }
}