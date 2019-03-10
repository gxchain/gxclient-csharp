using System.Security.Cryptography;
using System.Text;
namespace gxclient.Crypto
{
    public static class Hash
    {
        public static byte[] SHA256(string str)
        {
            return SHA256(Encoding.UTF8.GetBytes(str));
        }

        public static byte[] SHA256(byte[] data)
        {
            using (var hash = new SHA256Managed())
            {
                var result = hash.ComputeHash(data);
                return result;
            }
        }

        public static byte[] SHA512(string str)
        {
            return SHA512(Encoding.UTF8.GetBytes(str));
        }

        public static byte[] SHA512(byte[] data)
        {
            using (var hash = new SHA512Managed())
            {
                var result = hash.ComputeHash(data);
                return result;
            }
        }
    }
}
