using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EPIC.Utilities.Extensions
{
	// Token: 0x0200005D RID: 93
	public static class QueryableExtensions
	{
		// Token: 0x060002E2 RID: 738 RVA: 0x00017CF4 File Offset: 0x00015EF4
		public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> source, IEnumerable<Expression<Func<TEntity, object>>> columns, string search, bool useIndexOf = false)
		{
			Func<MethodInfo, bool> func = null;
			Func<MethodInfo, bool> func2 = null;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "m");
			MethodInfo method = typeof(string).GetMethod("Contains");
			Dictionary<Expression<Func<TEntity, object>>, KeyValuePair<MemberExpression, ParameterExpression>> dictionary;
			List<KeyValuePair<Expression<Func<TEntity, object>>, MemberExpression>> memberExpressions = QueryableExtensions.GetMemberExpressions<TEntity>(columns, parameterExpression, out dictionary);
			Dictionary<Expression<Func<TEntity, object>>, List<string>> dictionary2;
			Dictionary<Expression<Func<TEntity, object>>, List<string>> dictionary3;
			QueryableExtensions.GetSearchTerms<TEntity>(from x in memberExpressions
			select x.Key, search, out dictionary2, out dictionary3);
			Expression expression = null;
			Expression expression2 = null;
			foreach (KeyValuePair<Expression<Func<TEntity, object>>, MemberExpression> keyValuePair in memberExpressions)
			{
				if (dictionary2.ContainsKey(keyValuePair.Key))
				{
					MemberExpression property = keyValuePair.Value;
					if (dictionary != null && dictionary.ContainsKey(keyValuePair.Key))
					{
						property = dictionary[keyValuePair.Key].Key;
					}
					MemberExpression memberExpression = null;
					if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						memberExpression = property;
						property = Expression.MakeMemberAccess(property, property.Type.GetProperty("Value"));
					}
					TypeCode typeCode = Type.GetTypeCode(property.Type);
					decimal outVar;
					if (dictionary2[keyValuePair.Key].Any((string x) => decimal.TryParse(x, out outVar)) || typeCode == TypeCode.Object || typeCode == TypeCode.Char || typeCode == TypeCode.String || typeCode == TypeCode.DateTime || typeCode == TypeCode.Boolean || property.Type.IsEnum)
					{
						using (List<string>.Enumerator enumerator2 = dictionary2[keyValuePair.Key].GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								string term = enumerator2.Current;
								Expression expression3 = null;
								if (property.Type != typeof(string))
								{
									if (property.Type == typeof(bool))
									{
										if (true.ToString().ToLower().Contains(term.ToLower()))
										{
											expression3 = Expression.Equal(property, Expression.Constant(true));
										}
										if (false.ToString().ToLower().Contains(term.ToLower()))
										{
											expression3 = ((expression3 != null) ? Expression.OrElse(expression3, Expression.Equal(property, Expression.Constant(false))) : Expression.Equal(property, Expression.Constant(false)));
										}
									}
									else if (property.Type.IsEnum)
									{
										IEnumerable<object> enumerable = from x in Enum.GetNames(property.Type)
										select new KeyValuePair<string, string>(x, (Enum.Parse(property.Type, x) as Enum).GetDisplayText()) into x
										where x.Value.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) > -1
										select Enum.Parse(property.Type, x.Key, true);
										Type underlyingType = Enum.GetUnderlyingType(property.Type);
										foreach (object value in enumerable)
										{
											expression3 = ((expression3 != null) ? Expression.OrElse(expression3, Expression.Equal(Expression.Convert(property, underlyingType), Expression.Constant(Convert.ChangeType(value, underlyingType)))) : Expression.Equal(Expression.Convert(property, underlyingType), Expression.Constant(Convert.ChangeType(value, underlyingType))));
										}
									}
									else if (property.Type == typeof(DateTime))
									{
										MethodInfo method2 = typeof(DateTime).GetMethod("ToString", new Type[0]);
										MethodCallExpression instance = Expression.Call(property, method2, null);
										if (useIndexOf)
										{
											MethodCallExpression left = Expression.Call(instance, "IndexOf", null, new Expression[]
											{
												Expression.Constant(term, typeof(string)),
												Expression.Constant(StringComparison.OrdinalIgnoreCase)
											});
											expression3 = Expression.NotEqual(left, Expression.Constant(-1));
										}
										else
										{
											expression3 = Expression.Call(instance, method, new Expression[]
											{
												Expression.Constant(term, typeof(string))
											});
										}
									}
									else
									{
										MethodInfo method2 = typeof(Convert).GetMethod("ToString", new Type[]
										{
											property.Type
										});
										MethodCallExpression instance = Expression.Call(null, method2, new Expression[]
										{
											property
										});
										if (method2 != null)
										{
											if (useIndexOf)
											{
												MethodCallExpression left = Expression.Call(instance, "IndexOf", null, new Expression[]
												{
													Expression.Constant(term, typeof(string)),
													Expression.Constant(StringComparison.OrdinalIgnoreCase)
												});
												expression3 = Expression.NotEqual(left, Expression.Constant(-1));
											}
											else
											{
												expression3 = Expression.Call(instance, method, new Expression[]
												{
													Expression.Constant(term, typeof(string))
												});
											}
										}
									}
								}
								else if (useIndexOf)
								{
									MethodCallExpression left = Expression.Call(property, "IndexOf", null, new Expression[]
									{
										Expression.Constant(term, typeof(string)),
										Expression.Constant(StringComparison.OrdinalIgnoreCase)
									});
									expression3 = Expression.NotEqual(left, Expression.Constant(-1));
								}
								else
								{
									expression3 = Expression.Call(property, method, new Expression[]
									{
										Expression.Constant(term, typeof(string))
									});
								}
								if (memberExpression != null && expression3 != null)
								{
									expression3 = Expression.AndAlso(Expression.Equal(Expression.MakeMemberAccess(memberExpression, memberExpression.Type.GetProperty("HasValue")), Expression.Constant(true)), expression3);
								}
								if (dictionary != null && expression3 != null && dictionary.ContainsKey(keyValuePair.Key))
								{
									Type type = keyValuePair.Value.Type.GenericImplementsType(typeof(IEnumerable<>)).GetGenericArguments()[0];
									Type delegateType = typeof(Func<, >).MakeGenericType(new Type[]
									{
										type,
										typeof(bool)
									});
									IEnumerable<MethodInfo> methods = typeof(Enumerable).GetMethods();
									if (func == null)
									{
										func = ((MethodInfo x) => x.Name == "Any" && x.GetParameters().Count<ParameterInfo>() == 2);
									}
									MethodInfo methodInfo = methods.First(func);
									ParameterExpression value2 = dictionary[keyValuePair.Key].Value;
									Expression expression4 = Expression.Lambda(delegateType, expression3, new ParameterExpression[]
									{
										value2
									});
									expression3 = Expression.Call(null, methodInfo.MakeGenericMethod(new Type[]
									{
										type
									}), new Expression[]
									{
										keyValuePair.Value,
										expression4
									});
								}
								if (expression3 != null)
								{
									if (dictionary3.ContainsKey(keyValuePair.Key) && dictionary3[keyValuePair.Key].Contains(term))
									{
										expression2 = ((expression2 != null) ? Expression.AndAlso(expression2, Expression.Not(expression3)) : Expression.Not(expression3));
									}
									else
									{
										expression = ((expression != null) ? Expression.OrElse(expression, expression3) : expression3);
									}
								}
							}
						}
					}
				}
			}
			Expression expression5 = null;
			if (expression != null)
			{
				expression5 = expression;
			}
			if (expression2 != null)
			{
				expression5 = ((expression != null) ? Expression.AndAlso(expression, expression2) : expression2);
			}
			IQueryable<TEntity> result;
			if (expression5 != null)
			{
				LambdaExpression expression6 = Expression.Lambda(expression5, new ParameterExpression[]
				{
					parameterExpression
				});
				IQueryProvider provider = source.Provider;
				Expression instance2 = null;
				IEnumerable<MethodInfo> methods2 = typeof(Queryable).GetMethods();
				if (func2 == null)
				{
					func2 = ((MethodInfo x) => x.Name == "Where");
				}
				result = provider.CreateQuery<TEntity>(Expression.Call(instance2, methods2.First(func2).MakeGenericMethod(new Type[]
				{
					typeof(TEntity)
				}), new Expression[]
				{
					source.Expression,
					Expression.Quote(expression6)
				}));
			}
			else
			{
				result = source;
			}
			return result;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00018B5C File Offset: 0x00016D5C
		public static void GetSearchTerms<TEntity>(IEnumerable<Expression<Func<TEntity, object>>> validColumns, string search, out Dictionary<Expression<Func<TEntity, object>>, List<string>> outTerms, out Dictionary<Expression<Func<TEntity, object>>, List<string>> outExclusions)
		{
			Func<Expression<Func<TEntity, object>>, Expression<Func<TEntity, object>>> func = null;
			Func<Expression<Func<TEntity, object>>, Expression<Func<TEntity, object>>> func2 = null;
			Func<Expression<Func<TEntity, object>>, Expression<Func<TEntity, object>>> func3 = null;
			Func<Expression<Func<TEntity, object>>, Expression<Func<TEntity, object>>> func4 = null;
			QueryableExtensions.<>c__DisplayClass30<TEntity> CS$<>8__locals1 = new QueryableExtensions.<>c__DisplayClass30<TEntity>();
			CS$<>8__locals1.search = search;
			CS$<>8__locals1.columnTerms = new Dictionary<Expression<Func<TEntity, object>>, List<string>>();
			CS$<>8__locals1.columnExclusions = new Dictionary<Expression<Func<TEntity, object>>, List<string>>();
			if (CS$<>8__locals1.search.StartsWith("\"") && CS$<>8__locals1.search.EndsWith("\""))
			{
				CS$<>8__locals1.columnTerms = validColumns.ToDictionary((Expression<Func<TEntity, object>> x) => x, (Expression<Func<TEntity, object>> x) => new List<string>
				{
					CS$<>8__locals1.search.Substring(1, CS$<>8__locals1.search.Length - 2)
				});
			}
			else
			{
				try
				{
					Regex regex = new Regex("#match all terms with ~ infront\r\n\t\t            ~(?<exclusion>[^\\s]*)|\r\n\t\t            #match all column specific terms\r\n\t\t            (?<column>(?<columnName>[^\\s]*):(~(?<columnExclusion>[^\\s]*)|(?<columnTerm>[^\\s]*)))|\r\n\t\t            #match all other terms\r\n\t\t            (?<term>[^\\s]*)\r\n\t\t            ", RegexOptions.IgnorePatternWhitespace);
					Match matchResult = regex.Match(CS$<>8__locals1.search);
					while (matchResult.Success)
					{
						if (matchResult.Groups["exclusion"].Success)
						{
							QueryableExtensions.<>c__DisplayClass30<TEntity> CS$<>8__locals3 = CS$<>8__locals1;
							if (func == null)
							{
								func = ((Expression<Func<TEntity, object>> x) => x);
							}
							CS$<>8__locals3.columnTerms = validColumns.ToDictionary(func, (Expression<Func<TEntity, object>> x) => CS$<>8__locals1.columnTerms.ContainsKey(x) ? CS$<>8__locals1.columnTerms[x].Concat(new List<string>
							{
								matchResult.Groups["exclusion"].Value
							}).ToList<string>() : new List<string>
							{
								matchResult.Groups["exclusion"].Value
							});
							QueryableExtensions.<>c__DisplayClass30<TEntity> CS$<>8__locals4 = CS$<>8__locals1;
							if (func2 == null)
							{
								func2 = ((Expression<Func<TEntity, object>> x) => x);
							}
							CS$<>8__locals4.columnExclusions = validColumns.ToDictionary(func2, (Expression<Func<TEntity, object>> x) => CS$<>8__locals1.columnExclusions.ContainsKey(x) ? CS$<>8__locals1.columnExclusions[x].Concat(new List<string>
							{
								matchResult.Groups["exclusion"].Value
							}).ToList<string>() : new List<string>
							{
								matchResult.Groups["exclusion"].Value
							});
						}
						else if (matchResult.Groups["column"].Success)
						{
							Expression<Func<TEntity, object>> expression = validColumns.FirstOrDefault(delegate(Expression<Func<TEntity, object>> x)
							{
								string expressionText = x.GetExpressionText();
								return expressionText.ToLower() == matchResult.Groups["columnName"].Value.ToLower() || expressionText.ToLower().EndsWith('.' + matchResult.Groups["columnName"].Value.ToLower());
							});
							if (expression != null)
							{
								string value = matchResult.Groups["columnTerm"].Value;
								if (matchResult.Groups["columnExclusion"].Success)
								{
									value = matchResult.Groups["columnExclusion"].Value;
									if (CS$<>8__locals1.columnExclusions.ContainsKey(expression))
									{
										CS$<>8__locals1.columnExclusions[expression].Add(value);
									}
									else
									{
										CS$<>8__locals1.columnExclusions.Add(expression, new List<string>
										{
											value
										});
									}
								}
								if (CS$<>8__locals1.columnTerms.ContainsKey(expression))
								{
									CS$<>8__locals1.columnTerms[expression].Add(value);
								}
								else
								{
									CS$<>8__locals1.columnTerms.Add(expression, new List<string>
									{
										value
									});
								}
							}
							else
							{
								QueryableExtensions.<>c__DisplayClass30<TEntity> CS$<>8__locals5 = CS$<>8__locals1;
								if (func3 == null)
								{
									func3 = ((Expression<Func<TEntity, object>> x) => x);
								}
								CS$<>8__locals5.columnTerms = validColumns.ToDictionary(func3, (Expression<Func<TEntity, object>> x) => CS$<>8__locals1.columnTerms.ContainsKey(x) ? CS$<>8__locals1.columnTerms[x].Concat(new List<string>
								{
									matchResult.Groups["column"].Value
								}).ToList<string>() : new List<string>
								{
									matchResult.Groups["column"].Value
								});
							}
						}
						else if (matchResult.Groups["term"].Success)
						{
							QueryableExtensions.<>c__DisplayClass30<TEntity> CS$<>8__locals6 = CS$<>8__locals1;
							if (func4 == null)
							{
								func4 = ((Expression<Func<TEntity, object>> x) => x);
							}
							CS$<>8__locals6.columnTerms = validColumns.ToDictionary(func4, (Expression<Func<TEntity, object>> x) => CS$<>8__locals1.columnTerms.ContainsKey(x) ? CS$<>8__locals1.columnTerms[x].Concat(new List<string>
							{
								matchResult.Groups["term"].Value
							}).ToList<string>() : new List<string>
							{
								matchResult.Groups["term"].Value
							});
						}
						matchResult = matchResult.NextMatch();
					}
				}
				catch (ArgumentException ex)
				{
				}
			}
			outTerms = CS$<>8__locals1.columnTerms.ToDictionary((KeyValuePair<Expression<Func<TEntity, object>>, List<string>> x) => x.Key, (KeyValuePair<Expression<Func<TEntity, object>>, List<string>> x) => (from y in x.Value.Distinct<string>()
			where y != ""
			select y).ToList<string>());
			outExclusions = CS$<>8__locals1.columnExclusions.ToDictionary((KeyValuePair<Expression<Func<TEntity, object>>, List<string>> x) => x.Key, (KeyValuePair<Expression<Func<TEntity, object>>, List<string>> x) => (from y in x.Value.Distinct<string>()
			where y != ""
			select y).ToList<string>());
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00018F68 File Offset: 0x00017168
		public static List<KeyValuePair<Expression<Func<TEntity, object>>, MemberExpression>> GetMemberExpressions<TEntity>(IEnumerable<Expression<Func<TEntity, object>>> columns, ParameterExpression parameter)
		{
			Dictionary<Expression<Func<TEntity, object>>, KeyValuePair<MemberExpression, ParameterExpression>> dictionary;
			return QueryableExtensions.GetMemberExpressions<TEntity>(columns, parameter, out dictionary);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00018F84 File Offset: 0x00017184
		public static List<KeyValuePair<Expression<Func<TEntity, object>>, MemberExpression>> GetMemberExpressions<TEntity>(IEnumerable<Expression<Func<TEntity, object>>> columns, ParameterExpression parameter, out Dictionary<Expression<Func<TEntity, object>>, KeyValuePair<MemberExpression, ParameterExpression>> collections)
		{
			List<KeyValuePair<Expression<Func<TEntity, object>>, MemberExpression>> list = new List<KeyValuePair<Expression<Func<TEntity, object>>, MemberExpression>>();
			collections = new Dictionary<Expression<Func<TEntity, object>>, KeyValuePair<MemberExpression, ParameterExpression>>();
			foreach (Expression<Func<TEntity, object>> expression in columns)
			{
				string[] parts = expression.GetExpressionText().Split(new char[]
				{
					'.'
				});
				MemberExpression memberExpression;
				ParameterExpression value;
				MemberExpression expressionRecursive = QueryableExtensions.GetExpressionRecursive<TEntity>(parts, parameter, out memberExpression, out value);
				if (expressionRecursive != null)
				{
					list.Add(new KeyValuePair<Expression<Func<TEntity, object>>, MemberExpression>(expression, expressionRecursive));
					if (memberExpression != null)
					{
						collections.Add(expression, new KeyValuePair<MemberExpression, ParameterExpression>(memberExpression, value));
					}
				}
			}
			return list;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00019068 File Offset: 0x00017268
		public static MemberExpression GetExpressionRecursive<TEntity>(IEnumerable<string> parts, ParameterExpression parameter, out MemberExpression innerExpression, out ParameterExpression newParam)
		{
			Func<string, int, bool> func = null;
			MemberExpression memberExpression = null;
			Type type = typeof(TEntity);
			IEnumerable<string> enumerable = parts;
			foreach (string name in parts)
			{
				PropertyInfo property = type.GetProperty(name);
				if (!(property != null))
				{
					memberExpression = null;
					break;
				}
				IEnumerable<string> source = enumerable;
				if (func == null)
				{
					func = ((string s, int i) => i > 0);
				}
				enumerable = source.Where(func);
				Type type2 = property.PropertyType.GenericImplementsType(typeof(ICollection<>));
				if (type2 != null)
				{
					Type type3 = type2.GetGenericArguments()[0];
					MemberExpression memberExpression2 = Expression.MakeMemberAccess(memberExpression ?? parameter, property);
					newParam = Expression.Parameter(type3, memberExpression2.Type.Name);
					MemberExpression memberExpression3 = null;
					ParameterExpression parameterExpression = null;
					innerExpression = (MemberExpression)((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[]
					{
						type3
					}).Invoke(null, new object[]
					{
						enumerable,
						newParam,
						memberExpression3,
						parameterExpression
					});
					return memberExpression2;
				}
				memberExpression = Expression.MakeMemberAccess(memberExpression ?? parameter, property);
				type = property.PropertyType;
			}
			newParam = null;
			innerExpression = null;
			return memberExpression;
		}
	}
}
