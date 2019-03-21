using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gxclient.Interfaces;
using gxclient.Models;
using Newtonsoft.Json.Linq;
using System.Linq;
using gxclient.Crypto;
using Newtonsoft.Json;

namespace gxclient
{
    public class Operation
    {
        public int OperationID { get; set; }
        public JObject Payload { get; set; }
        public JArray ToObject()
        {
            return JArray.FromObject(new object[] { OperationID, Payload });
        }
    }

    public class TransactionResult
    {
        [JsonProperty("block_num")]
        public long BlockNum { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("trx")]
        public Transaction Trx { get; set; }
    }

    public class TransactionBuilder
    {
        #region transaction properties
        public DateTime expiration;

        public UInt16 ref_block_num;

        public UInt32 ref_block_prefix;

        public List<Operation> operations;

        public object[] extensions = new object[] { };

        public string[] signatures = new string[] { };
        #endregion

        private readonly ISignatureProvider signatureProvider;

        private readonly GXRPC RPC;

        private readonly string ChainId;

        private const int DEFAULT_EXPIRE_SECONDS = 120;

        #region constructor
        public TransactionBuilder(GXRPC rpc, ISignatureProvider provider, string chainId)
        {
            this.RPC = rpc;
            this.signatureProvider = provider;
            this.operations = new List<Operation>();
            this.ChainId = chainId;
        }
        #endregion

        #region public methods

        /// <summary>
        /// Add the operation
        /// </summary>
        /// <param name="op">Operation</param>
        public void AddOperation(Operation op)
        {
            this.operations.Add(op);
        }

        /// <summary>
        /// Process the transaction.
        /// </summary>
        /// <returns>TransactionResult</returns>
        /// <param name="broadcast">If set to <c>true</c> boradcast the signed transaction to entrypoint.</param>
        public async Task<TransactionResult> ProcessTransaction(bool broadcast = false)
        {
            if (this.operations == null || this.operations.Count == 0)
            {
                throw new ArgumentException("Operations are empty, cannot invoke process transaction");
            }
            Task[] tasks = { this.UpdateHeadBlock(), this.SetRequiredFees() };
            Task.WaitAll(tasks);
            await Sign();
            if (broadcast)
            {
                return await RPC.Broadcast<TransactionResult>(SignedTransaction());
            }
            else
            {
                return new TransactionResult() { Trx = SignedTransaction() };
            }
        }

        public Transaction SignedTransaction()
        {
            return new Transaction()
            {
                Expiration = expiration,
                RefBlockNum = ref_block_num,
                RefBlockPrefix = ref_block_prefix,
                Operations = operations.Select(o => o.ToObject()).ToArray(),
                Signatures = signatures,
                Extensions = extensions
            };

            //return JObject.FromObject(new
            //{
            //    expiration,
            //    ref_block_num,
            //    ref_block_prefix,
            //    operations = operations.Select(o => o.ToObject()),
            //    signatures,
            //    extensions
            //});
        }

        public async Task<string> Serialize()
        {
            string tx_hex =  await RPC.Query<string>("get_transaction_hex", new object[] { SignedTransaction() });
            tx_hex = tx_hex.Substring(0, tx_hex.Length - 2); // remove empty signature part
            return tx_hex;
        }

        #endregion

        #region private methods
        private async Task<bool> UpdateHeadBlock()
        {
            DynamicGlobalProperties dgp = await RPC.Query<DynamicGlobalProperties>("get_dynamic_global_properties", null);
            this.expiration = dgp.Time.AddSeconds(DEFAULT_EXPIRE_SECONDS);
            this.ref_block_num = (UInt16)(dgp.LastIrreversibleBlockNum & 0xFFFF);
            Block block = await RPC.Query<Block>("get_block", new object[] { dgp.LastIrreversibleBlockNum });
            string ref_block_prefix_str = block.BlockId.Substring(14, 2) + block.BlockId.Substring(12, 2) + block.BlockId.Substring(10, 2) + block.BlockId.Substring(8, 2);
            this.ref_block_prefix = UInt32.Parse(ref_block_prefix_str, System.Globalization.NumberStyles.HexNumber);
            return await Task.FromResult<bool>(true);
        }

        private async Task<bool> SetRequiredFees()
        {
            string fee_asset_id = this.operations[0].Payload["fee"]["asset_id"].ToString();
            List<JObject> fees =  await RPC.Query<List<JObject>>("get_required_fees", new object[] { this.operations.Select(o=>o.ToObject()).ToArray(), fee_asset_id });
            int index = 0;
            for(int i = 0; i < this.operations.Count; ++i)
            {
                SetFee(fees[index++], this.operations[i].Payload);
                if (this.operations[i].Payload.ContainsKey("proposed_ops"))
                {
                    JArray proposed_ops = (JArray)this.operations[i].Payload["proposed_ops"];
                    for(int j = 0; i < proposed_ops.Count; ++j)
                    {
                        SetFee(fees[index++], (JObject)proposed_ops[j]);
                    }
                }
            }
            return true;
        }

        private void SetFee(JObject fee, JObject operation)
        {
            operation["fee"]["asset_id"] = fee["asset_id"].ToString();
            operation["fee"]["amount"] = fee["amount"].ToObject<long>();
        }

        private async Task<bool> Sign()
        {
            var tx_hex = await Serialize();
            signatures = (await signatureProvider.Sign(this.ChainId, Hex.HexToBytes(tx_hex))).ToArray();
            return true;
        }

        #endregion
    }
}
