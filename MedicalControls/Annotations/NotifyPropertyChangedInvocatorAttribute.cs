namespace EPIC.MedicalControls.Annotations
{
    // Token: 0x02000025 RID: 37
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
    {
        // Token: 0x060001C9 RID: 457 RVA: 0x000117D0 File Offset: 0x0000F9D0
        public NotifyPropertyChangedInvocatorAttribute()
        {
        }

        // Token: 0x060001CA RID: 458 RVA: 0x000117DB File Offset: 0x0000F9DB
        public NotifyPropertyChangedInvocatorAttribute(string parameterName)
        {
            this.ParameterName = parameterName;
        }

        // Token: 0x17000067 RID: 103
        // (get) Token: 0x060001CB RID: 459 RVA: 0x000117F0 File Offset: 0x0000F9F0
        // (set) Token: 0x060001CC RID: 460 RVA: 0x00011807 File Offset: 0x0000FA07
        [UsedImplicitly]
        public string ParameterName { get; private set; }
    }
}
