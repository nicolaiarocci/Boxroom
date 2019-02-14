using System;
using System.Linq.Expressions;
using Boxroom.Rest;

namespace Test
{
    public class Boxroom : RestBox
    {
        public override string RenderAsQueryString<T>(Expression<Func<T, bool>> filter)
        {
            return "?test=me";
        }
    }
}