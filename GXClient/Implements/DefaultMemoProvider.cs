using System;
using System.Text;
using gxclient.Crypto;
using gxclient.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace gxclient.Implements
{
    public class DefaultMemoProvider : IMemoProvider
    {
        private PrivateKey PrivateKey { get; set; }

        public DefaultMemoProvider(PrivateKey privateKey)
        {
            this.PrivateKey = privateKey;
        }

        public DefaultMemoProvider(string privateKey)
        {
            this.PrivateKey = PrivateKey.FromWif(privateKey);
        }

        public Memo GenerateMemo(string publicKey, ulong nonce, string message)
        {
            return new Memo()
            {
                From = PrivateKey.GetPublicKey().ToString(),
                To = publicKey,
                Nonce = nonce,
                Message = Hex.BytesToHex(AES.EncryptWithChecksum(PrivateKey, PublicKey.FromString(publicKey), nonce, message))
            };
        }
    }
}
