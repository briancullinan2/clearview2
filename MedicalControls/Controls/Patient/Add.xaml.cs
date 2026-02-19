using EPIC.MedicalControls.Themes;

namespace EPIC.MedicalControls.Controls.Patient
{
    public partial class Add
    {
    }

    public class AddType : ModelItemsControl<DataLayer.Entities.Patient> { internal AddType(string group, bool? uncategorized = false) : base(group, uncategorized) { } } // internal constructor so it doesn't show up in xaml designer
    public class AddBasic : AddType { public AddBasic() : base("Basic Info") { } }

}
