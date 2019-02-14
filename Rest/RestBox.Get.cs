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
        public override async Task<List<T>> Find<T>(Expression<Func<T, bool>> filter, IFindOptions<T> options = null)
        {
            ValidateProperties();

            // TODO: why do I have to explictly pass 'this', otherwise I get an error? Shoud be an extension method.
            EnsureHeader.IfModifiedSince(this, options);

            var query = RenderAsQueryString(filter);

            var client = PreparedClient();

            Response = await client.GetAsync($"{TargetEndpointNormalized<T>().ToString()}{(query == null ? "" : query)}");
            if (Response.StatusCode != HttpStatusCode.OK) return null;

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        public override async Task<T> Get<T>(string itemId)
        {
            ValidateProperties();

            var client = PreparedClient();

            Response = await client.GetAsync($"{TargetEndpointNormalized<T>().ToString()}/{itemId}");
            if (Response.StatusCode != HttpStatusCode.OK) return default(T);

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}