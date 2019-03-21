using System;
using System.Collections.Generic;
using System.Text;
using Cryptography.ECDSA;
using System.Linq;

namespace gxclient.Crypto
{
    public class PublicKey
    {
        public const int Size = 64;
        public byte[] Data = new byte[Size];
        public const string PREFIX = "GXC";

        public static PublicKey FromString(string publicKeyString)
        {
            var prefix = publicKeyString.Substring(0, 3);
            if (prefix != PREFIX)
            {
                throw new Exception("Prefix should be " + PREFIX + " but get " + prefix);
            }
            var public_key_with_checksum = Base58.Decode(publicKeyString.Substring(3));
            var checksum = public_key_with_checksum.Skip(public_key_with_checksum.Length - 4).Take(4).ToArray();
            var d = public_key_with_checksum.Take(public_key_with_checksum.Length - 4).ToArray();
            var new_checksum = Ripemd160Manager.GetHash(d).Take(4).ToArray();
            if (!checksum.SequenceEqual(new_checksum))
            {
                throw new Exception("Checksum did not match");
            }
            return new PublicKey()
            {
                Data = d
            };
        }

        public override string ToString()
        {
            var checksum = Ripemd160Manager.GetHash(this.Data).Take(4).ToArray();
            var public_key_with_checksum = this.Data.Concat(checksum).ToArray();
            return PREFIX + Base58.Encode(public_key_with_checksum);
        }

        public ECPoint ToPoint()
        {
           return ECPoint.Decode(this.Data);
        }
    }
}
