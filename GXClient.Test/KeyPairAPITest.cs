using Microsoft.VisualStudio.TestTools.UnitTesting;
using gxclient.Crypto;
using gxclient.Models;
using System;
using Newtonsoft.Json;

namespace GXClient.Test
{
    [TestClass]
    public class KeyPairAPITest
    {
        [TestMethod]
        public void GenerateKey()
        {
            gxclient.GXClient client = new gxclient.GXClient(null, null, "", "https://testnet.gxchain.org");
            KeyPair keyPair = client.GenerateKeyPair();
            Console.WriteLine(JsonConvert.SerializeObject(keyPair,Formatting.Indented));
            Assert.IsNotNull(keyPair.BrainKey);
        }

        [TestMethod]
        public void TestPrivateKey()
        {
            string privateKeyString = "5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT";
            PrivateKey privateKey = PrivateKey.FromWif(privateKeyString);
            Console.WriteLine(privateKey.ToString());
            Console.WriteLine(privateKey.GetPublicKey().ToString());
            Assert.AreEqual<string>(privateKeyString, privateKey.ToString(),"Private keys should be equal");
        }

        [TestMethod]
        public void TestPublicKey()
        {
            string publicKeyString = "GXC69R784krfXRuFYMuNwhTTnMGPMuCSSng3WPssL6vrXRqTYCLT4";
            PublicKey publicKey = PublicKey.FromString(publicKeyString);
            Console.WriteLine(publicKey.ToString());
            Assert.AreEqual<string>(publicKeyString, publicKey.ToString(),"Public keys should be equal");
        }

        [TestMethod]
        public void TestSharedSecret()
        {
            string privateKeyString = "5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT";
            PrivateKey privateKey = PrivateKey.FromWif(privateKeyString);
            var sharedSecret = privateKey.SharedSecret(privateKey.GetPublicKey());
            Console.WriteLine(Hex.BytesToHex(sharedSecret));
            Assert.AreEqual<string>(Hex.BytesToHex(sharedSecret), "53e731cf6c166a972962e776734f7afbfb320b1b26ddfc32125ecb7c6d6e37aa59f5b0e4ccad49630cc455a9830274c3e2ac7ae43f94f12449250f85e2311a1c", "Shared secret not expected");
        }

        [TestMethod]
        public void TestSignature()
        {
            string privateKeyString = "5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT";
            PrivateKey privateKey = PrivateKey.FromWif(privateKeyString);
            var sign = Signature.SignString("1", privateKey);
            Console.WriteLine(sign.ToHex());
        }
    }
}
