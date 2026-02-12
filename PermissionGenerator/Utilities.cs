using EPIC.PermissionGenerator.Extensions;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml.Linq;

namespace EPIC.PermissionGenerator
{
    public static class Utilities
    {

        public static IEnumerable<string> GetBamlFiles()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return GetBamlFiles(assembly);
        }

        public static IEnumerable<string> GetBamlFiles(string assemblyFile)
        {
            var assembly = Assembly.LoadFrom(assemblyFile);
            return GetBamlFiles(assembly);
        }
        public static IEnumerable<string> GetBamlFiles(Assembly assembly)
        {
            // BAML resources are usually stored in [AssemblyName].g.resources
            var resourceName = assembly.GetName().Name + ".g.resources";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    yield break;

                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    foreach (DictionaryEntry entry in reader)
                    {
                        if (entry.Key.ToString().EndsWith(".baml"))
                        {
                            // We found a page! Now we introspect it.
                            yield return entry.Key.ToString();
                        }
                    }
                }
            }
        }


        public static IEnumerable<DataLayer.Entities.Permission> IntrospectXaml(System.Reflection.Assembly assembly, FrameworkElement root, string name)
        {

            if (root == null)
                return [];

            string defaultNamespace = string.Join(".", Path.GetDirectoryName(name).Replace('\\', '/').Split('/').Select(s => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s)));
            var allChildren = root.GetAllLogicalChildren().Concat(root.GetAllChildren()).Distinct();
            var allElements = allChildren
                                  .OfType<FrameworkElement>()
                                  .Where(e => e is ICommandSource || e is ButtonBase || !String.IsNullOrWhiteSpace(e.Name) || !String.IsNullOrWhiteSpace(e.GetDescriptor()))
                                  .Select(e => new DataLayer.Entities.Permission
                                  {
                                      Name = e.BuildAncestralAddress(root) + "." + e.GetType().Name, // Using your JS-style logic
                                      Description = "Interaction access to " + e.GetType().Name + " labeled " + e.GetDescriptor() + " in the " + name + " baml",
                                      IsActionable = e is ICommandSource || e is ButtonBase
                                  });
            return allElements;
        }

        public static IEnumerable<DataLayer.Entities.Permission?> IntrospectXDocument(XDocument root, string bamlName)
        {
            if (root?.Root == null) yield return null;

            // List of local names that imply ICommandSource or ButtonBase in WPF
            var actionableTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Button", "MenuItem", "CheckBox", "RadioButton", "RepeatButton", "Hyperlink"
            };

            // Flatten the document in order
            var allChildren = root.Root.DescendantsAndSelf();
            foreach (var child in allChildren)
            {
                string typeName = child.Name.LocalName;
                string descriptor = child.GetNameAttribute(); // Uses our XElement extension for Content/Header
                bool isActionable = actionableTypes.Contains(typeName);

                // Filter: Only create permissions for actionable items or items with descriptive labels
                if (!isActionable && string.IsNullOrWhiteSpace(descriptor))
                    continue;

                yield return new DataLayer.Entities.Permission
                {
                    // Uses BuildXAddress from our previous extension
                    Name = $"{child.BuildXAddress()}.{typeName}",

                    Description = $"Interaction access to {typeName} labeled '{descriptor}' in the {bamlName} baml",

                    IsActionable = isActionable
                };
            }
        }

        public static IEnumerable<DataLayer.Entities.Permission> IntrospectXaml(string bamlPath)
        {
            // Load the object graph without rendering it
            var assembly = Assembly.GetExecutingAssembly();
            var root = Application.LoadComponent(new Uri(bamlPath.Replace(".baml", ".xaml"), UriKind.Relative)) as FrameworkElement;
            //var name = Path.GetFileNameWithoutExtension();

            return IntrospectXaml(assembly, root, bamlPath);
        }

    }
}
