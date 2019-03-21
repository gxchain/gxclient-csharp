using System;
using System.Collections.Generic;

namespace gxclient.Exceptions
{
    /// <summary>
    /// Wrapper exception for GXChain API error
    /// </summary>
    public class APIErrorException : Exception
    {
        public int Code { get; set; }
        public new string Message { get; set; }
        public new APIError Data { get; set; }
    }

    /// <summary>
    /// GXChain API Error
    /// </summary>
    public class APIError
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public List<APIErrorDetail> Stack { get; set; }
    }

    /// <summary>
    /// GXChain API Error detail
    /// </summary>
    public class APIErrorDetail
    {
        public APIErrorDetailContext Context { get; set; }
        public string Format { get; set; }
        public Dictionary<string,Object> Data { get; set; }
    }

    public class APIErrorDetailContext
    {
        public string Level { get; set; }
        public string File { get; set; }
        public string Line { get; set; }
        public string Method { get; set; }
        public string Hostname { get; set; }
        public string ThreadName { get; set; }
        public string Timestamp { get; set; }
    }
}
