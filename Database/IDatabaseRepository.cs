using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataStorage.Core;

namespace DataStorage.Database
{
    public interface IDatabaseRepository : IRepository
    {
        string ConnectionString { get; set; }
        string DataBaseName { get; set; }
        // TODO these two below should really move to IRepository
        Task<List<T>> Get<T>(Expression<Func<T, bool>> filter);
        Task<T> Delete<T>(Expression<Func<T, bool>> filter);
    }
}