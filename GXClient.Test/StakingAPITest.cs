using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using gxclient;
using System.Threading.Tasks;
using gxclient.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using gxclient.Implements;

namespace GXClient.Test
{
    [TestClass]
    public class StakingAPITest
    {
        private readonly gxclient.GXClient Client = new gxclient.GXClient(new DefaultSignatureProvider("5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT"), new DefaultMemoProvider("5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT"), "gxb122", "https://testnet.gxchain.org");
        [TestMethod]
        public async Task GetStakingPrograms()
        {
            var stakingPrograms = await Client.GetStakingPrograms();
            Console.WriteLine(JsonConvert.SerializeObject(stakingPrograms, Formatting.Indented));
        }

        [TestMethod]
        public async Task CreateStaking()
        {
            try
            {
                var tx = await Client.CreateStaking("init1", 10.0f, "5", "GXC", true);
                Console.WriteLine(JsonConvert.SerializeObject(tx, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
            }
        }

        [TestMethod]
        public async Task UpdateStaking()
        {
            try
            {
                var tx = await Client.UpdateStaking("init1", "1.27.10136", "GXC", true);
                Console.WriteLine(JsonConvert.SerializeObject(tx, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
            }
        }

        [TestMethod]
        public async Task ClaimStaking()
        {
            try
            {
                var tx = await Client.ClaimStaking("1.27.10126", "GXC", true);
                Console.WriteLine(JsonConvert.SerializeObject(tx, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
            }
        }
    }
    
}
