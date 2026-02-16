using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace EPIC.DataLayer.Utilities.Extensions
{
    // Token: 0x02000058 RID: 88
    public static class EnumExtensions
    {
        // Token: 0x060002D8 RID: 728 RVA: 0x00017750 File Offset: 0x00015950
        public static string GetDisplayText(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var memberName = enumValue.ToString();
            var member = type.GetMember(memberName).FirstOrDefault();
            if (member == null) return memberName;

            // 1. Check [Display(Description = "")] - Highest priority for modern .NET
            var display = member.GetCustomAttribute<DisplayAttribute>();
            if (!string.IsNullOrEmpty(display?.GetDescription())) return display.GetDescription()!;

            // 2. Check [EnumMember(Value = "")] - Priority for Cloud/Serialization
            var enumMember = member.GetCustomAttribute<EnumMemberAttribute>();
            if (!string.IsNullOrEmpty(enumMember?.Value)) return enumMember.Value;

            // 3. Check [Description("")] - Standard legacy WPF/WinForms support
            var description = member.GetCustomAttribute<DescriptionAttribute>();
            if (!string.IsNullOrEmpty(description?.Description)) return description.Description;

            // Fallback: The raw string (munged or otherwise)
            return memberName;
        }
    }
}
