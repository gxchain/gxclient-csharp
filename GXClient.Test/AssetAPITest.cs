using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using gxclient;
using System.Threading.Tasks;
using gxclient.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GXClient.Test
{
    [TestClass]
    public class AssetAPITest
    {
        private readonly gxclient.GXClient Client = new gxclient.GXClient(null, null, "", "https://testnet.gxchain.org");

        [TestMethod]
        public async Task GetAsset()
        {
            string assetName = "GXC";
            var result = await Client.GetAsset(assetName);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.AreEqual(result.Symbol, assetName);
        }

        [TestMethod]
        public async Task GetAssets()
        {
            string[] assets = new string[] { "GXC", "PPS" };
            var result = await Client.GetAssets(assets);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}
