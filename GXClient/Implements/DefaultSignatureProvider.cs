using System;
using System.Text;
using gxclient.Crypto;
using gxclient.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace gxclient.Implements
{
    public class DefaultSignatureProvider : ISignatureProvider
    {
        private PrivateKey[] PrivateKeys { get; set; }

        public DefaultSignatureProvider(PrivateKey[] privateKeys)
        {
            this.PrivateKeys = privateKeys;
        }

        public DefaultSignatureProvider(string[] privateKeys)
        {
            this.PrivateKeys = privateKeys.Select(PrivateKey.FromWif).ToArray();
        }

        public DefaultSignatureProvider(PrivateKey privateKey)
        {
            this.PrivateKeys = new PrivateKey[] { privateKey };
        }

        public DefaultSignatureProvider(string privateKey)
        {
            this.PrivateKeys = new PrivateKey[] { PrivateKey.FromWif(privateKey) };
        }

        public Task<IEnumerable<string>> Sign(string chainId, byte[] serializedTransaction)
        {
            var packedPayload = Hex.HexToBytes(chainId).Concat(serializedTransaction).ToArray();

            return Task.FromResult(PrivateKeys.Select(key =>
            {
                return Signature.SignBytes(packedPayload, key).ToHex();
            }));
        }
    }
}
