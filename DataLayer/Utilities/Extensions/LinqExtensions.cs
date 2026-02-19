using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EPIC.DataLayer.Utilities.Extensions
{
    public static class LinqExtensions
    {
        public static PropertyInfo FindProperty<TModel, TReturn>(this Expression<Func<TModel, TReturn>> expression)
        {
            // Unrolling the 'Advanced' type from the expression tree
            if (expression.Body is MemberExpression member && member.Member is PropertyInfo prop)
            {
                return prop;
            }

            // Handle boxing if TReturn is 'object' but the property is a struct
            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression innerMember)
            {
                return innerMember.Member as PropertyInfo;
            }

            throw new ArgumentException("Could not descend the code tree to find a valid property.");
        }
    }
}
