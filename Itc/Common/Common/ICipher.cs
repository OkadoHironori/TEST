namespace Itc.Common
{
    public interface ICipher
    {
        byte[] Encrypt(byte[] data);

        byte[] Decrypt(byte[] data);
    }
}
