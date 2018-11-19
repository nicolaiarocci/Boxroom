using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using DataStorage.Core;
using Newtonsoft.Json;

namespace DataStorage.Rest
{
    public abstract class RestRepositoryBase : RepositoryBase, IRestRepository
    {
        public override Core.MetaFields MetaFields { get; } = new MetaFields();
        public Uri BaseAddress { get; set; }
        public HttpClient HttpClient { get; set; }
        public HttpResponseMessage Response { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        protected abstract string Render<T>(Expression<Func<T, bool>> filter);
        public override async Task<List<T>> Find<T>(Expression<Func<T, bool>> filter, IFindOptions<T> options = null)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            ValidateProperties();

            // TODO: why do I have to explictly pass 'this', otherwise I get an error? Shoud be an extension method.
            EnsureHeader.IfModifiedSince(this, options);

            var query = Render(filter);

            var client = PreparedClient();

            // TODO: more robust url building
            Response = await client.GetAsync($"{BaseAddress.ToString()}{DataSources[typeof(T)]}{(query == null ? "" : query)}");
            if (Response.StatusCode != HttpStatusCode.OK) return null;

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        public override async Task Delete<T>(string itemId)
        {
            if (itemId == null)
            {
                throw new ArgumentNullException(nameof(itemId));
            }
            if (itemId == string.Empty)
            {
                throw new ArgumentException($"{nameof(itemId)} cannot be empty", nameof(itemId));
            }

            ValidateProperties();

            var client = PreparedClient();

            // TODO: more robust url building
            Response = await client.DeleteAsync($"{BaseAddress.ToString()}{DataSources[typeof(T)]}/{itemId}");
        }
        public override async Task<List<T>> Insert<T>(List<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Count == 0)
            {
                throw new ArgumentException($"{nameof(items)} cannot be empty", nameof(items));
            }

            ValidateProperties();

            var client = PreparedClient();

            var content = new StringContent(JsonConvert.SerializeObject(items));
            Response = await client.PostAsync($"{BaseAddress.ToString()}{DataSources[typeof(T)]}", content);
            if (Response.StatusCode != HttpStatusCode.Created) return null;

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        public override async Task<T> Replace<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            ValidateProperties();

            var (idMemberName, idMemberValue) = GetIdMemberNameAndValue<T>(item);
            if (idMemberName == null)
            {
                // TODO throw?
            }

            var client = PreparedClient();

            var content = new StringContent(JsonConvert.SerializeObject(item));

            Response = await client.PutAsync($"{BaseAddress.ToString()}{DataSources[typeof(T)]}/{idMemberValue.ToString()}", content);
            if (Response.StatusCode != HttpStatusCode.OK) return default(T);

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);

        }
        public virtual HttpClient PreparedClient()
        {

            // TODO does this re/initializes a client every single time a call is made to this method?
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