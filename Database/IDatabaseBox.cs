using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Boxroom.Core;

namespace Boxroom.Database
{
    public interface IDatabaseBox : IBox
    {
        string ConnectionString { get; set; }
        string DataBaseName { get; set; }
    }
}