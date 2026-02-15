using EPIC.DataLayer.Entities;
using System.Windows;
using System.Windows.Controls;

namespace EPIC.MedicalControls.Utilities
{
    public class GeneralTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate CheckboxTemplate { get; set; }
        public DataTemplate LinkTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // 'item' is the Row's Model (your Proxy/Entity)
            var entity = item as IEntity;
            if (entity == null) return DefaultTemplate;

            // Mold the UI based on the data state
            //if (entity.IsCheckbox) return CheckboxTemplate;
            //if (entity.HasPermissionsDetected) return LinkTemplate;

            return DefaultTemplate;
        }
    }
}
