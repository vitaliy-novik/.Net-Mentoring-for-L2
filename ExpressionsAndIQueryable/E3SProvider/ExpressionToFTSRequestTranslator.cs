using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Sample03
{
	public class ExpressionToFTSRequestTranslator : ExpressionVisitor
	{
		StringBuilder resultString;

		public string Translate(Expression exp)
		{
			resultString = new StringBuilder();
			Visit(exp);

			return resultString.ToString();
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof(Queryable)
				&& node.Method.Name == "Where")
			{
				var predicate = node.Arguments[1];
				Visit(predicate);

				return node;
			}
			else if (node.Method.Name == "StartsWith"
				&& node.Method.DeclaringType == typeof(String))
			{
				Visit(node.Object);
				resultString.Append("(");
				Visit(node.Arguments[0]);
				resultString.Append("*)");

				return node;
			}
			else if (node.Method.Name == "EndsWith"
				&& node.Method.DeclaringType == typeof(String))
			{
				Visit(node.Object);
				resultString.Append("(*");
				Visit(node.Arguments[0]);
				resultString.Append(")");

				return node;
			}
			else if (node.Method.Name == "Contains"
				&& node.Method.DeclaringType == typeof(String))
			{
				Visit(node.Object);
				resultString.Append("(*");
				Visit(node.Arguments[0]);
				resultString.Append("*)");

				return node;
			}

			return base.VisitMethodCall(node);
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.Equal:
					Expression member, constant;
					if (node.Left.NodeType == ExpressionType.MemberAccess 
						&& node.Right.NodeType == ExpressionType.Constant)
					{
						member = node.Left;
						constant = node.Right;
					}
					else if (node.Right.NodeType == ExpressionType.MemberAccess
						&& node.Left.NodeType == ExpressionType.Constant)
					{
						member = node.Right;
						constant = node.Left;
					}
					else
					{
						throw new NotSupportedException("One operand should be property or field, another - constant.");
					}

					Visit(member);
					resultString.Append("(");
					Visit(constant);
					resultString.Append(")");
					break;

				case ExpressionType.AndAlso:
					resultString.Append("(");
					Visit(node.Left);
					resultString.Append(")AND(");
					Visit(node.Right);
					resultString.Append(")");

					break;

				default:
					throw new NotSupportedException(string.Format("Operation {0} is not supported", node.NodeType));
			};

			return node;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			resultString.Append(node.Member.Name).Append(":");

			return base.VisitMember(node);
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			resultString.Append(node.Value);

			return node;
		}
	}
}
