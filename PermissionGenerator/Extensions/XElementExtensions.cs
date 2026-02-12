using System.Xml.Linq;

namespace EPIC.PermissionGenerator.Extensions
{
    public static class XElementExtensions
    {
        // Replaces TryFindParent/FindAncestor
        public static XElement FindAncestor(this XElement element, string localName)
        {
            return element.Ancestors().FirstOrDefault(x => x.Name.LocalName == localName);
        }

        // Replaces FindChild<T>
        public static XElement FindChild(this XElement parent, string localName, string xName = null)
        {
            return parent.Descendants()
                .FirstOrDefault(x =>
                    x.Name.LocalName == localName &&
                    (xName == null || GetNameAttribute(x) == xName));
        }

        // Replaces GetIdentity - Crucial for the address building
        public static string GetIdentity(this XElement e)
        {
            // 1. Check for x:Name or Name
            string name = GetNameAttribute(e);
            if (!string.IsNullOrEmpty(name)) return name;

            // 2. Fallback to Type[Index] within siblings of the same type
            var parent = e.Parent;
            if (parent == null) return e.Name.LocalName;

            var siblingsOfSameType = parent.Elements()
                .Where(x => x.Name.LocalName == e.Name.LocalName)
                .ToList();

            int index = siblingsOfSameType.IndexOf(e);
            return $"{e.Name.LocalName}[{index}]";
        }

        // Replaces BuildAncestralAddress
        public static string BuildXAddress(this XElement element)
        {
            var pathParts = element.AncestorsAndSelf()
                .Reverse()
                .Select(x => x.GetIdentity());

            return string.Join(".", pathParts);
        }

        public static string GetNameAttribute(this XElement e)
        {
            // Checks both "Name" and "x:Name"
            return (string)e.Attribute("Name") ??
                   (string)e.Attributes().FirstOrDefault(a => a.Name.LocalName == "Name");
        }
    }
}
