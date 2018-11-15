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
        protected override string Render<T>(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                return null;
            }

            // TODO actually do parse 'filter'.
            // (by implementing a Expression Visitor pattern). See #8.
            throw new NotImplementedException();
        }
    }

}