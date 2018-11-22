using System;

namespace Boxroom.Core
{
    public interface IFindOptions<T>
    {
        DateTime? IfModifiedSince { get; set; }
    }
}