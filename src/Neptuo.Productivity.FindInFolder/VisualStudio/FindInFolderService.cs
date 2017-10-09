using EnvDTE;
using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public class FindInFolderService
    {
        private readonly DTE dte;

        public FindInFolderService(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
        }

        public void Find(string folderPath)
        {
            Properties properties = dte.get_Properties("Environment", "FindAndReplace");
            Property property = properties.Item("InitializeFromEditor");
            bool flag = Convert.ToBoolean(property.Value);

            try
            {
                property.Value = false;

                string searchPath = dte.Find.SearchPath;

                dte.Find.SearchPath = folderPath;
                dte.ExecuteCommand("Edit.FindinFiles", "");
            }
            finally
            {
                property.Value = flag ? 1 : 0;
            }
        }
    }
}
