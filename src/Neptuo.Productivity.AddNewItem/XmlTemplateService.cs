using Neptuo.FileSystems;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Neptuo.Productivity
{
    public class XmlTemplateService : ITemplateService
    {
        private readonly TemplateList list;

        public XmlTemplateService(string sourcePath)
        {
            Ensure.Condition.FileExists(sourcePath, "sourcePath");

            XmlSerializer serializer = new XmlSerializer(typeof(TemplateList));
            using (FileStream sourceContent = new FileStream(sourcePath, FileMode.Open))
                list = (TemplateList)serializer.Deserialize(sourceContent);
        }

        public ITemplate FindTemplate(string path)
        {
            throw new NotImplementedException();
        }

        [XmlRoot("Templates")]
        public class TemplateList : List<TemplateNode>
        { }

        [XmlType("Template")]
        [XmlRoot("Template")]
        public class TemplateNode
        {
            [XmlElement("Selector")]
            public List<SelectorNode> Selector { get; set; }

            [XmlElement]
            public string Content { get; set; }

            [XmlElement]
            public SnippetNode Snippet { get; set; }

            [XmlElement]
            public FileNode File { get; set; }

            [XmlElement]
            public VsTemplateNode VsTemplate { get; set; }
        }

        public class SelectorNode
        {
            [XmlAttribute]
            public string FileName { get; set; }
        }

        public class SnippetNode
        {
            [XmlAttribute]
            public string Shortcut { get; set; }

            [XmlAttribute]
            public string Path { get; set; }
        }

        public class FileNode
        {
            [XmlAttribute]
            public string Path { get; set; }
        }

        public class VsTemplateNode
        {
        }
    }
}
