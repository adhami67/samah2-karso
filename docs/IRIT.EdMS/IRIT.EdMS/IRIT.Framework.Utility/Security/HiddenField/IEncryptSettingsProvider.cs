namespace IRIT.Framework.Utility.Security.HiddenField
{
    public interface IEncryptSettingsProvider
    {
        byte[] EncryptionKey { get; }
        string EncryptionPrefix { get; }
    }
}
