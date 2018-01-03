using System;
namespace AdobeReg.Utility
{
    public interface IAdobeGuid
    {
        string makeGuid();
        void setVendor(string vendor);
    }
}
