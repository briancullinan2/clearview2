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


// Reference to Log4Net configuration file.
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.xml", Watch = true)]

[assembly: XmlnsDefinition("EPIC.ClearView;assembly=EPICClearView", "EPICClearView")]

