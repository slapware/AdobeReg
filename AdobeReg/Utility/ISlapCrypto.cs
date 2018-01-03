using System;
using Microsoft.Extensions.Configuration; // for app settings

namespace AdobeReg.Utility
                  //namespace Microsoft.Extensions.DependencyInjection
{
    public interface ISlapCrypto
    {
        void setConfig(IConfiguration configuration);
        bool SetKey(string keyName);
        string Decrypt(string content);
        string Encrypt(string text);
    }
}
