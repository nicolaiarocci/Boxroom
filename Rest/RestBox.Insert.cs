using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Boxroom.Core;
using Newtonsoft.Json;

namespace Boxroom.Rest
{
    public abstract partial class RestBox : BoxBase
    {
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
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Response = await client.PostAsync($"{TargetEndpointNormalized<T>().ToString()}", content);
            if (Response.StatusCode != HttpStatusCode.Created) return null;

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        public override async Task<T> Insert<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            ValidateProperties();

            var client = PreparedClient();

            var content = new StringContent(JsonConvert.SerializeObject(item));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Response = await client.PostAsync($"{TargetEndpointNormalized<T>().ToString()}", content);
            if (Response.StatusCode != HttpStatusCode.Created) return default(T);

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}