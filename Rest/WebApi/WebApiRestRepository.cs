using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataStorage.Rest
{
    public class WebApiRestRepository : BaseRestRepository

    {
        public override async Task<List<T>> Get<T>()
        {
            ValidateProperties();

            var client = ClientWithHeaders();

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

            var client = ClientWithHeaders();

            // TODO: more robust url building
            Response = await client.DeleteAsync($"{BaseAddress.ToString()}{DataSources[typeof(T)]}/{itemId}");
        }
        public override async Task<List<T>> Insert<T>(List<T> items)
        {
            ValidateProperties();

            var client = ClientWithHeaders();

            var content = new StringContent(JsonConvert.SerializeObject(items));
            Response = await client.PostAsync($"{BaseAddress.ToString()}{DataSources[typeof(T)]}", content);
            if (Response.StatusCode != HttpStatusCode.Created) return null;

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }

}