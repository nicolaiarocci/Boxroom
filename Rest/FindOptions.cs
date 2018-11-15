using System;

namespace DataStorage.Rest
{
    public class FindOptions<T> : Core.FindOptions<T>
    {
        public string ETag { get; set; }
    }
}