using System;
using DataStorage.Core;

namespace DataStorage.Rest
{
    public class RestFindOptions<T> : FindOptions<T>
    {
        public string ETag { get; set; }
    }
}