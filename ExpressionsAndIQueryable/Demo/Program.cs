using ExpressionVisitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			ExpressionTreeModifier modifier = new ExpressionTreeModifier();
			Expression<Func<int, int, int, int>> expr = (a, b, c) => a + b * (c + 1);
			Console.WriteLine(expr);
			Console.WriteLine(expr.Compile()(1, 2, 3));

			Expression<Func<int, int, int, int>> expr1 = modifier.VisitAndConvert(expr, "");
			Console.WriteLine(expr1);
			Console.WriteLine(expr1.Compile()(1, 2, 3));

			Dictionary<string, object> constants = new Dictionary<string, object> { { "a", 1 }, { "d", 2 }, { "c", 3 } };
			LambdaExpression expr2 = (LambdaExpression) modifier.Modify(expr, constants);
			Console.WriteLine(expr2);
			Console.WriteLine(expr2.Compile()(1, 2, 3));


			Console.ReadKey();
		}
	}
}
