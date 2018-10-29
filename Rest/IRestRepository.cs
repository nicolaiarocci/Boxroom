using System;
using System.Collections.Generic;
using System.Net.Http;
using DataStorage.Core;

namespace DataStorage.Rest
{

    public interface IRestRepository : IRepository
    {
        Uri BaseAddress { get; set; }
        HttpClient HttpClient { get; set; }
        HttpResponseMessage Response { get; set; }
        Dictionary<string, string> Headers { get; set; }

    }
}
