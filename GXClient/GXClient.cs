using System;
using dotnetstandard_bip39;
using gxclient.Crypto;
using Cryptography.ECDSA;
using System.Linq;
using System.Threading.Tasks;
using gxclient.Interfaces;
using Newtonsoft.Json;
using gxclient.Models;

namespace gxclient
{
    public class KeyPair
    {
        [JsonProperty("private_key")]
        public string PrivateKey { get; set; }
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
        [JsonProperty("brain_key")]
        public string BrainKey { get; set; }
    }

    public class GXClient
    {
        private ISignatureProvider SignatureProvider { get; set; }
        private string AccountName { get; set; }
        private string EntryPoint { get; set; }
        private GXRPC RPC { get; set; }

        private static BIP39 bip39 = new BIP39();

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:gxclient.GXClient"/> class.
        /// </summary>
        /// <param name="signatureProvider">SignatureProvider.</param>
        /// <param name="AccountName">Account name.</param>
        /// <param name="EntryPoint">Entry point.</param>
        public GXClient(ISignatureProvider signatureProvider, String AccountName, String EntryPoint = "https://node1.gxb.io")
        {
            this.SignatureProvider = signatureProvider;
            this.AccountName = AccountName;
            this.EntryPoint = EntryPoint;
            this.RPC = new GXRPC(this.EntryPoint);
        }
        #endregion

        #region keypair api
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
        #endregion

        #region chain api

        /// <summary>
        /// Query the specified method with parameter.
        /// </summary>
        /// <returns>Dictionary value</returns>
        /// <param name="method">Method.</param>
        /// <param name="parameter">Parameter.</param>
        /// <typeparam name="TData">The 1st type parameter.</typeparam>
        public async Task<TData> Query<TData>(string method, object parameter)
        {
            return await RPC.Query<TData>(method, parameter);
        }

        /// <summary>
        /// Broadcast the specified transaction.
        /// </summary>
        /// <returns>Transaction</returns>
        /// <param name="transaction">Transaction.</param>
        /// <typeparam name="TData">Transaction</typeparam>
        public async Task<TData> Broadcast<TData>(object transaction)
        {
            return await RPC.Broadcast<TData>(transaction);
        }

        /// <summary>
        /// GET ChainId of entry point
        /// </summary>
        /// <returns>ChainId.</returns>
        public async Task<string> GetChainId()
        {
            return await this.Query<string>("get_chain_id", null);
        }

        /// <summary>
        /// Gets dynamic global propertis of current blockchain
        /// </summary>
        /// <returns>The dynamic global propertis.</returns>
        public async Task<DynamicGlobalProperties> GetDynamicGlobalPropertis()
        {
            return await this.Query<DynamicGlobalProperties>("get_dynamic_global_properties", null);
        }

        #endregion
    }
}
