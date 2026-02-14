[assembly: EPIC.DataAnalysis.RootNamespaceAttribute("EPIC.DataAnalysis")]
[assembly: System.Reflection.AssemblyCompanyAttribute("DataAnalysis")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0+7b5b6f31f3c0f5392844872ebdfe54c4a1104f29")]
[assembly: System.Reflection.AssemblyProductAttribute("DataAnalysis")]
[assembly: System.Reflection.AssemblyTitleAttribute("DataAnalysis")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: System.Runtime.Versioning.TargetPlatformAttribute("Windows8.0")]
[assembly: System.Runtime.Versioning.SupportedOSPlatformAttribute("Windows8.0")]

namespace EPIC.DataAnalysis
{

    [AttributeUsage(AttributeTargets.Assembly)]
    public class RootNamespaceAttribute : Attribute
    {
        public string Namespace { get; }
        public RootNamespaceAttribute(string ns) => Namespace = ns;
    }
}
