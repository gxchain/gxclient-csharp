using System;
using System.Threading.Tasks;
using gxclient.Exceptions;
using gxclient.Implements;
using gxclient.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace gxclient
{
    public class RPCRequest
    {
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("jsonrpc")]
        public string JsonRPC { get; set; }
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("params")]
        public object Params { get; set; }
    }

    public class RPCResponse<TResponse>
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("jsonrpc")]
        public string JsonRPC { get; set; }
        [JsonProperty("result")]
        public TResponse Result { get; set; }
        [JsonProperty("error")]
        public APIErrorException Error { get; set; }
    }

    public class GXRPC
    {
        private static IHttpHandler Handler = new HttpHandler();
        private string EntryPoint { get; set; }
        private int ID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:gxclient.GXRPC"/> class.
        /// </summary>
        /// <param name="EntryPoint">Entry point.</param>
        public GXRPC(String EntryPoint)
        {
            this.EntryPoint = EntryPoint;
            this.ID = 0;
        }

        public async Task<TData> Query<TData>(String method, Object parameters)
        {
            var para = new RPCRequest {
                Method =  method ,
                JsonRPC = "2.0",
                Params = parameters,
                ID = this.ID++
            };
            RPCResponse<TData> result = await Handler.PostJsonAsync<RPCResponse<TData>>(this.EntryPoint, para);
#if DEBUG
            //Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
#endif
            return await Task.FromResult<TData>(result.Result);
        }

        public async Task<TData> Broadcast<TData>(object tx)
        {
            var para = new RPCRequest
            {
                Method = "call",
                JsonRPC = "2.0",
                Params = new object[] { 2, "broadcast_transaction_synchronous", new object[] { tx } },
                ID = this.ID++
            };
            RPCResponse<TData> result = await Handler.PostJsonAsync<RPCResponse<TData>>(this.EntryPoint, para);
#if DEBUG
            //Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
#endif
            if (result.Error != null)
            {
                throw result.Error;
            }
            return result.Result;
        }
    }
}