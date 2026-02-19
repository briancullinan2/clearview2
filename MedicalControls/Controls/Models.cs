using EPIC.DataLayer;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace EPIC.MedicalControls.Controls
{
    // TODO: ?
    //[DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(EntityMetadata))]
    /*
    [Preserve]
    public class UserProperties : EntityMetadata<DataLayer.Entities.User>
    {

    }
    [Preserve]
    public class PatientProperties : EntityMetadata<DataLayer.Entities.Patient>
    {

    }
    [Preserve]
    public class SettingProperties : EntityMetadata<DataLayer.Entities.Setting>
    {

    }
    */

    public class ModelItemsControl : ItemsControl
    {
        // We store the ViewModel as 'object' or a common base/interface 
        // since we won't know 'T' at compile time anymore.
        public object ViewModel { get; protected set; }
        public static readonly DependencyProperty EntityTypeProperty = DependencyProperty.RegisterAttached("EntityType", typeof(Type), typeof(EntityMetadata), new System.Windows.PropertyMetadata(null, OnEntityTypeChanged));

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        private static void OnEntityTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Type type)
            {
                // Attach the fully cooked Metadata object to the element's DataContext or a Tag
                //var metadata = new EntityMetadata(type);
                //if (d is FrameworkElement fe) fe.DataContext = metadata;
            }
        }
        public Type EntityType
        {
            get
            {
                return (Type)base.GetValue(EntityTypeProperty);
            }
            set
            {
                base.SetValue(EntityTypeProperty, value);
                Type metadataGenericType = typeof(DataLayer.EntityMetadata<>);
                Type constructedType = metadataGenericType.MakeGenericType(value);

                // 3. Create the instance
                ViewModel = Activator.CreateInstance(constructedType)
                            ?? throw new InvalidOperationException("Failed to create ViewModel instance.");

                DataContext = ViewModel;

            }
        }

        public static void SetEntityType(UIElement element, Type value) => element.SetValue(EntityTypeProperty, value);
        public static Type GetEntityType(UIElement element) => (Type)element.GetValue(EntityTypeProperty);

        protected ModelItemsControl(Type entityType, string? groupName = null, bool? uncategorized = false)
        {
            // 1. Validate the Type
            if (!typeof(DataLayer.Entities.IEntity).IsAssignableFrom(entityType))
            {
                throw new ArgumentException($"Type {entityType.Name} must implement IEntity.");
            }

            // 2. Construct the generic EntityMetadata<T> type
            Type metadataGenericType = typeof(DataLayer.EntityMetadata<>);
            Type constructedType = metadataGenericType.MakeGenericType(entityType);

            // 3. Create the instance
            ViewModel = Activator.CreateInstance(constructedType)
                        ?? throw new InvalidOperationException("Failed to create ViewModel instance.");

            DataContext = ViewModel;

            // 4. Access properties via Reflection (or dynamic)
            // Since we know the shape of EntityMetadata<T>, 'dynamic' is the cleanest way to access properties
            dynamic vm = ViewModel;

            if (string.IsNullOrWhiteSpace(groupName) && uncategorized != true)
            {
                this.ItemsSource = vm.AllProperties;
                this.Style = TryFindResource(entityType.Name + "EntityStyle") as System.Windows.Style;
            }
            else
            {
                if (uncategorized == true || string.IsNullOrWhiteSpace(groupName))
                {
                    this.ItemsSource = (uncategorized == true)
                        ? vm.Ungrouped?[groupName]
                        : vm.Uncategorized;
                }
                else
                {
                    this.ItemsSource = vm.Groups?[groupName] ?? vm.Categories?[groupName];
                }

                this.Style = TryFindResource(entityType.Name + SafeUrlToTitle(groupName!) + "EntityStyle") as System.Windows.Style;
            }
        }

        public static string SafeUrlToTitle(string url)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;
            string[] words = Regex.Split(url, @"[^a-zA-Z0-9]+", RegexOptions.IgnoreCase);
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

            var titleCasedWords = words
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => ti.ToTitleCase(w.ToLower()));

            string result = string.Join("", titleCasedWords);
            return result.Substring(0, Math.Min(result.Length, 100));
        }
    }

    public class ModelItemsControl<T> : ModelItemsControl where T : DataLayer.Entities.IEntity<T>
    {

        public ModelItemsControl(string? groupName = null, bool? uncategorized = false) : base(typeof(T), groupName, uncategorized)
        {

        }
    }


}
