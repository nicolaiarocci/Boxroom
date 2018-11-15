using System;

namespace DataStorage.Core
{
    public interface IFindOptions<T>
    {
        DateTime? IfModifiedSince { get; set; }
    }
}