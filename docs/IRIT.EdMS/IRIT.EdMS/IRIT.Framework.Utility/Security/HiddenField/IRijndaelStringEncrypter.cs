using System;

namespace IRIT.Framework.Utility.Security.HiddenField
{
    public interface IRijndaelStringEncrypter : IDisposable
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
