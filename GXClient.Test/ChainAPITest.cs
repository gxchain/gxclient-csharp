using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using gxclient;
using System.Threading.Tasks;

namespace GXClient.Test
{
    [TestClass]
    public class ChainAPITest
    {
        private readonly gxclient.GXClient Client = new gxclient.GXClient(null, "", "https://testnet.gxchain.org");

        [TestMethod]
        public async Task GetChainID()
        {
            var chainId = await Client.GetChainId();
            Console.WriteLine(chainId);
            Assert.AreEqual<string>(chainId, "c2af30ef9340ff81fd61654295e98a1ff04b23189748f86727d0b26b40bb0ff4");
        }
    }
}
