using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using dotnetstandard_bip39;
using gxclient.Crypto;
using gxclient.Implements;
using gxclient.Interfaces;
using gxclient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace gxclient
{
    public class GXClient
    {
        private ISignatureProvider SignatureProvider { get; set; }
        private IMemoProvider MemoProvider { get; set; }
        private string AccountName { get; set; }
        private string EntryPoint { get; set; }
        private GXRPC RPC { get; set; }

        private static BIP39 bip39 = new BIP39();

        private static readonly IHttpHandler httpHandler = new HttpHandler();

        private string ChainId { get; set; }

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:gxclient.GXClient"/> class.
        /// </summary>
        /// <param name="signatureProvider">Signature provider.</param>
        /// /// <param name="memoProvider">Memo generator provider.</param>
        /// <param name="AccountName">Account name.</param>
        /// <param name="EntryPoint">Entry point.</param>
        public GXClient(ISignatureProvider signatureProvider, IMemoProvider memoProvider, String AccountName, String EntryPoint = "https://node1.gxb.io")
        {
            this.SignatureProvider = signatureProvider;
            this.MemoProvider = memoProvider;
            this.AccountName = AccountName;
            this.EntryPoint = EntryPoint;
            this.RPC = new GXRPC(this.EntryPoint);
        }
        #endregion

        #region keypair api
        /// <summary>
        /// Generates the key pair.
        /// </summary>
        /// <returns>The key pair.</returns>
        /// <param name="brainKey">Brain key.</param>
        public KeyPair GenerateKeyPair(string brainKey = null)
        {
            string brain_key = brainKey;
            if (brain_key == null)
            {
                brain_key = String.Join(" ", bip39.GenerateMnemonic(160, BIP39Wordlist.English).Split('\r').AsEnumerable<string>().Select(a =>
                {
                    return a.Trim();
                }).ToArray()).Trim();
            }
            byte[] private_key_bytes = Hash.SHA256(brain_key + " " + 0);
            Crypto.PrivateKey privateKey = Crypto.PrivateKey.FromBytes(private_key_bytes);

            return new KeyPair()
            {
                BrainKey = brain_key,
                PrivateKey = privateKey.ToWif(),
                PublicKey = privateKey.GetPublicKey().ToString()
            };
        }

        /// <summary>
        /// Export public key from private key
        /// </summary>
        /// <returns>Public key string.</returns>
        /// <param name="privateKey">Private key string.</param>
        public String PrivateToPublic(string privateKey)
        {
            return Crypto.PrivateKey.FromWif(privateKey).GetPublicKey().ToString();
        }

        /// <summary>
        /// Check if <paramref name="privateKey"/> is valid or not
        /// </summary>
        /// <returns><c>true</c>, if valid private was assigned, <c>false</c> otherwise.</returns>
        /// <param name="privateKey">Private key string.</param>
        public bool IsValidPrivate(string privateKey)
        {
            try
            {
                return Crypto.PrivateKey.FromWif(privateKey).ToWif() == privateKey;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if <paramref name="publicKey"/> is valid or not
        /// </summary>
        /// <returns><c>true</c>, if valid public was ised, <c>false</c> otherwise.</returns>
        /// <param name="publicKey">Public key.</param>
        public bool IsValidPublic(string publicKey)
        {
            try
            {
                return PublicKey.FromString(publicKey).ToString() == publicKey;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region faucet api
        /// <summary>
        /// Register account by <paramref name="accountName"/> and public keys
        /// </summary>
        /// <returns>A broadcated transaction</returns>
        /// <param name="accountName">Account name.</param>
        /// <param name="activeKey">Active key.</param>
        /// <param name="ownerKey">Owner key.</param>
        /// <param name="memoKey">Memo key.</param>
        /// <param name="faucetUrl">Faucet URL.</param>
        public async Task<Transaction> RegisterAccount(string accountName, string activeKey, string ownerKey, string memoKey, string faucetUrl = "https://opengateway.gxb.io")
        {
            var payload = new { account = new { name = accountName, active_key = activeKey, owner_key = ownerKey, memo_key = memoKey } };
            var response = await httpHandler.PostJsonAsync<JObject>($"{faucetUrl}/account/register", payload);
            return response.ToObject<Transaction>();
        }
        #endregion

        #region chain api

        /// <summary>
        /// Query the specified method with parameter.
        /// </summary>
        /// <returns>Dictionary value</returns>
        /// <param name="method">Method.</param>
        /// <param name="parameter">Parameter.</param>
        /// <typeparam name="TData">The 1st type parameter.</typeparam>
        public async Task<TData> Query<TData>(string method, object parameter)
        {
            return await RPC.Query<TData>(method, parameter);
        }

        /// <summary>
        /// Broadcast the specified transaction.
        /// </summary>
        /// <returns>Transaction</returns>
        /// <param name="transaction">Transaction.</param>
        /// <typeparam name="TData">Transaction</typeparam>
        public async Task<TData> Broadcast<TData>(object transaction)
        {
            return await RPC.Broadcast<TData>(transaction);
        }

        /// <summary>
        /// GET ChainId of entry point
        /// </summary>
        /// <returns>ChainId.</returns>
        public async Task<string> GetChainId()
        {
            return await this.Query<string>("get_chain_id", null);
        }

        /// <summary>
        /// Gets dynamic global propertis of current blockchain
        /// </summary>
        /// <returns>The dynamic global propertis.</returns>
        public async Task<DynamicGlobalProperties> GetDynamicGlobalPropertis()
        {
            return await this.Query<DynamicGlobalProperties>("get_dynamic_global_properties", null);
        }

        /// <summary>
        /// Get block by block <paramref name="height"/>
        /// </summary>
        /// <returns>The block.</returns>
        /// <param name="height">Height.</param>
        public async Task<Block> GetBlock(ulong height)
        {
            return await this.Query<Block>("get_block", new object[] { height });
        }

        /// <summary>
        /// Gets Object by <paramref name="object_ids"/>
        /// </summary>
        /// <returns>Objects.</returns>
        /// <param name="object_ids">Object identifiers.</param>
        public async Task<JArray> GetObjects(string[] object_ids)
        {
            return await this.Query<JArray>("get_objects", new object[] { object_ids });
        }

        /// <summary>
        /// Get single object by <paramref name="object_id"/>
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="object_id">Object identifier.</param>
        public async Task<JObject> GetObject(string object_id)
        {
            var objects = await GetObjects(new string[] { object_id });
            return objects[0].ToObject<JObject>();
        }

        /// <summary>
        /// Gets the vote identifiers by accounts.
        /// </summary>
        /// <returns>The vote identifiers by accounts.</returns>
        /// <param name="accountNames">Account names.</param>
        public async Task<IEnumerable<string>> GetVoteIdsByAccounts(string[] accountNames)
        {
            var accounts = await GetAccounts(accountNames);
            var getWitnessVoteIdTasks = accounts.Select(a => Query<JObject>("get_witness_by_account", new string[] { a.Id }));
            var getCommitteeVoteIdTasks = accounts.Select(a => Query<JObject>("get_committee_member_by_account", new string[] { a.Id }));
            var witnessVoteIds = await Task.WhenAll(getWitnessVoteIdTasks);
            var committeeVoteIds = await Task.WhenAll(getCommitteeVoteIdTasks);
            IList<string> voteIDs = new List<string>();
            foreach (var item in witnessVoteIds)
            {
                if (item != null)
                {
                    voteIDs.Add(item["vote_id"].ToObject<string>());
                }
            }
            foreach (var item in committeeVoteIds)
            {
                if (item != null)
                {
                    voteIDs.Add(item["vote_id"].ToObject<string>());
                }
            }
            return voteIDs;
        }

        /// <summary>
        /// Vote for specified accounts, setup proxyAccount
        /// </summary>
        /// <returns>Transaction result</returns>
        /// <param name="accounts">Accounts.</param>
        /// <param name="proxyAccount">Proxy account.</param>
        /// <param name="feeAsset">Fee asset.</param>
        /// <param name="broadcast">If set to <c>true</c> broadcast.</param>
        public async Task<TransactionResult> Vote(string[] accounts, string proxyAccount, string feeAsset = "GXC", bool broadcast = false)
        {
            Account myAccount = await GetAccount(this.AccountName);
            string[] voteIds = (await GetVoteIdsByAccounts(accounts)).ToArray();
            var transactionBuilder = await CreateTransactionBuilder();
            var feeAssetId = (await GetAsset(feeAsset)).Id;
            var globalParams = await GetObject("2.0.0");

            var votingAccount = string.IsNullOrEmpty(proxyAccount) ? "1.2.5" : (await GetAccount(proxyAccount)).Id;
            var newOptions = myAccount.Options;
            int maximumCommitteeCount = globalParams["parameters"]["maximum_committee_count"].ToObject<int>();
            int maximumWitnessCount = globalParams["parameters"]["maximum_witness_count"].ToObject<int>();

            newOptions.Votes = newOptions.Votes.Concat(voteIds).Distinct().ToArray();
            int numCommitee = 0;
            int numWitness = 0;
            foreach(string voteId in newOptions.Votes)
            {
                var splits = voteId.Split(':');
                if(splits[0] == "0")
                {
                    numCommitee += 1;
                }
                if(splits[0] == "1")
                {
                    numWitness += 1;
                }
            }
            newOptions.NumWitness = Math.Min(numWitness, maximumWitnessCount);
            newOptions.NumCommittee = Math.Min(numCommitee, maximumCommitteeCount);
            newOptions.VotingAccount = votingAccount;

            transactionBuilder.AddOperation(new Operation()
            {
                OperationID = 6,
                Payload = JObject.FromObject(new
                {
                    fee =new { asset_id = feeAssetId, amount = 0 },
                    account = myAccount.Id,
                    new_options = newOptions
                })
            });

            return await transactionBuilder.ProcessTransaction(broadcast);
        }

        /// <summary>
        /// Transfer a amount of assets to specified account with specified memo
        /// </summary>
        /// <returns>The transfer.</returns>
        /// <param name="to">To.</param>
        /// <param name="memo">Memo.</param>
        /// <param name="amountAsset">Amount asset.</param>
        /// <param name="feeAsset">Fee asset.</param>
        /// <param name="broadcast">If set to <c>true</c> broadcast.</param>
        public async Task<TransactionResult> Transfer(string to, string memo, string amountAsset, string feeAsset="GXC",bool broadcast = false)
        {
            var toAccount = await GetAccount(to);
            var fromAccount = await GetAccount(this.AccountName);
            string[] amountAssetArr = amountAsset.Split(' ');
            if (amountAssetArr.Length != 2)
            {
                throw new Exception("invalid amount asset, a valid example is \"100 GXC\"");
            }
            UInt64 amount = UInt64.Parse(amountAssetArr[0]);
            var asset = (await GetAsset(amountAssetArr[1]));
            var feeAssetId = string.IsNullOrEmpty(feeAsset) ? asset.Id : (await GetAsset(feeAsset)).Id;

            Operation op = null;
            if (!String.IsNullOrEmpty(memo))
            {
                using(RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    byte[] randomBytes = new byte[sizeof(UInt64)];
                    rng.GetBytes(randomBytes);
                    UInt64 nonce = UInt64.Parse(Hex.BytesToHex(randomBytes), System.Globalization.NumberStyles.HexNumber);
                    Memo m = MemoProvider.GenerateMemo(toAccount.Options.MemoKey, nonce, memo);
                    op = new Operation()
                    {
                        OperationID = 0,
                        Payload = JObject.FromObject(new
                        {
                            fee = new { asset_id = feeAssetId, amount = 0 },
                            from = fromAccount.Id,
                            to = toAccount.Id,
                            amount = new { asset_id = asset.Id, amount = amount * Math.Pow(10,asset.Precision) },
                            memo = m,
                            extensions = new object[] { }
                        })
                    };
                }
            }
            else
            {
                op = new Operation()
                {
                    OperationID = 0,
                    Payload = JObject.FromObject(new
                    {
                        fee = new { asset_id = feeAssetId, amount = 0 },
                        from = fromAccount.Id,
                        to = toAccount.Id,
                        amount = new { asset_id = asset.Id, amount = amount * Math.Pow(10, asset.Precision) },
                        extensions = new object[] { }
                    })
                };
            }
            TransactionBuilder transactionBuilder = await CreateTransactionBuilder();
            transactionBuilder.AddOperation(op);
            return await transactionBuilder.ProcessTransaction(broadcast);
        }

        #endregion

        #region account api
        /// <summary>
        /// Get account by account name
        /// </summary>
        /// <returns>Account</returns>
        /// <param name="accountName">AccountName</param>
        public async Task<Account> GetAccount(string accountName)
        {
            return await this.Query<Account>("get_account_by_name", new object[] { accountName });
        }

        /// <summary>
        /// Get a list of accounts by <paramref name="accountNames"/>
        /// </summary>
        /// <returns>accounts.</returns>
        /// <param name="accountNames">Account names.</param>
        public async Task<IEnumerable<Account>> GetAccounts(string[] accountNames)
        {
            var tasks = accountNames.Select(async accountName => await GetAccount(accountName));
            var result = await Task.WhenAll<Account>(tasks);
            return result;
        }

        /// <summary>
        /// Get account balances by <paramref name="accountName"/>.
        /// </summary>
        /// <returns>The account balances.</returns>
        /// <param name="accountName">Account name.</param>
        public async Task<IEnumerable<AssetAmount>> GetAccountBalances(string accountName)
        {
            var account = await this.GetAccount(accountName);
            return await this.Query<List<AssetAmount>>("get_account_balances", new object[] { account.Id, new object[] { } });
        }

        /// <summary>
        /// Get account by public key.
        /// </summary>
        /// <returns>A list of account id</returns>
        /// <param name="publicKey">Publickey.</param>
        public async Task<IEnumerable<string>> GetAccountByPublicKey(string publicKey)
        {
            JArray result = await this.Query<JArray>("get_key_references", new string[][] { new string[] { publicKey } });
            if (result == null || result.Count == 0)
            {
                return new string[] { };
            }
            var accounts = result[0].Select(o => o.ToObject<string>()).Distinct();
            return accounts;
        }
        #endregion

        #region asset api

        /// <summary>
        /// Get a list of asset by asset symbols
        /// </summary>
        /// <returns>List of asset.</returns>
        /// <param name="assets">Asset symbols.</param>
        public async Task<IEnumerable<Asset>> GetAssets(string[] assets)
        {
            var result = await this.Query<IEnumerable<Asset>>("lookup_asset_symbols", new string[][] { assets });
            var detailIds = result.Select(asset => asset.DynamicAssetDataId).ToArray();
            var details = await GetObjects(detailIds);
            for (int i = 0; i < result.Count(); ++i)
            {
                result.ElementAt(i).Detail = details[i].ToObject<AssetDetail>();
            }
            return result;
        }

        /// <summary>
        /// Get asset by symbol
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="asset">Asset symbol.</param>
        public async Task<Asset> GetAsset(string asset)
        {
            var assets = await GetAssets(new string[] { asset });
            return assets.First();
        }

        #endregion

        #region contract apis

        /// <summary>
        /// Gets contract abi by <paramref name="contractName"/>
        /// </summary>
        /// <returns>The contract abi.</returns>
        /// <param name="contractName">Contract name.</param>
        public async Task<Abi> GetContractABI(string contractName)
        {
            Account account = await this.GetAccount(contractName);
            if (account != null)
            {
                return account.Abi;
            }
            return null;
        }

        /// <summary>
        /// Gets contrct tables by <paramref name="contractName"/>.
        /// </summary>
        /// <returns>The contrct tables.</returns>
        /// <param name="contractName">Contract name.</param>
        public async Task<IEnumerable<Table>> GetContrctTables(string contractName)
        {
            Account account = await this.GetAccount(contractName);
            if (account != null)
            {
                return account.Abi.Tables;
            }
            return null;
        }

        /// <summary>
        /// Gets contract table objects
        /// </summary>
        /// <returns>The table objects.</returns>
        /// <param name="contractName">Contract name.</param>
        /// <param name="tableName">Table name.</param>
        /// <param name="start">Start.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="reverse">If set to <c>true</c> reverse.</param>
        public async Task<IEnumerable<JObject>> GetTableObjects(string contractName,string tableName,UInt64 start, UInt64 limit, bool reverse)
        {
            return await this.Query<IJEnumerable<JObject>>("get_table_rows_ex", new object[] { contractName, tableName,  new {
                lower_bound = start,
                upper_bound = -1,
                limit,
                reverse
            } });
        }

        /// <summary>
        /// Serializes the contract parameters.
        /// </summary>
        /// <returns>Serialized result.</returns>
        /// <param name="contractName">Contract name.</param>
        /// <param name="method">Method name.</param>
        /// <param name="parameters">Parameters.</param>
        public async Task<string> SerializeContractParams(string contractName, string method, object parameters)
        {
            return await this.Query<string>("serialize_contract_call_args", new object[] { contractName, method, JsonConvert.SerializeObject(parameters) });
        }

        /// <summary>
        /// Call contract with indicated <paramref name="method"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <returns>The contract.</returns>
        /// <param name="contractName">Contract name.</param>
        /// <param name="method">Method name.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="amountAsset">Amount asset, eg. "100 GXC".</param>
        /// <param name="feeAsset">Fee asset symbol, eg. "GXC".</param>
        /// <param name="broadcast">If set to <c>true</c> broadcast.</param>
        public async Task<TransactionResult> CallContract(string contractName, string method, object parameters, string amountAsset, string feeAsset="GXC", bool broadcast= false)
        {
            Account contract = await GetAccount(contractName);
            Account fromAccount = await GetAccount(this.AccountName);
            string[] amountAssetArr = null;
            UInt64 amount = 0L;
            Asset asset = null;
            string feeAssetSymbol = feeAsset;
            if (string.IsNullOrEmpty(amountAsset))
            {
                amountAssetArr = amountAsset.Split(' ');
                if (amountAssetArr.Length != 2)
                {
                    throw new Exception("invalid amount asset, a valid example is \"100 GXC\"");
                }
                amount = UInt64.Parse(amountAssetArr[0]);
                asset = (await GetAsset(amountAssetArr[1]));
            }
            var feeAssetId = string.IsNullOrEmpty(feeAsset) ? (asset==null?"1.3.1":asset.Id) : (await GetAsset(feeAsset)).Id;
            TransactionBuilder transactionBuilder = await CreateTransactionBuilder();
            Operation op = null;
            if(asset == null)
            {
                op = new Operation()
                {
                    OperationID = 75,
                    Payload = JObject.FromObject(new
                    {
                        fee = new { asset_id = feeAssetId, amount = 0 },
                        account = fromAccount.Id,
                        contract_id = contract.Id,
                        method_name = method,
                        data = await SerializeContractParams(contractName, method, parameters)
                    })
                };
            }
            else
            {
                op = new Operation()
                {
                    OperationID = 75,
                    Payload = JObject.FromObject(new
                    {
                        fee = new { asset_id = feeAssetId, amount = 0 },
                        amount = new {asset_id = asset.Id, amount},
                        account = fromAccount.Id,
                        contract_id = contract.Id,
                        method_name = method,
                        data = await SerializeContractParams(contractName, method, parameters)
                    })
                };
            }
            transactionBuilder.AddOperation(op);
            return await transactionBuilder.ProcessTransaction(broadcast);
        }

        #endregion

        #region private methods
        private async Task<TransactionBuilder> CreateTransactionBuilder()
        {
            if (string.IsNullOrEmpty(this.ChainId))
            {
                this.ChainId = await GetChainId();
            }
            return new TransactionBuilder(this.RPC, this.SignatureProvider, this.ChainId);
        }
        #endregion
    }
}
