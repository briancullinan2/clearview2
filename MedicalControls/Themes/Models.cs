using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace EPIC.MedicalControls.Themes
{
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

        public static ObservableCollection<System.Reflection.PropertyInfo> User = InitializeModels();
        public static ObservableCollection<System.Reflection.PropertyInfo> InitializeModels()
        {
            var props = typeof(DataLayer.Entities.User).GetProperties();
            var orders = props.Select(x => (prop: x, attr: x.GetCustomAttribute<DisplayAttribute>()))
                              .Select(x => (x.prop, x.attr, order: x.attr?.GetOrder() ?? 1000));
            var sorted = orders.OrderBy(x => x.order).Select(x => x.prop).ToList();

            var collection = new ObservableCollection<System.Reflection.PropertyInfo>(sorted);
            var view = CollectionViewSource.GetDefaultView(collection);
            view.SortDescriptions.Clear();
            return collection;
        }

        public static readonly DependencyProperty UserPropertiesProperty =
        DependencyProperty.RegisterAttached("UserProperties", typeof(ObservableCollection<System.Reflection.PropertyInfo>), typeof(Models),
        new PropertyMetadata(User));


        public static readonly DependencyProperty PassPageToViewModelProperty =
            DependencyProperty.RegisterAttached("PassPageToViewModel", typeof(bool), typeof(Models),
            new PropertyMetadata(false, (d, e) =>
            {
                //if (d is System.Windows.Controls.Page p && p.DataContext is Models vm)
                //    vm.UserProperties = InitializeModels();
            }));
        public static void SetPassPageToViewModel(UIElement element, bool value) => element.SetValue(PassPageToViewModelProperty, value);
        public static bool GetPassPageToViewModel(UIElement element) => (bool)element.GetValue(PassPageToViewModelProperty);


        public Models()
        {

        }

    }

}
