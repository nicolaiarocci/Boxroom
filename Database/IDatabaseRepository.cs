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
    }
}