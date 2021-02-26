using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.Elasticsearch.Application
{
    /// <summary>
    /// Elasticsearch异常
    /// </summary>
    public class ElasticsearchException : ApplicationException
    {
        public ElasticsearchException(IResponse response)
            : base($"{response.ServerError?.Error?.Reason} : {response.DebugInformation}")
        {

        }

        public ElasticsearchException(string title, string message = null)
            : base($"{title} : {message}")
        {

        }
    }
}
