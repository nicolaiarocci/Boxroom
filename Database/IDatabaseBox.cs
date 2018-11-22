using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Boxroom.Core;

namespace Boxroom.Database
{
    public interface IDatabaseBox : IBoxroom
    {
        string ConnectionString { get; set; }
        string DataBaseName { get; set; }
    }
}