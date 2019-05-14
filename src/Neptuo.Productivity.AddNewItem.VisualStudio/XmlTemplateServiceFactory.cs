using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    [Export(typeof(ITemplateService))]
    [Name(Name)]
    public class XmlTemplateServiceFactory : ITemplateService
    {
        public const string Name = "Xml";
        public const string FileName = "AddNewItem.xml";

        private static Dictionary<string, XmlTemplateService> storage = new Dictionary<string, XmlTemplateService>();

        public ITemplate FindTemplate(string path)
        {
            Ensure.NotNullOrEmpty(path, "path");

            foreach (string filePath in GetConfigurationFilePaths(path))
            {
                if (!storage.TryGetValue(filePath, out XmlTemplateService service))
                    service = new XmlTemplateService(filePath);

                if (service != null)
                {
                    ITemplate template = service.FindTemplate(path);
                    if (template != null)
                        return template;

                    if (service.IsStandalone)
                        break;
                }
            }

            return null;
        }

        private IEnumerable<string> GetConfigurationFilePaths(string path)
        {
            string directoryPath = Path.GetDirectoryName(path);

            directoryPath = directoryPath.ToLowerInvariant();
            while (!String.IsNullOrEmpty(directoryPath))
            {
                if (Directory.Exists(directoryPath))
                {
                    string filePath = Path.Combine(directoryPath, FileName);
                    if (File.Exists(filePath))
                        yield return filePath;
                }

                directoryPath = Path.GetDirectoryName(directoryPath);
            }
        }
    }
}
