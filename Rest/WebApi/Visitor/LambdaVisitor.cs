using System;
using System.Linq.Expressions;

namespace Boxroom.Rest
{
    internal class LambdaVisitor : VisitorBase
    {
        private readonly LambdaExpression node;
        public LambdaVisitor(LambdaExpression node) : base(node)
        {
            this.node = node;
        }

        public override string Visit()
        {
            foreach (var argumentExpression in node.Parameters)
            {
                var argumentVisitor = VisitorBase.CreateFromExpression(argumentExpression);
                this.builder.Append(argumentVisitor.Visit());
            }
            var bodyVisitor = VisitorBase.CreateFromExpression(node.Body);
            this.builder.Append(bodyVisitor.Visit());
            if (this.builder.Length > 0)
            {
                this.builder.Insert(0, "?");
            }
            return Uri.EscapeUriString(this.builder.ToString());
        }
    }
}