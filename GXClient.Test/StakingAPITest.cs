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
    public class StakingAPITest
    {
        private readonly gxclient.GXClient Client = new gxclient.GXClient(null, null, "", "https://testnet.gxchain.org");

        [TestMethod]
        public async Task GetStakingPrograms()
        {
            var stakingPrograms = await Client.GetStakingPrograms();
            Console.WriteLine(JsonConvert.SerializeObject(stakingPrograms, Formatting.Indented));
        }
    }
}
