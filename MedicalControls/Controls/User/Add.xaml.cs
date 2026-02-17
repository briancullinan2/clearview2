using EPIC.MedicalControls.Themes;
using System.Windows;

namespace EPIC.MedicalControls.Controls.User
{
    public partial class Add : ResourceDictionary
    {

    }
    //[Browsable(false)]
    //[DesignTimeVisible(false)]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    public class AddType : ModelItemsControl<DataLayer.Entities.User> { internal AddType(string group, bool? uncategorized = false) : base(group, uncategorized) { } } // internal constructor so it doesn't show up in xaml designer
    public class AddUser : AddType { public AddUser() : base("General Info") { } }
    public class AddLogin : AddType { public AddLogin() : base("Login Info") { } }
    public class AddUncategorized : AddType { public AddUncategorized() : base("") { } }
    public class AddUngroupedInfo : AddType { public AddUngroupedInfo() : base("User Info", uncategorized: true) { } }

}
