using System;
using System.Linq.Expressions;

namespace Boxroom.Rest
{
    internal class BinaryVisitor : VisitorBase
    {
        private readonly BinaryExpression node;
        public BinaryVisitor(BinaryExpression node) : base(node)
        {
            this.node = node;
        }

        public override string Visit()
        {
            var left = VisitorBase.CreateFromExpression(this.node.Left, VisitorOption.Left);
            this.builder.Append(left.Visit());

            this.builder.Append(Operator());

            var right = VisitorBase.CreateFromExpression(this.node.Right, VisitorOption.Right);
            this.builder.Append(right.Visit());

            return this.builder.ToString();
        }
        private string Operator()
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    {
                        return "=";
                    }
                case ExpressionType.AndAlso:
                    {
                        return "&";
                    }
                default:
                    {
                        throw new NotSupportedException();
                    }
            }

        }
    }
}