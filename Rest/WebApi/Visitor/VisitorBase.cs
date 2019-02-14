using System;
using System.Linq.Expressions;
using System.Text;

namespace Boxroom.Rest
{
    internal abstract class VisitorBase
    {
        private readonly Expression node;
        protected StringBuilder builder = new StringBuilder();

        protected VisitorBase(Expression node)
        {
            this.node = node;
        }

        public abstract string Visit();

        public ExpressionType NodeType => this.node.NodeType;
        public static VisitorBase CreateFromExpression(Expression node, VisitorOption option = VisitorOption.None)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Constant:
                    return new ConstantVisitor((ConstantExpression)node);
                case ExpressionType.Parameter:
                    return new ParameterVisitor((ParameterExpression)node);
                case ExpressionType.Equal:
                case ExpressionType.AndAlso:
                    return new BinaryVisitor((BinaryExpression)node);
                case ExpressionType.MemberAccess:
                    return new MemberVisitor((MemberExpression)node, option);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}