using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using DataStorage.Core;

namespace DataStorage.Rest
{
    public abstract class BaseRestRepository : BaseRepository, IRestRepository
    {
        public Uri BaseAddress { get; set; }
        public HttpClient HttpClient { get; set; }
        public HttpResponseMessage Response { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public virtual HttpClient ClientWithHeaders()
        {

            var client = (HttpClient != null) ? HttpClient : new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var header in Headers)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }

            return client;
        }

        public override void ValidateProperties()
        {
            if (BaseAddress == null)
            {
                throw new ArgumentNullException(nameof(BaseAddress));
            }
            if (DataSources == null)
            {
                throw new ArgumentNullException(nameof(DataSources));
            }
            if (DataSources.Count == 0)
            {
                throw new ArgumentException($"{nameof(DataSources)} cannot be empty", nameof(DataSources));
            }
        }
    }
}