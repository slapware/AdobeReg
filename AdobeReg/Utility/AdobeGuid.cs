using System;

namespace AdobeReg.Utility
{
    public class AdobeGuid : IAdobeGuid
    {
        public string myguid { get; set; }
        public string avendorid { get; set; }

        public AdobeGuid()
        {
        }

        public void setVendor(string vendor)
        {
            this.avendorid = vendor;
        }
        public string makeGuid()
        {
            Guid g = Guid.NewGuid();
            string aguid = g.ToString("D");
            aguid = aguid.Insert(0, "0");
            aguid = aguid.Insert(14, "1");
            aguid = aguid.Substring(0, 23) + this.avendorid;
            this.myguid = "urn:uuid:" + aguid;
            return this.myguid;
        }
    }
}
