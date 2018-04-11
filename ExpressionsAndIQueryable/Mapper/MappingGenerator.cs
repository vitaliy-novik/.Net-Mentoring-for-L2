using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mapper
{
	public class MappingGenerator
	{
		public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
		{
			PropertyInfo[] sourceProperties = GetProperties(typeof(TSource));
			PropertyInfo[] destinationProperties = GetProperties(typeof(TDestination));

			var sourcePropNames = sourceProperties.Select(prop => prop.Name);
			var commonProperties = destinationProperties.Where(prop => sourcePropNames.Contains(prop.Name));

			Expression<Func<TSource, TDestination>> mapFunction = 
				BuildMapperFunction<TSource, TDestination>(sourceProperties, commonProperties);

			Console.WriteLine(mapFunction);

			return new Mapper<TSource, TDestination>(mapFunction.Compile());
		}

		private Expression<Func<TSource, TDestination>> BuildMapperFunction<TSource, TDestination>(
			IEnumerable<PropertyInfo> sourceProperties,
			IEnumerable<PropertyInfo> destinationProperties)
		{
			ParameterExpression parameter = Expression.Parameter(typeof(TSource));
			NewExpression constructor = Expression.New(typeof(TDestination));

			MemberBinding[] bindings = new MemberBinding[destinationProperties.Count()];
			int index = 0;
			foreach (PropertyInfo property in destinationProperties)
			{
				MemberAssignment propertyAssignment = Expression.Bind(
					property,
					Expression.MakeMemberAccess(
						parameter,
						sourceProperties.First(p => p.Name == property.Name)
					)
				);

				bindings[index] = propertyAssignment;
				index++;
			}

			MemberInitExpression memberInit = Expression.MemberInit(constructor, bindings);
			Expression<Func<TSource, TDestination>> mapFunction =
				Expression.Lambda<Func<TSource, TDestination>>(
					memberInit,
					parameter);

			return mapFunction;
		}

		private PropertyInfo[] GetProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		}
	}
}

