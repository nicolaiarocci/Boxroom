using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DataStorage.Rest
{
    public abstract class BaseRestRepository : IRestRepository
    {
        public Uri BaseAddress { get; set; }
        public HttpClient HttpClient { get; set; }
        public HttpResponseMessage Response { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<Type, string> DataSources { get; set; }

        public virtual Task Delete<T>(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task Delete<T>(string itemId)
        {
            throw new NotImplementedException();
        }

        public virtual Task Delete<T>()
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Get<T>(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Get<T>(string itemId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<T>> Get<T>()
        {
            throw new NotImplementedException();
        }

        public virtual Task Insert<T>(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task Insert<T>(List<T> items)
        {
            throw new NotImplementedException();
        }
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

        public virtual void ValidateProperties()
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