using Microsoft.VisualStudio.TestTools.UnitTesting;
using gxclient;
using System;
using System.Threading.Tasks;
using gxclient.Models;
using Newtonsoft.Json;

namespace GXClient.Test
{
    [TestClass]
    public class RPCTest
    {
        [TestMethod]
        public async Task Query()
        {
            GXRPC rpc = new GXRPC("https://testnet.gxchain.org");
            var result = await rpc.Query<Object>("get_objects", new object[] { new object[] { "2.1.0" } });
            Console.WriteLine(result);
        }

        [TestMethod]
        public async Task DGP()
        {
            GXRPC rpc = new GXRPC("https://testnet.gxchain.org");
            Console.WriteLine(DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
            var dgp = await rpc.Query<DynamicGlobalProperties>("get_dynamic_global_properties", null);
            Console.WriteLine(JsonConvert.SerializeObject(dgp, Formatting.Indented));
        }
    }
}
