using Cryptography.ECDSA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using gxclient.Crypto.Extensions;
using Newtonsoft.Json;

namespace gxclient.Crypto
{
    public class PrivateKey
    {
        public BigInteger D { get; set; }

        public static byte Version = 0x80;

        public PrivateKey(BigInteger d)
        {
            this.D = d;
        }
        /// <summary>
        /// Initialize with Wallet import format
        /// </summary>
        /// <param name="wif">Wallet import formated string</param>
        /// <returns>An instance of private key</returns>
        public static PrivateKey FromWif(string wif)
        {
            var keyBytes = Base58.Decode(wif);
            byte version = keyBytes[0];
            if (version != PrivateKey.Version)
            {
                throw new Exception("Expected version 128, instead got " + version);
            }
            var checksum = keyBytes.Skip(keyBytes.Length - 4).Take(4).ToArray();
            var priv_key_with_version = keyBytes.Take(keyBytes.Length - 4).ToArray();
            var new_checksum = Sha256Manager.GetHash(Sha256Manager.GetHash(priv_key_with_version)).Take(4).ToArray();
            if (!checksum.SequenceEqual(new_checksum))
            {
                throw new Exception("Checksum did not match");
            }
            var priv_key = priv_key_with_version.Skip(1).ToArray();
            return PrivateKey.FromBytes(priv_key);
        }

        /// <summary>
        /// Alia of FromWif
        /// </summary>
        /// <returns>An instance of private key</returns>
        /// <param name="wif">Wif.</param>
        public static PrivateKey FromString(string wif)
        {
            return FromWif(wif);
        }

        /// <summary>
        /// Initialize with byte array
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static PrivateKey FromBytes(byte[] bytes)
        {
            return new PrivateKey(bytes.ToBigIntegerUnsigned(true));
        }

        /// <summary>
        /// Export as Base58 encoded wallet import format
        /// </summary>
        /// <returns></returns>
        public string ToWif()
        {
            var priv_key_with_version = new byte[] { PrivateKey.Version }.Concat(this.D.ToByteArrayUnsigned(true)).ToArray();
            var checksum = Sha256Manager.GetHash(Sha256Manager.GetHash(priv_key_with_version)).Take(4).ToArray();
            var private_wif = priv_key_with_version.Concat(checksum).ToArray();
            return Base58.Encode(private_wif);
        }

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <returns>The public key.</returns>
        public PublicKey GetPublicKey()
        {
            var pubKeyByes = Secp256K1Manager.GetPublicKey(this.D.ToByteArrayUnsigned(true), true);
            return new PublicKey() { Data = pubKeyByes };
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:gxclient.Crypto.PrivateKey"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:gxclient.Crypto.PrivateKey"/>.</returns>
        public override string ToString()
        {
            return ToWif();
        }

        /// <summary>
        /// Shared secret calculated by ECDH algorithm
        /// </summary>
        /// <returns>The secret.</returns>
        /// <param name="publicKey">Public key.</param>
        public byte[] SharedSecret(PublicKey publicKey)
        {
            ECPoint point = publicKey.ToPoint();
            ECPoint sharedPoint = point.Multiply(this.D);
            byte[] xValue = sharedPoint.X.ToByteArrayUnsigned(true);
            if (xValue.Length < 32)
            {
                Console.WriteLine($"Length {xValue.Length} less than 32, will fill by 0");
                do
                {
                    xValue = new byte[] { 0 }.Concat(xValue).ToArray();
                } while (xValue.Length < 32);
            }
            return Hash.SHA512(xValue);
        }
    }
}
