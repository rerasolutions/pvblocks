using System;
using System.Collections.Generic;

namespace pvblocks_api.Model
{
    public class IvData
    {
        public Int32 TimeStamp { get; set; } = new();

        public List<IvPoint> IvPoints { get; set; } = new();

        public IvData()
        {
            // empty constructor for json
        }

        public IvData(List<double> voltages, List<double> currents)
        {
            if (voltages.Count != currents.Count)
                throw new Exception("Voltage and current count are not equal.");

            IvPoints = new List<IvPoint>();
            for (int i = 0; i < voltages.Count; i++)
            {
                IvPoints.Add(new IvPoint(voltages[i], currents[i]));
            }
        }

    }
}