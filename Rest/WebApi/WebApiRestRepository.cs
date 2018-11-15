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
    public class WebApiRestRepository : RestRepositoryBase

    {
        public override async Task<List<T>> Find<T>(Expression<Func<T, bool>> filter, IFindOptions<T> options = null)
        {
            // TODO actually do take filter argument into consideration! 
            // or raise if we only support a "findAll" filter type (hopefully not)

            ValidateProperties();

            EnsureIfModifiedSinceHeader((FindOptions<T>) options);

            var client = PreparedClient();

            // TODO: more robust url building
            Response = await client.GetAsync($"{BaseAddress.ToString()}{DataSources[typeof(T)]}");
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
            ValidateProperties();

            // var (etagMemberName, etagMemberValue) = GetMemberNameAndValue<T>(item, RestMetaFields.ETag);
            // if (etagMemberName != null)
            // {
            //     Headers["If-Match"] = etagMemberValue.ToString();
            // }
            // var (lastUpdatedMemberName, lastUpdatedMemberValue) = GetMemberNameAndValue<T>(item, RestMetaFields.LastUpdated);
            // if (lastUpdatedMemberName != null)
            // {
            //     Headers["If-Modified-Since"] = lastUpdatedMemberValue.ToString();
            // }

            var (idMemberName, idMemberValue) = GetIdMemberNameAndValue<T>(item);
            if (idMemberName == null)
            {
                // TODO throw
            }

            var client = PreparedClient();

            var content = new StringContent(JsonConvert.SerializeObject(item));

            Response = await client.PutAsync($"{BaseAddress.ToString()}{DataSources[typeof(T)]}/{idMemberValue.ToString()}", content);
            if (Response.StatusCode != HttpStatusCode.OK) return default(T);

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);

        }
        private void EnsureIfModifiedSinceHeader<T>(FindOptions<T> options)
        {

            if (options == null || !options.IfModifiedSince.HasValue) return;
            Headers.Add("If-Modified-Since", options.IfModifiedSince.Value.ToString("r"));
        }
        private void EnsureIfNoneMatchHeader<T>(FindOptions<T> options)
        {

            if (options == null || options.ETag == null) return;
            Headers.Add("If-None-Match", options.ETag);
        }

    }

}