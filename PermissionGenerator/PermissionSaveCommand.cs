using System.Runtime.InteropServices;

namespace EPIC.PermissionGenerator
{
    public class PermissionSaveCommand : IVsRunningDocTableEvents
    {
        public int OnBeforeSave(uint docCookie)
        {
            var dte = (EnvDTE.DTE)Marshal.GetActiveObject("VisualStudio.DTE");
            var activeDoc = dte.ActiveDocument;

            // 1. Identify if the saved file is .xaml
            runningDocumentTable.GetDocumentInfo(docCookie, out _, out _, out _, out string mkDocument, out _, out _, out _);

            if (mkDocument.EndsWith(".xaml"))
            {
                // 2. The Secret Sauce: Get the Designer's live instance
                // This bypasses the need to "re-parse" because the Designer is already running.
                var designer = GetActiveDesignerRoot(mkDocument);

                // 3. Run your VisualTreeParser logic
                var permissions = designer.GetAllLogicalChildren().GeneratePermissions();

                // 4. Write the .p.cs file
                WritePermissionFile(mkDocument, permissions);
            }
            return VSConstants.S_OK;
        }
    }
}
