using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gxclient.Interfaces
{
    public interface ISignatureProvider
    {
        /// <summary>
        /// Sign the specified chainId and transaction.
        /// </summary>
        /// <returns>List of signatures</returns>
        /// <param name="chainId">Chain id.</param>
        /// <param name="transaction">Serialized Transaction.</param>
        Task<IEnumerable<string>> Sign(string chainId, byte[] transaction);
    }
}
