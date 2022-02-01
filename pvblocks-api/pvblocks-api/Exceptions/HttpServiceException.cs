using System;

namespace pvblocks_api.Exceptions
{
    public class HttpServiceException : Exception
    {
        public HttpServiceException()
            : base("Something went wrong in the HttpService")
        {
        }
    }
}