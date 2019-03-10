using System;
using dotnetstandard_bip39;
using gxclient.Crypto;
using Cryptography.ECDSA;
using System.Linq;

namespace gxclient
{
    public class KeyPair
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string BrainKey { get; set; }
    }

    public class GXClient
    {
        private string PrivateKey { get; set; }
        private string AccountName { get; set; }
        private string EntryPoint { get; set; }
        private GXRPC RPC { get; set; }

        private static BIP39 bip39 = new BIP39();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:gxclient.GXClient"/> class.
        /// </summary>
        /// <param name="PrivateKey">Private key.</param>
        /// <param name="AccountName">Account name.</param>
        /// <param name="EntryPoint">Entry point.</param>
        public GXClient(String PrivateKey, String AccountName, String EntryPoint = "https://node1.gxb.io")
        {
            this.PrivateKey = PrivateKey;
            this.AccountName = AccountName;
            this.EntryPoint = EntryPoint;
            this.RPC = new GXRPC(this.EntryPoint);
        }

        /// <summary>
        /// Generates the key pair.
        /// </summary>
        /// <returns>The key pair.</returns>
        /// <param name="brainKey">Brain key.</param>
        public KeyPair GenerateKeyPair(string brainKey = null)
        {
            string brain_key = brainKey;
            if (brain_key == null)
            {
                brain_key = String.Join(" ", bip39.GenerateMnemonic(160, BIP39Wordlist.English).Split('\r').AsEnumerable<string>().Select(a =>
                {
                    return a.Trim();
                }).ToArray()).Trim();
            }
            byte[] private_key_bytes = Hash.SHA256(brain_key + " " + 0);
            Crypto.PrivateKey privateKey = Crypto.PrivateKey.FromBytes(private_key_bytes);

            return new KeyPair()
            {
                BrainKey = brain_key,
                PrivateKey = privateKey.ToWif(),
                PublicKey = privateKey.GetPublicKey().ToString()
            };
        }

        /// <summary>
        /// Export public key from private key
        /// </summary>
        /// <returns>Public key string.</returns>
        /// <param name="privateKey">Private key string.</param>
        public String PrivateToPublic(string privateKey)
        {
            return Crypto.PrivateKey.FromWif(privateKey).GetPublicKey().ToString();
        }

        /// <summary>
        /// Check if <paramref name="privateKey"/> is valid or not
        /// </summary>
        /// <returns><c>true</c>, if valid private was assigned, <c>false</c> otherwise.</returns>
        /// <param name="privateKey">Private key string.</param>
        public bool IsValidPrivate(string privateKey)
        {
            try
            {
                return Crypto.PrivateKey.FromWif(privateKey).ToWif()== privateKey;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if <paramref name="publicKey"/> is valid or not
        /// </summary>
        /// <returns><c>true</c>, if valid public was ised, <c>false</c> otherwise.</returns>
        /// <param name="publicKey">Public key.</param>
        public bool IsValidPublic(string publicKey)
        {
            try
            {
                return PublicKey.FromString(publicKey).ToString() == publicKey;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
