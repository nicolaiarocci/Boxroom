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
    public abstract partial class RestRepositoryBase : RepositoryBase
    {
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
    }
}