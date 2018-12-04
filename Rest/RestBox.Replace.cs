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
                throw new InvalidOperationException($"No id member has been mapped for type {typeof(T).ToString()}");
            }
            if (idMemberValue == null)
            {
                throw new InvalidOperationException($"Id member {idMemberName} has no value. A value per the Id member is needed in order to perform the {nameof(Replace)} operation.");
            }

            var client = PreparedClient();

            var content = new StringContent(JsonConvert.SerializeObject(item));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var endpoint = $"{TargetEndpointNormalized<T>().ToString()}/{idMemberValue.ToString()}";
            Response = await client.PutAsync(endpoint, content);
            if (Response.StatusCode != HttpStatusCode.OK) return default(T);

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);

        }
    }
}