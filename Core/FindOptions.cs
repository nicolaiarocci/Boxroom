using System;

namespace DataStorage.Core
{
    public class FindOptions<T>
    {
        public DateTime? IfModifiedSince { get; set; }

    }
}