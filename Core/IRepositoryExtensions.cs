using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStorage.Core
{
    public static class IRepositoryExtensions
    {
        public static Task<List<T>> Get<T>(this IRepository repository)
        {
            return repository.Find<T>(_ => true);
        }
    }
}