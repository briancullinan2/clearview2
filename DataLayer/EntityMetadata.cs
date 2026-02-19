using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace EPIC.DataLayer
{
    public class PropertyMetadata
    {
        // The "Old Object" we are wrapping
        protected readonly PropertyInfo _info;

        public IEnumerable<Attribute> Custom { get; private set; }

        public string Name => _info.Name;
        public Type PropertyType => _info.PropertyType;
        public MemberTypes MemberType => _info.MemberType;
        public Type? DeclaringType => _info.DeclaringType;

        // Your custom extended properties
        public int? MaxLength { get; private set; }
        public string DisplayName { get; private set; }
        public string? GroupName { get; private set; }
        public int? Order { get; private set; }
        public string? Category { get; private set; }

        internal PropertyMetadata(PropertyInfo info)
        {
            _info = info ?? throw new ArgumentNullException(nameof(info));

            // Initialize your custom lookups once here
            Custom = _info.GetCustomAttributes();
            MaxLength = _info.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? _info.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength;
            DisplayName = _info.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? _info.Name;
            GroupName = _info.GetCustomAttribute<DisplayAttribute>()?.GetGroupName();
            Category = _info.GetCustomAttribute<CategoryAttribute>()?.Category;
            Order = _info.GetCustomAttribute<DisplayAttribute>()?.GetOrder();
        }

        // You can even wrap the actual Get/Set calls
        public object? GetValue(object obj) => _info.GetValue(obj);
        public void SetValue(object obj, object? value) => _info.SetValue(obj, value);
    }

    //[AttributeUsage(AttributeTargets.Class)]
    //public class GenerateIndexerPropertiesAttribute : Attribute { }
    //[GenerateIndexerProperties]
    public class AttributeValueIndexer<TValue>
    {
        private readonly IEnumerable<PropertyMetadata> _source;
        private readonly Func<PropertyMetadata, TValue> _selector;
        private readonly Dictionary<string, TValue?> _cache = new();

        public AttributeValueIndexer(IEnumerable<PropertyMetadata> source, Func<PropertyMetadata, TValue> selector)
        {
            _source = source;
            _selector = selector;
        }

        public TValue? this[string propertyName]
        {
            get
            {
                if (_cache.TryGetValue(propertyName, out var value)) return value;

                var prop = _source.FirstOrDefault(p => p.Name == propertyName);
                return _cache[propertyName] = (prop != null) ? _selector(prop) : default;
            }
        }
    }
    public class AttributeIndexer
    {
        private readonly IEnumerable<PropertyMetadata> _source;
        private readonly Func<PropertyMetadata, string?> _selector;
        private readonly Dictionary<string, ObservableCollection<PropertyMetadata>> _cache = new();

        public AttributeIndexer(IEnumerable<PropertyMetadata> source, Func<PropertyMetadata, string?> selector)
        {
            _source = source;
            _selector = selector;
        }

        public ObservableCollection<PropertyMetadata>? this[string key]
        {
            get
            {
                if (_cache.TryGetValue(key, out var list)) return list;

                var props = _source.Where(p => _selector(p) == key);
                if (props.Count() > 0) return new ObservableCollection<PropertyMetadata>(props);
                return null;

            }
        }
    }

    public class AttributeTypeIndexer
    {
        private readonly IEnumerable<PropertyMetadata> _source;
        private readonly Dictionary<string, ObservableCollection<Attribute>> _cache = new();

        public AttributeTypeIndexer(IEnumerable<PropertyMetadata> source)
        {
            _source = source;
        }

        public ObservableCollection<Attribute>? this[string key]
        {
            get
            {
                if (_cache.TryGetValue(key, out var list)) return list;
                // TODO: compare types the attribute is on like Types[string]
                IEnumerable<Attribute> attributes = _source
                                  .Select(obj => (p: obj, match: obj.Custom.FirstOrDefault(attr => attr.GetType().Name.Replace("Attribute", "") == key)))
                                  .Where(obj => obj.match != null)
                                  .Select(obj => obj.match)
                                  .OfType<Attribute>();
                if (attributes.Any()) return new ObservableCollection<Attribute>(attributes);
                return null;

            }
        }
    }

    public class EntityMetadata : INotifyPropertyChanged
    {
        public static EntityMetadata<Entities.User> User => new EntityMetadata<Entities.User>();
        public static EntityMetadata<Entities.Patient> Patient => new EntityMetadata<Entities.Patient>();
        public static EntityMetadata<Entities.Setting> Setting => new EntityMetadata<Entities.Setting>();
        public static EntityMetadata<Entities.Message> Message => new EntityMetadata<Entities.Message>();
        public EntityMetadata? this[string key] => typeof(EntityMetadata).GetProperty(key, BindingFlags.Static | BindingFlags.Public)?.GetValue(null) as EntityMetadata;

        public ObservableCollection<PropertyMetadata> AllProperties { get; private set; }
        public ObservableCollection<PropertyMetadata> Uncategorized { get; private set; }
        public AttributeIndexer Groups { get; private set; }
        public AttributeIndexer Categories { get; private set; }
        public AttributeIndexer Ungrouped { get; private set; }
        public AttributeValueIndexer<int?> MaxLength { get; private set; }
        public AttributeTypeIndexer Attributes { get; private set; }
        public Type EntityType { get; private set; }


        protected EntityMetadata(Type entityType) : base()
        {
            EntityType = entityType;

            var props = entityType.GetProperties()
                                             .Select(p => new { p, attr = p.GetCustomAttribute<DisplayAttribute>() })
                                             .OrderBy(x => x.attr?.GetOrder() ?? 1000)
                                             .Select(x => x.p)
                                             .ToList();

            AllProperties = new ObservableCollection<PropertyMetadata>(props.Select(p => new PropertyMetadata(p)));
            Uncategorized = new ObservableCollection<PropertyMetadata>(AllProperties.Where(p => string.IsNullOrWhiteSpace(p.Category) && string.IsNullOrWhiteSpace(p.GroupName)));

            // Nested Indexers for the XAML [Brackets]
            Groups = new AttributeIndexer(AllProperties, p => p.GroupName);
            Categories = new AttributeIndexer(AllProperties, p => p.Category);
            Ungrouped = new AttributeIndexer(AllProperties.Where(p => string.IsNullOrWhiteSpace(p.GroupName)), p => p.Category);
            MaxLength = new AttributeValueIndexer<int?>(AllProperties, p => p.MaxLength);
            Attributes = new AttributeTypeIndexer(AllProperties);
        }

        // TODO: ? i guess update if the attribute in the change?
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class EntityMetadata<T> : EntityMetadata where T : Entities.IEntity<T>
    {
        public EntityMetadata() : base(typeof(T))
        {
        }
    }


}
