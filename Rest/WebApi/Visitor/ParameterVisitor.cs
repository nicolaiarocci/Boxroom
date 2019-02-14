using System.Linq.Expressions;

namespace Boxroom.Rest
{
    internal class ParameterVisitor : VisitorBase
    {
        private readonly ParameterExpression node;
        public ParameterVisitor(ParameterExpression node) : base(node)
        {
            this.node = node;
        }

        public override string Visit()
        {
            return string.Empty;
        }
    }
}