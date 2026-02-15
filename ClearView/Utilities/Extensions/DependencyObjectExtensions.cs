using System.Windows;
using System.Windows.Media;

namespace EPIC.ClearView.Utilities.Extensions
{
    // Token: 0x02000067 RID: 103
    public static class DependencyObjectExtensions
    {
        // Token: 0x0600031F RID: 799 RVA: 0x00019ECC File Offset: 0x000180CC
        public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = child.GetParentObject();
            T result;
            if (parentObject == null)
            {
                result = default(T);
            }
            else
            {
                T t = parentObject as T;
                T t2;
                if ((t2 = t) == null)
                {
                    t2 = parentObject.TryFindParent<T>();
                }
                result = t2;
            }
            return result;
        }

        // Token: 0x06000320 RID: 800 RVA: 0x00019F1C File Offset: 0x0001811C
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            DependencyObject result;
            if (child == null)
            {
                result = null;
            }
            else
            {
                ContentElement contentElement = child as ContentElement;
                if (contentElement != null)
                {
                    DependencyObject parent = ContentOperations.GetParent(contentElement);
                    if (parent != null)
                    {
                        result = parent;
                    }
                    else
                    {
                        FrameworkContentElement frameworkContentElement = contentElement as FrameworkContentElement;
                        result = ((frameworkContentElement != null) ? frameworkContentElement.Parent : null);
                    }
                }
                else
                {
                    FrameworkElement frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null)
                    {
                        DependencyObject parent = frameworkElement.Parent;
                        if (parent != null)
                        {
                            return parent;
                        }
                    }
                    result = VisualTreeHelper.GetParent(child);
                }
            }
            return result;
        }

        // Token: 0x06000321 RID: 801 RVA: 0x00019FB4 File Offset: 0x000181B4
        public static T FindAncestor<T>(this DependencyObject that) where T : DependencyObject
        {
            DependencyObject parentObject = that.GetParentObject();
            while (!object.ReferenceEquals(parentObject, null))
            {
                if (parentObject is T)
                {
                    return parentObject as T;
                }
                parentObject = parentObject.GetParentObject();
            }
            return default(T);
        }

        // Token: 0x06000322 RID: 802 RVA: 0x0001A010 File Offset: 0x00018210
        public static T FindChild<T>(this DependencyObject that, string elementName = null) where T : FrameworkElement
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(that);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(that, i);
                FrameworkElement frameworkElement = child as FrameworkElement;
                if (frameworkElement != null)
                {
                    T result;
                    if (string.IsNullOrEmpty(elementName) && frameworkElement is T)
                    {
                        result = (frameworkElement as T);
                    }
                    else if (elementName == frameworkElement.Name && frameworkElement is T)
                    {
                        result = (frameworkElement as T);
                    }
                    else
                    {
                        if ((frameworkElement = frameworkElement.FindChild<FrameworkElement>(elementName)) == null)
                        {
                            goto IL_B4;
                        }
                        result = (frameworkElement as T);
                    }
                    return result;
                }
            IL_B4:;
            }
            return default(T);
        }

        // Token: 0x06000323 RID: 803 RVA: 0x0001A25C File Offset: 0x0001845C
        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject item)
        {
            if (!object.ReferenceEquals(item, null))
            {
                DependencyObject curItem = item.GetParentObject();
                while (!object.ReferenceEquals(curItem, null))
                {
                    yield return curItem;
                    curItem = curItem.GetParentObject();
                }
            }
            yield break;
        }

        public static IEnumerable<DependencyObject> GetAllChildren(this DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                yield return child;
                foreach (var descendants in GetAllChildren(child))
                {
                    yield return descendants;
                }
            }
        }

        public static IEnumerable<DependencyObject> GetAllLogicalChildren(this DependencyObject parent)
        {
            if (parent == null) yield break;

            // LogicalTreeHelper handles Page.Content and Ribbon.Items automatically
            foreach (var child in LogicalTreeHelper.GetChildren(parent))
            {
                if (child is DependencyObject depObj)
                {
                    yield return depObj;
                    foreach (var descendant in depObj.GetAllLogicalChildren())
                    {
                        yield return descendant;
                    }
                }
            }
        }

        public static string? GetDescriptor(this DependencyObject obj)
        {
            // Check common WPF properties via reflection
            var props = new[] { "Name", "Title", "Header", "Text", "Label", "Content" };
            foreach (var name in props)
            {
                var prop = obj.GetType().GetProperty(name);
                if (prop == null) continue;
                var value = prop.GetValue(obj);

                // skip content because lots has this
                if (name == "Content" && typeof(string).IsAssignableFrom(value?.GetType()))
                    continue;

                if (!string.IsNullOrEmpty(value?.ToString())) return value.ToString();
            }
            return null;
        }


        public static string BuildAncestralAddress(this FrameworkElement element, FrameworkElement root)
        {
            // 1. Get the Full Long Path (The "Safety" Address)
            var ancestors = element.GetAncestors().OfType<FrameworkElement>().Reverse().ToList();
            var pathParts = ancestors.Select(a => GetIdentity(a)).ToList();
            pathParts.Add(GetIdentity(element));

            // 2. The "Removal Test" - Try to shorten from the left
            //for (int i = 0; i < pathParts.Count - 1; i++)
            //{
            //var testPath = string.Join(">", pathParts.Skip(i + 1));

            // Count how many elements in the root match this specific tail
            //int matches = root.GetAllChildren()
            //                  .OfType<FrameworkElement>()
            //                  .Count(e => e.BuildLongPath().EndsWith(testPath));

            //if (matches == 1)
            //{
            //return testPath; // This is the shortest unique path!
            //}
            //}

            return string.Join(".", pathParts); // Fallback to full path
        }

        private static string GetIdentity(FrameworkElement e)
        {
            // Use Name if it exists, otherwise use Type + Index in parent
            if (!string.IsNullOrEmpty(e.Name)) return e.Name;

            var parent = VisualTreeHelper.GetParent(e);
            if (parent == null) return e.GetType().Name;

            int index = 0;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                if (VisualTreeHelper.GetChild(parent, i) == e) { index = i; break; }
            }
            return $"{e.GetType().Name}[{index}]";
        }
    }
}
