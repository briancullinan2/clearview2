using SQLitePCL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace EPIC.MedicalControls.Themes
{
    public class EntityMetadata : DependencyObject, INotifyPropertyChanged
    {
        public static readonly DependencyProperty EntityTypeProperty =
        DependencyProperty.RegisterAttached("EntityType", typeof(Type), typeof(EntityMetadata), new PropertyMetadata(null, OnEntityTypeChanged));

        public event PropertyChangedEventHandler? PropertyChanged;

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
                var props = value.GetProperties()
                                 .Select(p => new { p, attr = p.GetCustomAttribute<DisplayAttribute>() })
                                 .OrderBy(x => x.attr?.GetOrder() ?? 1000)
                                 .Select(x => x.p)
                                 .ToList();

                AllProperties = new ObservableCollection<PropertyInfo>(props);

                // Nested Indexers for the XAML [Brackets]
                Groups = new AttributeIndexer(AllProperties, p => p.GetCustomAttribute<DisplayAttribute>()?.GroupName);
                Categories = new AttributeIndexer(AllProperties, p => p.GetCustomAttribute<CategoryAttribute>()?.Category);

            }
        }

        public static void SetEntityType(UIElement element, Type value) => element.SetValue(EntityTypeProperty, value);
        public static Type GetEntityType(UIElement element) => (Type)element.GetValue(EntityTypeProperty);
        public ObservableCollection<PropertyInfo> AllProperties { get; private set; }
        public AttributeIndexer Groups { get; private set; }
        public AttributeIndexer Categories { get; private set; }

        public EntityMetadata() : base()
        {

        }

        public EntityMetadata(Type entityType) : base()
        {
            EntityType = entityType;

            // Reflect and Sort - The "One Line" Engine
        }

        public class AttributeIndexer
        {
            private readonly IEnumerable<PropertyInfo> _source;
            private readonly Func<PropertyInfo, string?> _selector;
            private readonly Dictionary<string, ObservableCollection<PropertyInfo>> _cache = new();

            public AttributeIndexer(IEnumerable<PropertyInfo> source, Func<PropertyInfo, string?> selector)
            {
                _source = source;
                _selector = selector;
            }

            public ObservableCollection<PropertyInfo> this[string key] =>
                _cache.TryGetValue(key, out var list) ? list : _cache[key] =
                new ObservableCollection<PropertyInfo>(_source.Where(p => _selector(p) == key));
        }
    }

    public class EntityMetadata<T> : EntityMetadata where T : DataLayer.Entities.IEntity<T>
    {
        public EntityMetadata() : base(typeof(T))
        {
        }
    }

    public class Models // : DependencyObject
    {
        // One line per entity, just like a DbContext!
        public static EntityMetadata<DataLayer.Entities.User> User => new EntityMetadata<DataLayer.Entities.User>();
        public static EntityMetadata<DataLayer.Entities.Patient> Patient => new EntityMetadata<DataLayer.Entities.Patient>();
        public static EntityMetadata<DataLayer.Entities.Setting> Setting => new EntityMetadata<DataLayer.Entities.Setting>();
        public EntityMetadata? this[string key]
        {
            get
            {
                return typeof(Models).GetProperty(key, BindingFlags.Static | BindingFlags.Public)?.GetValue(null) as EntityMetadata;
            }
        }
    }

    //[DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(EntityMetadata))]
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


    public class ModelItemsControl<T> : ItemsControl where T : DataLayer.Entities.IEntity<T>
    {
        public static string SafeUrlToTitle(string url)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;

            // 1. Split by anything NOT a letter or number
            // Regex.Split will find all non-alphanumeric spots
            string[] words = Regex.Split(url, @"[^a-zA-Z0-9]+", RegexOptions.IgnoreCase);

            // 2. Use TextInfo for Title Casing
            // We ToLower() first because ToTitleCase ignores ALL-CAPS words (acronyms)
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

            var titleCasedWords = words
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => ti.ToTitleCase(w.ToLower()));

            // 3. Join with spaces and cap at 100 chars
            string result = string.Join("", titleCasedWords);
            return result.Substring(0, Math.Min(result.Length, 100));
        }

        public ModelItemsControl()
        {
            ViewModel = new EntityMetadata<T>();
            DataContext = ViewModel;
            ItemsSource = ViewModel.AllProperties;
            Style = FindResource(typeof(T).Name + "EntityStyle") as System.Windows.Style;
        }

        public ModelItemsControl(string GroupName)
        {
            ViewModel = new EntityMetadata<T>();
            DataContext = ViewModel;
            ItemsSource = ViewModel.Groups[GroupName];
            Style = TryFindResource(typeof(T).Name + SafeUrlToTitle(GroupName) + "EntityStyle") as System.Windows.Style;
        }

        public Themes.EntityMetadata<T> ViewModel { get; private set; }
    }

    /*

    public class Models : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<System.Reflection.PropertyInfo> UserProperties
        {
            get
            {
                return (ObservableCollection<System.Reflection.PropertyInfo>)base.GetValue(UserPropertiesProperty);
            }
            private set
            {
                base.SetValue(UserPropertiesProperty, value);
            }
        }

        public ObservableCollection<System.Reflection.PropertyInfo> UserGroups[string value]
        {
            get
            {

            }
        }

        public static ObservableCollection<System.Reflection.PropertyInfo> User = InitializeModels();
        private static ObservableCollection<System.Reflection.PropertyInfo> InitializeModels()
        {
            var props = typeof(DataLayer.Entities.User).GetProperties();
            var orders = props.Select(x => (prop: x, attr: x.GetCustomAttribute<DisplayAttribute>()))
                              .Select(x => (x.prop, x.attr, order: x.attr?.GetOrder() ?? 1000));
            var sorted = orders.OrderBy(x => x.order).Select(x => x.prop).ToList();

            var collection = new ObservableCollection<System.Reflection.PropertyInfo>(sorted);
            //var view = CollectionViewSource.GetDefaultView(collection);
            //view.SortDescriptions.Clear();
            return collection;
        }

        public static readonly DependencyProperty UserPropertiesProperty =
        DependencyProperty.RegisterAttached("UserProperties", typeof(ObservableCollection<System.Reflection.PropertyInfo>), typeof(Models),
        new PropertyMetadata(User));


        public static readonly DependencyProperty PassPageToViewModelProperty =
            DependencyProperty.RegisterAttached("PassPageToViewModel", typeof(bool), typeof(Models), new PropertyMetadata(false));
        public static void SetPassPageToViewModel(UIElement element, bool value) => element.SetValue(PassPageToViewModelProperty, value);
        public static bool GetPassPageToViewModel(UIElement element) => (bool)element.GetValue(PassPageToViewModelProperty);


        public Models()
        {

        }

    }
    */

}
