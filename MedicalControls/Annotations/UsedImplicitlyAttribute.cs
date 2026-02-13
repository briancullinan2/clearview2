namespace EPIC.MedicalControls.Annotations
{
    // Token: 0x0200002B RID: 43
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class UsedImplicitlyAttribute : Attribute
    {
        // Token: 0x060001D9 RID: 473 RVA: 0x000118DC File Offset: 0x0000FADC
        [UsedImplicitly]
        public UsedImplicitlyAttribute() : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
        {
        }

        // Token: 0x060001DA RID: 474 RVA: 0x000118E9 File Offset: 0x0000FAE9
        [UsedImplicitly]
        public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
        {
            this.UseKindFlags = useKindFlags;
            this.TargetFlags = targetFlags;
        }

        // Token: 0x060001DB RID: 475 RVA: 0x00011904 File Offset: 0x0000FB04
        [UsedImplicitly]
        public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags) : this(useKindFlags, ImplicitUseTargetFlags.Default)
        {
        }

        // Token: 0x060001DC RID: 476 RVA: 0x00011911 File Offset: 0x0000FB11
        [UsedImplicitly]
        public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags) : this(ImplicitUseKindFlags.Default, targetFlags)
        {
        }

        // Token: 0x1700006B RID: 107
        // (get) Token: 0x060001DD RID: 477 RVA: 0x00011920 File Offset: 0x0000FB20
        // (set) Token: 0x060001DE RID: 478 RVA: 0x00011937 File Offset: 0x0000FB37
        [UsedImplicitly]
        public ImplicitUseKindFlags UseKindFlags { get; private set; }

        // Token: 0x1700006C RID: 108
        // (get) Token: 0x060001DF RID: 479 RVA: 0x00011940 File Offset: 0x0000FB40
        // (set) Token: 0x060001E0 RID: 480 RVA: 0x00011957 File Offset: 0x0000FB57
        [UsedImplicitly]
        public ImplicitUseTargetFlags TargetFlags { get; private set; }
    }
}
