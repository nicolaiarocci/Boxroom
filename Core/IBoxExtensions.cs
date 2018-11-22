using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Boxroom.Core
{
    public static class IBoxExtensions
    {
        public static Task<List<T>> Find<T>(this IBox box, IFindOptions<T> options = null)
        {
            return box.Find<T>(null, options);
        }
    }
}