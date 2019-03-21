using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace gxclient.Interfaces
{
    /// <summary>
    /// Http handler interface for cross platform specific implementations
    /// </summary>
    public interface IHttpHandler
    {
        /// <summary>
        /// Make post request with data converted to json asynchronously
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="data">data sent in the body</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data);

        /// <summary>
        /// Make post request with data converted to json asynchronously
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="data">data sent in the body</param>
        /// <param name="cancellationToken">Notification that operation should be canceled</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, CancellationToken cancellationToken);

        /// <summary>
        /// Make get request asynchronously.
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        Task<TResponseData> GetJsonAsync<TResponseData>(string url);

        /// <summary>
        /// Make get request asynchronously.
        /// </summary>
        /// <typeparam name="TResponseData">Response type</typeparam>
        /// <param name="url">Url to send the request</param>
        /// <param name="cancellationToken">Notification that operation should be canceled</param>
        /// <returns>Response data deserialized to type TResponseData</returns>
        Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken);

        /// <summary>
        /// Generic http request sent asynchronously
        /// </summary>
        /// <param name="request">request body</param>
        /// <returns>Stream with response</returns>
        Task<Stream> SendAsync(HttpRequestMessage request);

        /// <summary>
        /// Generic http request sent asynchronously
        /// </summary>
        /// <param name="request">request body</param>
        /// /// <param name="cancellationToken">Notification that operation should be canceled</param>
        /// <returns>Stream with response</returns>
        Task<Stream> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

        /// <summary>
        /// Convert response to stream
        /// </summary>
        /// <param name="response">response object</param>
        /// <returns>Stream with response</returns>
        Task<Stream> BuildSendResponse(HttpResponseMessage response);

        /// <summary>
        /// Convert stream to a string
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<string> StreamToStringAsync(Stream stream);
    }
}
