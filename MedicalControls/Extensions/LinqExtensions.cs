using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace EPIC.MedicalControls.Extensions
{
    // Token: 0x0200005C RID: 92
    public static class LinqExtensions
    {
        // Token: 0x060002DE RID: 734 RVA: 0x00017874 File Offset: 0x00015A74
        public static string GetExpressionText(this LambdaExpression expression)
        {
            Stack<string> stack = new Stack<string>();
            Expression expression2 = expression.Body;
            while (expression2 != null)
            {
                if (expression2.NodeType == ExpressionType.Call)
                {
                    MethodCallExpression methodCallExpression = (MethodCallExpression)expression2;
                    if (!LinqExtensions.IsSingleArgumentIndexer(methodCallExpression))
                    {
                        break;
                    }
                    stack.Push(LinqExtensions.GetIndexerInvocation(methodCallExpression.Arguments.Single<Expression>(), expression.Parameters.ToArray<ParameterExpression>()));
                    expression2 = methodCallExpression.Object;
                }
                else if (expression2.NodeType == ExpressionType.ArrayIndex)
                {
                    BinaryExpression binaryExpression = (BinaryExpression)expression2;
                    stack.Push(LinqExtensions.GetIndexerInvocation(binaryExpression.Right, expression.Parameters.ToArray<ParameterExpression>()));
                    expression2 = binaryExpression.Left;
                }
                else if (expression2.NodeType == ExpressionType.MemberAccess)
                {
                    MemberExpression memberExpression = (MemberExpression)expression2;
                    stack.Push("." + memberExpression.Member.Name);
                    expression2 = memberExpression.Expression;
                }
                else if (expression2.NodeType == ExpressionType.Parameter)
                {
                    stack.Push(string.Empty);
                    expression2 = null;
                }
                else
                {
                    if (expression2.NodeType != ExpressionType.Convert)
                    {
                        break;
                    }
                    expression2 = ((UnaryExpression)expression2).Operand;
                }
            }
            if (stack.Count > 0 && string.Equals(stack.Peek(), ".model", StringComparison.OrdinalIgnoreCase))
            {
                stack.Pop();
            }
            string result;
            if (stack.Count > 0)
            {
                result = stack.Aggregate((string left, string right) => left + right).TrimStart(new char[]
                {
                    '.'
                });
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        // Token: 0x060002DF RID: 735 RVA: 0x00017A60 File Offset: 0x00015C60
        private static string GetIndexerInvocation(Expression expression, ParameterExpression[] parameters)
        {
            Expression body = Expression.Convert(expression, typeof(object));
            ParameterExpression parameterExpression = Expression.Parameter(typeof(object), null);
            Expression<Func<object, object>> expression2 = Expression.Lambda<Func<object, object>>(body, new ParameterExpression[]
            {
                parameterExpression
            });
            Func<object, object> func;
            try
            {
                func = expression2.Compile();
            }
            catch (InvalidOperationException innerException)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Invalid indexer expression {0} {1}", new object[]
                {
                    expression,
                    parameters[0].Name
                }), innerException);
            }
            return "[" + Convert.ToString(func(null), CultureInfo.InvariantCulture) + "]";
        }

        // Token: 0x060002E0 RID: 736 RVA: 0x00017B50 File Offset: 0x00015D50
        internal static bool IsSingleArgumentIndexer(Expression expression)
        {
            MethodCallExpression methodExpression = expression as MethodCallExpression;
            return methodExpression != null && methodExpression.Arguments.Count == 1 && methodExpression.Method.DeclaringType.GetDefaultMembers().OfType<PropertyInfo>().Any((PropertyInfo p) => p.GetGetMethod() == methodExpression.Method);
        }
    }
}
