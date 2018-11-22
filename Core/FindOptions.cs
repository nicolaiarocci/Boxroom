using System;

namespace Boxroom.Core
{
    public class FindOptions<T> : IFindOptions<T>
    {
        public DateTime? IfModifiedSince { get; set; }
    }
}