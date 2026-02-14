using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,            //where theme specific resource dictionaries are located
                                                //(used if a resource is not found in the page,
                                                // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly   //where the generic resource dictionary is located
                                                //(used if a resource is not found in the page,
                                                // app, or any theme specific resource dictionaries)
)]

[assembly: EPIC.ClearView.RootNamespaceAttribute("EPIC.ClearView")]
[assembly: System.Reflection.AssemblyCompanyAttribute("EPICClearView")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0+7b5b6f31f3c0f5392844872ebdfe54c4a1104f29")]
[assembly: System.Reflection.AssemblyProductAttribute("EPICClearView")]
[assembly: System.Reflection.AssemblyTitleAttribute("EPICClearView")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: System.Runtime.Versioning.TargetPlatformAttribute("Windows8.0")]
[assembly: System.Runtime.Versioning.SupportedOSPlatformAttribute("Windows8.0")]

// Reference to Log4Net configuration file.
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.xml", Watch = true)]

[assembly: XmlnsDefinition("EPIC.ClearView;assembly=EPICClearView", "EPICClearView")]

namespace EPIC.ClearView
{

    [AttributeUsage(AttributeTargets.Assembly)]
    public class RootNamespaceAttribute : Attribute
    {
        public string Namespace { get; }
        public RootNamespaceAttribute(string ns) => Namespace = ns;
    }
}

