using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EPIC.Utilities.Extensions
{
	// Token: 0x0200005E RID: 94
	public static class TypeExtensions
	{
		// Token: 0x060002F6 RID: 758 RVA: 0x00019200 File Offset: 0x00017400
		public static bool IsLocked(this object o)
		{
			bool result;
			if (!Monitor.TryEnter(o))
			{
				result = true;
			}
			else
			{
				Monitor.Exit(o);
				result = false;
			}
			return result;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00019228 File Offset: 0x00017428
		public static IEnumerable<Type> GetBaseTypes(this Type type)
		{
			IEnumerable<Type> result;
			if (type.BaseType != null)
			{
				result = new Type[]
				{
					type.BaseType
				}.Concat(type.BaseType.GetBaseTypes());
			}
			else
			{
				result = new Type[0];
			}
			return result;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00019278 File Offset: 0x00017478
		public static Type GenericExtendsType(this Type type, Type ofType)
		{
			foreach (Type type2 in type.GetBaseTypes())
			{
				if (type2.IsGenericType)
				{
					if (type2.GetGenericTypeDefinition() == ofType)
					{
						return type2;
					}
				}
			}
			return null;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000192F8 File Offset: 0x000174F8
		public static Type GenericImplementsType(this Type type, Type ofType)
		{
			foreach (Type type2 in type.GetInterfaces())
			{
				if (type2.IsGenericType)
				{
					if (type2.GetGenericTypeDefinition() == ofType)
					{
						return type2;
					}
				}
			}
			return null;
		}
	}
}
