using EPIC.MedicalControls.Themes;
using System.Windows;

namespace EPIC.MedicalControls.Controls.User
{
    public partial class Add : ResourceDictionary
    {

    }
    public class AddType : ModelItemsControl<DataLayer.Entities.User> { internal AddType(string group) : base(group) { } }
    public class AddUser : AddType { public AddUser() : base("General Info") { } }
    public class AddLogin : AddType { public AddLogin() : base("Login Info") { } }

}
