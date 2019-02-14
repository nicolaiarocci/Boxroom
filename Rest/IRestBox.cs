using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using Boxroom.Core;

namespace Boxroom.Rest
{

    public interface IRestBox : IBox
    {
        Uri BaseAddress { get; set; }
        HttpClient HttpClient { get; set; }
        HttpResponseMessage Response { get; set; }
        Dictionary<string, string> Headers { get; set; }
        string RenderAsQueryString<T>(Expression<Func<T, bool>> filter);
    }
}