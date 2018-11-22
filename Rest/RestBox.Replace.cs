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
    public abstract partial class RestBox : BoxroomBase
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
                // TODO throw?
            }

            var client = PreparedClient();

            var content = new StringContent(JsonConvert.SerializeObject(item));

            Response = await client.PutAsync($"{BaseAddress.ToString()}{DataSources[typeof(T)]}/{idMemberValue.ToString()}", content);
            if (Response.StatusCode != HttpStatusCode.OK) return default(T);

            var json = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);

        }
    }
}