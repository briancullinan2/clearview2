using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EPIC.DataLayer.Entities
{

    public interface IEntity : INotifyPropertyChanged
    {
        abstract public event PropertyChangedEventHandler? PropertyChanged;
        //abstract internal static IEntity Create(IEntity target);
        //abstract internal static IEntity Wrap(IEntity target);
    }

    public interface IEntity<T> : INotifyPropertyChanged, IEntity
    {
        abstract public event PropertyChangedEventHandler? PropertyChanged;
        abstract internal static T Create(T target);
        abstract internal static T Wrap(T target);
    }

    abstract public class Entity : DispatchProxy
    {
        protected abstract override object? Invoke(MethodInfo? targetMethod, object?[]? args);

        public static IEntity Wrap(IEntity target)
        {
            object generic = typeof(Entity<>).MakeGenericType(target.GetType());
            object proxy = System.Reflection.DispatchProxy.Create(generic.GetType(), target.GetType());

            var prop = proxy.GetType().GetField("_target");
            prop.SetValue(proxy, target);

            return proxy as IEntity;
        }
    }

    public class Entity<T> : Entity, IEntity<T>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected Entity() : base()
        {
            // observables don't like this because of invoke recursion bla bla bla, damn gemini is chatty
            // _target = this;
        }

        internal T _target = default!;

        // 1. You create a static helper to wrap the real object
        public static T Create(T target)
        {
            // This calls the internal EF/WPF magic to create the proxy
            object proxy = DispatchProxy.Create<T, Entity<T>>();

            // This sets the _target field so Invoke can use it later
            ((Entity<T>)proxy)._target = target;

            return (T)proxy;
        }


        public static T Wrap(T target)
        {
            // 1. This generates the 'Magic' proxy object at runtime

            return (T)Wrap((IEntity)target);
        }


        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;

            field = value;

            // "Looking for null or subs": 
            // If PropertyChanged is null (no WPF binding), nothing happens.
            // If it has subs, it notifies the UI.
            PropertyChanged?.Invoke(_target, new PropertyChangedEventArgs(propertyName));

            // Lead back to your static TranslationContext
            OnEntityChanged(propertyName);
        }

        private void OnEntityChanged(string prop)
        {
            // Add your logic to track "dirty" states for your Cloud/Disk swap
            Console.WriteLine($"Property {prop} changed. Ready for sync.");
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            var result = targetMethod?.Invoke(_target, args);

            if (targetMethod?.Name.StartsWith("set_") == true)
            {
                var propName = targetMethod.Name.Substring(4);
                // Trigger your WPF logic here
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
                OnEntityChanged(propName);
            }
            if (targetMethod?.Name.StartsWith("get_") == true && result == null)
            {
                var prop = _target.GetType().GetProperty(targetMethod.Name.Substring(4));

                if (typeof(IEntity<>).IsAssignableFrom(prop.PropertyType))
                {
                    // 1. Force EF to hit the DB/Disk/Cloud
                    TranslationContext.Current.Entry(_target).Reference(prop.Name).Load();

                    // 2. Retrieve the now-populated value
                    var rawValue = prop.GetValue(_target) as IEntity;

                    // 3. Make it "Smart" before returning it to the UI
                    result = rawValue != null ? Entities.Entity.Wrap(rawValue) : null;
                }
                else if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                {
                    // 1. Force load the collection
                    TranslationContext.Current.Entry(_target).Collection(prop.Name).Load();

                    // 2. Collections are slightly different; we want the proxy to 
                    // track the collection itself or wrap the items.
                    result = (prop.GetValue(_target) as IEnumerable<IEntity>).Select(e => Entities.Entity.Wrap(e));
                }
            }
            return result;
        }
    }
}
