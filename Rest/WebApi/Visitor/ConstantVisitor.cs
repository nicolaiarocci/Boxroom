using System.Linq.Expressions;

namespace Boxroom.Rest
{
    internal class ConstantVisitor : VisitorBase
    {
        private readonly ConstantExpression node;
        public ConstantVisitor(ConstantExpression node) : base(node)
        {
            this.node = node;
        }

        public override string Visit()
        {
            return node.Value.ToString();
        }
    }
}