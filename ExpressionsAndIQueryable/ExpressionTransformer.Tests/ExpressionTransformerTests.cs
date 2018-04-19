using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ExpressionTransformer.Tests
{
	[TestClass]
	public class ExpressionTransformerTests
	{
		private ExpressionTransformer transformer;

		[TestInitialize]
		public void Initialize()
		{
			this.transformer = new ExpressionTransformer();
		}

		[TestMethod]
		public void Transform_Decrement()
		{
			Expression<Func<int, int, int>> expr = (a, b) => (a - b) + (b - 1) - (a - 1);

			LambdaExpression result = transformer.Transform(expr) as LambdaExpression;

			Console.WriteLine(expr);
			Console.WriteLine(result);

			Assert.IsNotNull(result);
			Assert.AreEqual(expr.Compile()(1, 2), result.Compile().DynamicInvoke(1, 2));
		}

		[TestMethod]
		public void Transform_Increment()
		{
			Expression<Func<int, int, int>> expr = (a, b) => (a + b) + (b + 1) - (a + 1);

			LambdaExpression result = transformer.Transform(expr) as LambdaExpression;

			Console.WriteLine(expr);
			Console.WriteLine(result);

			Assert.IsNotNull(result);
			Assert.AreEqual(expr.Compile()(1, 2), result.Compile().DynamicInvoke(1, 2));
		}

		[TestMethod]
		public void Transform_Replace()
		{
			Expression<Func<int, int, int>> expr = (a, b) => (a + b) + b - a;
			Dictionary<string, object> constants = new Dictionary<string, object> { { "a", 3 }, { "b", 4 } };

			LambdaExpression result = transformer.Transform(expr, constants) as LambdaExpression;

			Console.WriteLine(expr);
			Console.WriteLine(result);

			Assert.IsNotNull(result);
			Assert.AreEqual(expr.Compile()(3, 4), result.Compile().DynamicInvoke());
		}

		[TestMethod]
		public void Transform()
		{
			Expression<Func<int, int, int>> expr = (a, b) => (a + b) + (b - 1) - (a + 1);
			Dictionary<string, object> constants = new Dictionary<string, object> { { "a", 3 }, { "b", 4 } };

			LambdaExpression result = transformer.Transform(expr, constants) as LambdaExpression;

			Console.WriteLine(expr);
			Console.WriteLine(result);

			Assert.IsNotNull(result);
			Assert.AreEqual(expr.Compile()(3, 4), result.Compile().DynamicInvoke());
		}
	}
}
