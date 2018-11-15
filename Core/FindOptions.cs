using System;

namespace DataStorage.Core
{
    public class FindOptions<T> : IFindOptions<T>
    {
        public DateTime? IfModifiedSince { get; set; }

    }
}