using System;
using System.Linq;

namespace pvblocks_api.Model
{ 
    public class IvArrayData
    {
        public Int32 TimeStamp { get; set; }
        
        public double[] Voltages { get; set; }
        public double[] Currents { get; set; }


        public IvArrayData()
        {
            
        }
        
        public IvArrayData(IvData ivData)
        {
            Voltages = ivData.IvPoints.Select(p => p.Voltage).ToArray();
            Currents = ivData.IvPoints.Select(p => p.Current).ToArray();

            if (Voltages.Length != Currents.Length)
                throw new Exception("Voltage and current count are not equal.");

            TimeStamp = ivData.TimeStamp;
        }



      
    }
}