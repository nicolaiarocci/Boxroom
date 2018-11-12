using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataStorage.Core;

namespace DataStorage.Database
{
    public abstract class DatabaseRepositoryBase : RepositoryBase, IDatabaseRepository
    {
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
        public override void ValidateProperties()
        {

            if (DataBaseName == null)
            {
                throw new ArgumentNullException(nameof(DataBaseName));
            }
            if (ConnectionString == null)
            {
                throw new ArgumentNullException(nameof(ConnectionString));
            }
            if (DataSources == null)
            {
                throw new ArgumentNullException(nameof(DataSources));
            }
            if (DataSources.Count == 0)
            {
                throw new ArgumentException($"{nameof(DataSources)} cannot be empty", nameof(DataSources));
            }
        }
    }
}