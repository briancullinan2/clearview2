using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace EPIC.ClearView.Utilities.Extensions
{
	// Token: 0x02000058 RID: 88
	public static class EnumExtensions
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x00017750 File Offset: 0x00015950
		public static string GetDisplayText(this Enum type)
		{
			Type type2 = type.GetType();
			string text = type.ToString();
			DisplayAttribute displayAttribute = type2.GetMember(text).Single<MemberInfo>().GetCustomAttributes(typeof(DisplayAttribute), false).OfType<DisplayAttribute>().SingleOrDefault<DisplayAttribute>();
			return (displayAttribute != null) ? (displayAttribute.GetDescription() ?? text) : text;
		}
	}
}
