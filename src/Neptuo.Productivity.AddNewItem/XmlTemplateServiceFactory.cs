using Neptuo;
using Neptuo.FileSystems;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class XmlTemplateServiceFactory : ITemplateService
    {
        public const int ProbeDepth = 10;
        public const string FileName = "AddNewItem.xml";

        private static Dictionary<string, XmlTemplateService> storage = new Dictionary<string, XmlTemplateService>();

        public ITemplate FindTemplate(string path)
        {
            Ensure.NotNullOrEmpty(path, "path");
            string directoryPath = Path.GetDirectoryName(path);

            directoryPath = directoryPath.ToLowerInvariant();
            for (int i = 0; i < ProbeDepth; i++)
            {
                if (!Directory.Exists(directoryPath))
                    break;

                string filePath = Path.Combine(directoryPath, FileName);
                if (!storage.TryGetValue(filePath, out XmlTemplateService service) && File.Exists(filePath))
                    service = new XmlTemplateService(filePath);

                if (service != null)
                {
                    ITemplate template = service.FindTemplate(path);
                    if (template != null)
                        return template;
                }

                directoryPath = Path.GetDirectoryName(directoryPath);
            }

            return null;
        }
    }
}
