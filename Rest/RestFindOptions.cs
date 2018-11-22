using System;
using Boxroom.Core;

namespace Boxroom.Rest
{
    public class RestFindOptions<T> : FindOptions<T>
    {
        public string ETag { get; set; }
    }
}