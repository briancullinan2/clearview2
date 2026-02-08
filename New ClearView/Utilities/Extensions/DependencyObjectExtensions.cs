using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using EPICClearView.Utilities.Extensions;

namespace EPICClearView.Utilities.Extensions
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
	}
}
