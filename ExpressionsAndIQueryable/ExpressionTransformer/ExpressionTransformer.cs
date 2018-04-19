using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTransformer
{
	public class ExpressionTransformer : ExpressionVisitor
	{
		private Dictionary<string, object> constants;

		public Expression Transform(Expression expression, Dictionary<string, object> constants = null)
		{
			this.constants = constants;

			return Visit(expression);
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (node.Left.NodeType == ExpressionType.Parameter && IsConstantEqualsTo(node.Right, 1))
			{
				if (node.NodeType == ExpressionType.Add)
				{
					return Expression.Increment(Visit(node.Left));
				}
				else if (node.NodeType == ExpressionType.Subtract)
				{
					return Expression.Decrement(Visit(node.Left));
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
			if (this.constants == null || this.constants.Count == 0)
			{
				return base.VisitLambda(lambda);
			}

			IEnumerable<ParameterExpression> parameters = 
				lambda.Parameters.Where(p => !this.constants.ContainsKey(p.Name));
			Expression body = Visit(lambda.Body);

			return Expression.Lambda(body, parameters);
		}

		private bool IsConstantEqualsTo<T>(Expression expression, T value)
		{
			if (expression.NodeType != ExpressionType.Constant)
			{
				return false;
			}

			ConstantExpression constant = expression as ConstantExpression;

			return constant != null && constant.Type == typeof(T) && constant.Value.Equals(value);
		}

	}
}
