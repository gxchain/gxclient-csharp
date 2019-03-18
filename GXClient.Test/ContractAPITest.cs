using System;
using System.Linq;
using System.Threading.Tasks;
using gxclient;
using gxclient.Implements;
using gxclient.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace GXClient.Test
{
    [TestClass]
    public class ContractAPITest
    {
        private readonly gxclient.GXClient Client = new gxclient.GXClient(new DefaultSignatureProvider("5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT"), new DefaultMemoProvider("5J7Yu8zZD5oV9Ex7npmsT3XBbpSdPZPBKBzLLQnXz5JHQVQVfNT"), "gxb122", "https://testnet.gxchain.org");

        [TestMethod]
        public async Task GetContractABI()
        {
            Abi abi = await Client.GetContractABI("bank");
            Console.WriteLine(JsonConvert.SerializeObject(abi,Formatting.Indented));
        }

        [TestMethod]
        public async Task GetContractTables()
        {
            Table[] tables = (await Client.GetContrctTables("redpacket")).ToArray();
            Console.WriteLine(JsonConvert.SerializeObject(tables, Formatting.Indented));
        }

        [TestMethod]
        public async Task SerializeContractParams()
        {
            string result = await Client.SerializeContractParams("redpacket", "issue", new
            {
                pubkey = "GXC5NEGqM8BTnMm5NT7Vv2Shxh4eg4tk1kfmAUf3EGHtksig5vZdN",
                number = 3
            });
            Console.WriteLine(result);
            Assert.AreEqual(result, "35475843354e4547714d3842546e4d6d354e54375676325368786834656734746b316b666d41556633454748746b73696735765a644e0300000000000000");
        }

        [TestMethod]
        public async Task CallContract()
        {
            TransactionResult result = await Client.CallContract("redpacket", "issue", new
            {
                pubkey = "GXC5NEGqM8BTnMm5NT7Vv2Shxh4eg4tk1kfmAUf3EGHtksig5vZdN",
                number = 3
            }, "30 GXC", "GXC", false);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

    }
}
