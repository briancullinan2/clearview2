using System;
using System.ComponentModel;
using System.Windows;

namespace EPIC.ClearView.Utilities
{
	// Token: 0x02000064 RID: 100
	public static class StaticEnvironment
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00019BE8 File Offset: 0x00017DE8
		public static bool IsInDesignMode
		{
			get
			{
				if (StaticEnvironment._isInDesignMode == null)
				{
					DependencyProperty isInDesignModeProperty = DesignerProperties.IsInDesignModeProperty;
					StaticEnvironment._isInDesignMode = new bool?((bool)DependencyPropertyDescriptor.FromProperty(isInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue);
				}
				return StaticEnvironment._isInDesignMode.Value;
			}
		}

		// Token: 0x04000179 RID: 377
		private static bool? _isInDesignMode;
	}
}
