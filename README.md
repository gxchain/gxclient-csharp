# gxclient-csharp
A client to interact with gxchain implemented in C#
<p>
 <a href='javascript:;'>
   <img width="300px" src='https://raw.githubusercontent.com/gxchain/gxips/master/assets/images/task-gxclient.png'/>
 </a>
 <a href='javascript:;'>
   <img width="300px" src='https://raw.githubusercontent.com/gxchain/gxips/master/assets/images/task-gxclient-en.png'/>
 </a>
</p> 

## Dependencies
- .NETStandard 2.0
- Cryptography.ECDSA.Secp256K1 (>= 1.1.2)
- dotnetstandard-bip39 (>= 1.0.2)
- Newtonsoft.Json (>= 12.0.1)

# APIs
- [x] [Keypair API](#keypair-api)
- [x] [Chain API](#chain-api)
- [x] [Faucet API](#faucet-api)
- [x] [Account API](#account-api)
- [x] [Asset API](#asset-api)
- [ ] [Contract API](#contract-api)

## Constructors
```c#
public GXClient(ISignatureProvider signatureProvider, IMemoProvider memoProvider, String AccountName, String EntryPoint = "https://node1.gxb.io")
```

## Keypair API
```c#
/// <summary>
/// Generates the key pair.
/// </summary>
/// <returns>The key pair.</returns>
/// <param name="brainKey">Brain key.</param>
public KeyPair GenerateKeyPair(string brainKey = null)

/// <summary>
/// Export public key from private key
/// </summary>
/// <returns>Public key string.</returns>
/// <param name="privateKey">Private key string.</param>
public String PrivateToPublic(string privateKey)

/// <summary>
/// Check if <paramref name="privateKey"/> is valid or not
/// </summary>
/// <returns><c>true</c>, if valid private was assigned, <c>false</c> otherwise.</returns>
/// <param name="privateKey">Private key string.</param>
public bool IsValidPrivate(string privateKey)

/// <summary>
/// Check if <paramref name="publicKey"/> is valid or not
/// </summary>
/// <returns><c>true</c>, if valid public was ised, <c>false</c> otherwise.</returns>
/// <param name="publicKey">Public key.</param>
public bool IsValidPublic(string publicKey)
```

## Chain API
```c#
/// <summary>
/// Query the specified method with parameter.
/// </summary>
/// <returns>Dictionary value</returns>
/// <param name="method">Method.</param>
/// <param name="parameter">Parameter.</param>
/// <typeparam name="TData">The 1st type parameter.</typeparam>
public async Task<TData> Query<TData>(string method, object parameter)

/// <summary>
/// Broadcast the specified transaction.
/// </summary>
/// <returns>Transaction</returns>
/// <param name="transaction">Transaction.</param>
/// <typeparam name="TData">Transaction</typeparam>
public async Task<TData> Broadcast<TData>(object transaction)

/// <summary>
/// GET ChainId of entry point
/// </summary>
/// <returns>ChainId.</returns>
public async Task<string> GetChainId()

/// <summary>
/// Gets dynamic global propertis of current blockchain
/// </summary>
/// <returns>The dynamic global propertis.</returns>
public async Task<DynamicGlobalProperties> GetDynamicGlobalPropertis()

/// <summary>
/// Get block by block <paramref name="height"/>
/// </summary>
/// <returns>The block.</returns>
/// <param name="height">Height.</param>
public async Task<Block> GetBlock(ulong height)

/// <summary>
/// Gets Object by <paramref name="object_ids"/>
/// </summary>
/// <returns>Objects.</returns>
/// <param name="object_ids">Object identifiers.</param>
public async Task<JArray> GetObjects(string[] object_ids)

/// <summary>
/// Get single object by <paramref name="object_id"/>
/// </summary>
/// <returns>The object.</returns>
/// <param name="object_id">Object identifier.</param>
public async Task<JObject> GetObject(string object_id)

/// <summary>
/// Gets the vote identifiers by accounts.
/// </summary>
/// <returns>The vote identifiers by accounts.</returns>
/// <param name="accountNames">Account names.</param>
public async Task<IEnumerable<string>> GetVoteIdsByAccounts(string[] accountNames)

/// <summary>
/// Vote for specified accounts, setup proxyAccount
/// </summary>
/// <returns>Transaction result</returns>
/// <param name="accounts">Accounts.</param>
/// <param name="proxyAccount">Proxy account.</param>
/// <param name="feeAsset">Fee asset.</param>
/// <param name="broadcast">If set to <c>true</c> broadcast.</param>
public async Task<TransactionResult> Vote(string[] accounts, string proxyAccount, string feeAsset = "GXC", bool broadcast = false)

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
```

## Faucet API
```c#
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
```
## Account API
```c#
/// <summary>
/// Get account by account name
/// </summary>
/// <returns>Account</returns>
/// <param name="accountName">AccountName</param>
public async Task<Account> GetAccount(string accountName)

/// <summary>
/// Get a list of accounts by <paramref name="accountNames"/>
/// </summary>
/// <returns>accounts.</returns>
/// <param name="accountNames">Account names.</param>
public async Task<IEnumerable<Account>> GetAccounts(string[] accountNames)


/// <summary>
/// Get account balances by <paramref name="accountName"/>.
/// </summary>
/// <returns>The account balances.</returns>
/// <param name="accountName">Account name.</param>
public async Task<IEnumerable<AssetAmount>> GetAccountBalances(string accountName)

/// <summary>
/// Get account by public key.
/// </summary>
/// <returns>A list of account id</returns>
/// <param name="publicKey">Publickey.</param>
public async Task<IEnumerable<string>> GetAccountByPublicKey(string publicKey)
```

## Asset API
```c#
/// <summary>
/// Get a list of asset by asset symbols
/// </summary>
/// <returns>List of asset.</returns>
/// <param name="assets">Asset symbols.</param>
public async Task<IEnumerable<Asset>> GetAssets(string[] assets)

/// <summary>
/// Get asset by symbol
/// </summary>
/// <returns>The asset.</returns>
/// <param name="asset">Asset symbol.</param>
public async Task<Asset> GetAsset(string asset)
```

## Contract API
Comming soon
