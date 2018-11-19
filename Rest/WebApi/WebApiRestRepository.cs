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
        public override string Render<T>(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                return null;
            }

            // TODO actually parse 'filter'.
            // By implementing a Expression Visitor pattern. See #8.
            throw new NotImplementedException();
        }
    }

}