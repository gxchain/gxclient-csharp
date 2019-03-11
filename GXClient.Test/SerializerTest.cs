using Microsoft.VisualStudio.TestTools.UnitTesting;
using gxclient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gxclient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GXClient.Test
{
    //[TestClass]
    //public class SerializerTest
    //{
    //    [TestMethod]
    //    public void SerializeActionData()
    //    {
    //        var parameters = new { to_account = "lzydophin94", amount = new { asset_id = 1, amount = 1000000 } };
    //        string abiStr = "{\"version\":\"gxc::abi/1.0\",\"types\":[],\"structs\":[{\"name\":\"account\",\"base\":\"\",\"fields\":[{\"name\":\"owner\",\"type\":\"uint64\"},{\"name\":\"balances\",\"type\":\"contract_asset[]\"}]},{\"name\":\"deposit\",\"base\":\"\",\"fields\":[]},{\"name\":\"withdraw\",\"base\":\"\",\"fields\":[{\"name\":\"to_account\",\"type\":\"string\"},{\"name\":\"amount\",\"type\":\"contract_asset\"}]}],\"actions\":[{\"name\":\"deposit\",\"type\":\"deposit\",\"payable\":true},{\"name\":\"withdraw\",\"type\":\"withdraw\",\"payable\":false}],\"tables\":[{\"name\":\"account\",\"index_type\":\"i64\",\"key_names\":[\"owner\"],\"key_types\":[\"uint64\"],\"type\":\"account\"}],\"error_messages\":[],\"abi_extensions\":[]}";
    //        var abi = JObject.Parse(abiStr);
    //        string result = Serializer2.SerializeActionData("withdraw", parameters,abi);
    //        Console.WriteLine(result);
    //    }
    //}
}
