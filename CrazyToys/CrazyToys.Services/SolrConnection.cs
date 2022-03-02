using SolrNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    internal class SolrConnection : ISolrConnection

    {
        public string Get(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAsync(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public string Post(string relativeUrl, string s)
        {
            throw new NotImplementedException();
        }

        public Task<string> PostAsync(string relativeUrl, string s)
        {
            throw new NotImplementedException();
        }

        public string PostStream(string relativeUrl, string contentType, Stream content, IEnumerable<KeyValuePair<string, string>> getParameters)
        {
            throw new NotImplementedException();
        }

        public Task<string> PostStreamAsync(string relativeUrl, string contentType, Stream content, IEnumerable<KeyValuePair<string, string>> getParameters)
        {
            throw new NotImplementedException();
        }
    }
}
