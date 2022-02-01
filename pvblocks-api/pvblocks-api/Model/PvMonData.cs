using System.Collections.Generic;

namespace pvblocks_api.Model
{
    public record Mc20(byte nodeId, double temperature, double reading, double irradiance);
    public record QiPower(byte nodeId, double voltage, double current, double energy);


    public class PvMonData
    {
        public List<Mc20> Mc20s { get; set; }
        public List<QiPower> QiPowers { get; set; }

        public PvMonData()
        {
            Mc20s = new List<Mc20>();
            QiPowers = new List<QiPower>();
        }


    }
}
