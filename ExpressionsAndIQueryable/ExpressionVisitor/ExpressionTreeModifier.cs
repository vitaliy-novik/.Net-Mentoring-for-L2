using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionVisitor
{
	public class ExpressionTreeModifier : System.Linq.Expressions.ExpressionVisitor
	{
		private Dictionary<string, object> constants;

		public Expression Modify<T>(Expression<T> expression, Dictionary<string, object> constants = null)
		{
			this.constants = constants;

			return VisitAndConvert(expression, "Modify");
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (node.Left.NodeType == ExpressionType.Parameter && this.IsConstantEqualsTo<int>(node.Right, 1))
			{
				if (node.NodeType == ExpressionType.Add)
				{
					return Expression.Increment(node.Left);
				}
				else if (node.NodeType == ExpressionType.Subtract)
				{
					return Expression.Decrement(node.Left);
				}
			}

			return base.VisitBinary(node);
		}

		protected override Expression VisitParameter(ParameterExpression parameter)
		{
			if (this.constants != null && this.constants.ContainsKey(parameter.Name))
			{
				return Expression.Constant(this.constants[parameter.Name]);
			}

			return base.VisitParameter(parameter);
		}

		protected override Expression VisitLambda<T>(Expression<T> lambda)
		{
			Expression body = base.Visit(lambda.Body);

			return Expression.Lambda(body, lambda.Parameters);
		}

		private bool IsConstantEqualsTo<T>(Expression node, T value)
		{
			if (node.NodeType != ExpressionType.Constant)
			{
				return false;
			}

			ConstantExpression constant = node as ConstantExpression;

			return constant != null && constant.Type == typeof(T) && constant.Value.Equals(value);
		}
	}
}
