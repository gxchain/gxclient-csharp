using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using Cryptography.ECDSA;
using System.Text;
using gxclient.Crypto.Extensions;

namespace gxclient.Crypto
{
    public class Signature
    {
        public byte I { get; set; }
        public BigInteger R { get; set; }
        public BigInteger S { get; set; }

        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            byte[] result = new byte[] { this.I };
            return result.Concat(this.R.ToByteArray()).Concat(this.S.ToByteArray()).ToArray();
        }

        /// <summary>
        /// Convert to hex string
        /// </summary>
        /// <returns></returns>
        public string ToHex()
        {
            var bytes = ToBytes();
            return Hex.BytesToHex(bytes);
        }

        public override string ToString()
        {
            return ToHex();
        }

        /// <summary>
        /// Sign a string
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="message">Message.</param>
        /// <param name="privateKey">Private key.</param>
        public static Signature SignString(string message,PrivateKey privateKey)
        {
            return SignBytes(Encoding.UTF8.GetBytes(message),privateKey);
        }

        /// <summary>
        /// ECDSA with secp256k1
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="privateKey">Private key</param>
        /// <returns></returns>
        public static Signature SignBytes(byte[] message, PrivateKey privateKey)
        {
            var sign = Secp256K1Manager.SignCompressedCompact(Sha256Manager.GetHash(message), privateKey.D.ToByteArrayUnsigned(true));
            return Signature.FromBytes(sign);
        }

        /// <summary>
        /// Initilize from bytes
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static Signature FromBytes(byte[] sign)
        {
            return new Signature()
            {
                I = sign[0],
                R = new BigInteger(sign.Skip(1).Take(32).ToArray()),
                S = new BigInteger(sign.Skip(33).Take(32).ToArray())
            };
        }

        /// <summary>
        /// Initialize from hex string
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Signature FromHex(string hex)
        {
            var bytes = Hex.HexToBytes(hex);
            return Signature.FromBytes(bytes);
        }
        
    }
}
