using System;
using System.Linq.Expressions;
using DataStorage.Rest;

namespace Test
{
    public class Repository : RestRepository
    {
        public override string Render<T>(Expression<Func<T, bool>> filter)
        {
            return "?test=me";
        }
    }
}