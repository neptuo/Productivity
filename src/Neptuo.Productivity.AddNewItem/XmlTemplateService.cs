﻿using Neptuo.FileSystems;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Neptuo.Productivity
{
    public class XmlTemplateService : ITemplateService
    {
        private readonly string directoryPath;
        private readonly TemplateList list;

        public XmlTemplateService(string sourcePath)
        {
            Ensure.Condition.FileExists(sourcePath, "sourcePath");

            directoryPath = Path.GetDirectoryName(sourcePath);

            XmlSerializer serializer = new XmlSerializer(typeof(TemplateList));
            using (FileStream sourceContent = new FileStream(sourcePath, FileMode.Open))
                list = (TemplateList)serializer.Deserialize(sourceContent);
        }

        public ITemplate FindTemplate(string path)
        {
            string fileName = Path.GetFileName(path);
            foreach (TemplateNode templateNode in list)
            {
                foreach (SelectorNode selectorNode in templateNode.Selector)
                {
                    if (selectorNode.IsMatched(fileName))
                        return CreateTemplate(templateNode);
                }
            }

            return null;
        }

        private ITemplate CreateTemplate(TemplateNode node)
        {
            if (node.File != null)
                return new FileTemplate(Path.Combine(directoryPath, node.File.Path));

            if (node.Content != null)
                return new StringTemplate(node.Content);

            return null;
        }

        #region Xml Nodes

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
            private Regex regex;

            [XmlAttribute]
            public string FileName { get; set; }

            public bool IsMatched(string fileName)
            {
                if (regex == null)
                    regex = new Regex("^" + FileName.Replace("*", "(.*)") + "$");

                return regex.IsMatch(fileName);
            }
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

        #endregion
    }
}
