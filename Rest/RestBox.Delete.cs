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
    }
}