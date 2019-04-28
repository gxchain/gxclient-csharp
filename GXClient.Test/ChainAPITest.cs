using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using gxclient;
using System.Threading.Tasks;
using gxclient.Models;
using Newtonsoft.Json;
using gxclient.Implements;
using gxclient.Crypto;

namespace GXClient.Test
{
    [TestClass]
    public class ChainAPITest
    {
        private readonly gxclient.GXClient Client = new gxclient.GXClient(new DefaultSignatureProvider("5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT"), new DefaultMemoProvider("5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT"), "gxb122", "https://testnet.gxchain.org");

        [TestMethod]
        public async Task GetChainID()
        {
            var chainId = await Client.GetChainId();
            Console.WriteLine(chainId);
            Assert.AreEqual<string>(chainId, "c2af30ef9340ff81fd61654295e98a1ff04b23189748f86727d0b26b40bb0ff4");
        }

        [TestMethod]
        public async Task GetDymamicGlobalProperties()
        {
            DynamicGlobalProperties dgp = await Client.GetDynamicGlobalPropertis();
            Console.WriteLine(JsonConvert.SerializeObject(dgp));
        }

        [TestMethod]
        public async Task GetObject()
        {
            var result = await Client.GetObject("1.3.1");
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task GetObjects()
        {
            var result = await Client.GetObjects(new string[] { "1.3.1", "1.2.1" });
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task GetBlock()
        {
            var result = await Client.GetBlock(12022594);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task Vote()
        {
            try
            {
                var transaction = await Client.Vote(new string[] { "w1", "w2" }, null, "GXC", true);
                Console.WriteLine(JsonConvert.SerializeObject(transaction, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
            }
        }
        [TestMethod]
        public async Task Transfer()
        {
            try
            {
                var transaction = await Client.Transfer("gxb121", "6不6", "10 GXC", "GXC", true);
                Console.WriteLine(JsonConvert.SerializeObject(transaction, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
            }
        }
        [TestMethod]
        public void MemoEncryptDecrypt()
        {
            string message = "6不6"; // including special charactors
            var result = AES.EncryptWithChecksum(PrivateKey.FromWif("5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT"), PublicKey.FromString("GXC8H1wXTAUWcTtogBmA5EW8TUWLA6T1kAXwMKYtnNuqAe1VCXFD9"), 16087047636745223546L, message);
            var msg = Hex.BytesToHex(result);
            Console.WriteLine(Hex.BytesToHex(result));
            string decryptedMsg = AES.DecryptWithChecksum(PrivateKey.FromWif("5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT"), PublicKey.FromString("GXC8H1wXTAUWcTtogBmA5EW8TUWLA6T1kAXwMKYtnNuqAe1VCXFD9"), 16087047636745223546L, Hex.HexToBytes(msg));
            Console.WriteLine(decryptedMsg);
            Assert.AreEqual(message, decryptedMsg);
        }
        [TestMethod]
        public void MemoEncryptDecrypt2()
        {
            string message = "hi你好"; // including special charactors
            var msg = "d504585e9d604bcf24a2ce19c220e5f7";
            string decryptedMsg = AES.DecryptWithChecksum(PrivateKey.FromWif("5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT"), PublicKey.FromString("GXC8H1wXTAUWcTtogBmA5EW8TUWLA6T1kAXwMKYtnNuqAe1VCXFD9"), 398450244318460L, Hex.HexToBytes(msg));
            Console.WriteLine(decryptedMsg);
            Assert.AreEqual(message, decryptedMsg);
        }
    }
}
