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
    public class AccountAPITest
    {
        private readonly gxclient.GXClient Client = new gxclient.GXClient(null, null, "", "https://testnet.gxchain.org");

        [TestMethod]
        public async Task GetAccount()
        {
            string accountName = "gxb122";
            var account = await Client.GetAccount(accountName);
            Console.WriteLine(JsonConvert.SerializeObject(account, Formatting.Indented));
            Assert.AreEqual(account.Name, accountName);
        }

        [TestMethod]
        public async Task GetAccounts()
        {
            string[] accountNames = new string[] { "gxb122", "gxb121" };
            var account = await Client.GetAccounts(accountNames);
            Console.WriteLine(JsonConvert.SerializeObject(account, Formatting.Indented));
        }

        [TestMethod]
        public async Task GetVoteIDs()
        {
            string[] accountNames = new string[] { "bob", "bao" };
            var account = await Client.GetVoteIdsByAccounts(accountNames);
            Console.WriteLine(JsonConvert.SerializeObject(account, Formatting.Indented));
        }

        [TestMethod]
        public async Task GetAccountBalances()
        {
            string accountName = "gxb122";
            var accountBalances = await Client.GetAccountBalances(accountName);
            Console.WriteLine(JsonConvert.SerializeObject(accountBalances, Formatting.Indented));
        }

        [TestMethod]
        public async Task GetAccountByPublicKey()
        {
            string publicKey = "GXC69R784krfXRuFYMuNwhTTnMGPMuCSSng3WPssL6vrXRqTYCLT4";
            var accounts = await Client.GetAccountByPublicKey(publicKey);
            Console.WriteLine(JsonConvert.SerializeObject(accounts, Formatting.Indented));
        }
    }
}
