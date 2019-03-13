using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using gxclient;
using System.Threading.Tasks;
using gxclient.Models;
using Newtonsoft.Json;

namespace GXClient.Test
{
    [TestClass]
    public class FaucetAPITest
    {
        private readonly gxclient.GXClient Client = new gxclient.GXClient(null, "", "https://testnet.gxchain.org");

        [TestMethod]
        public async Task Register()
        {
            try
            {
                var keypair = Client.GenerateKeyPair();
                var tx = await Client.RegisterAccount("gxb-test-register-1", keypair.PublicKey, keypair.PublicKey, keypair.PublicKey, "https://testnet.faucet.gxchain.org");
                Console.WriteLine(JsonConvert.SerializeObject(tx, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
