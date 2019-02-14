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
    public class WebApiBox : RestBox

    {
        public override string RenderAsQueryString<T>(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                return null;
            }
            var visitor = new LambdaVisitor(filter);
            return visitor.Visit();
        }
    }

}