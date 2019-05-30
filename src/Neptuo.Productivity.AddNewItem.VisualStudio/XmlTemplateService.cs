using Neptuo.FileSystems;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Neptuo.Productivity
{
    public class XmlTemplateService : ITemplateService
    {
        private readonly string directoryPath;
        private readonly TemplateRoot root;

        public XmlTemplateService(string sourcePath)
        {
            Ensure.Condition.FileExists(sourcePath, "sourcePath");

            directoryPath = Path.GetDirectoryName(sourcePath);

            try
            {
                HashSet<string> processedPaths = new HashSet<string>();
                processedPaths.Add(sourcePath);
                root = LoadFile(sourcePath, processedPaths);
            }
            catch (Exception e)
            {
                throw new XmlTemplateException(e, sourcePath);
            }
        }

        private TemplateRoot LoadFile(string sourcePath, HashSet<string> processedPaths)
        {
            TemplateRoot root;
            XmlSerializer serializer = new XmlSerializer(typeof(TemplateRoot));
            using (FileStream sourceContent = new FileStream(sourcePath, FileMode.Open))
                root = (TemplateRoot)serializer.Deserialize(sourceContent);

            if (root.Includes != null)
            {
                foreach (IncludeNode include in root.Includes)
                {
                    string includePath = Path.Combine(Path.GetDirectoryName(sourcePath), include.Path);
                    includePath = Path.GetFullPath(includePath);
                    includePath = includePath.ToLowerInvariant();

                    if (processedPaths.Add(includePath))
                    {
                        TemplateRoot includeRoot = LoadFile(includePath, processedPaths);
                        if (includeRoot != null)
                        {
                            root.Templates.AddRange(includeRoot.Templates);
                        }
                    }
                }
            }

            return root;
        }

        public bool IsStandalone => root.IsStandalone;

        public ITemplate FindTemplate(string path)
        {
            string fileName = Path.GetFileName(path);
            foreach (TemplateNode templateNode in root.Templates)
            {
                foreach (SelectorNode selectorNode in templateNode.Selectors)
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
                return new StringTemplate(EncodeXmlLines(node));

            return null;
        }

        private string EncodeXmlLines(TemplateNode node) => node.Content.Replace("\n", "\r\n");

        #region Xml Nodes

        [XmlRoot("Templates", Namespace = "http://schemas.neptuo.com/xsd/productivity/vsix/AddNewItem.xsd")]
        public class TemplateRoot
        {
            [XmlAttribute]
            public bool IsStandalone { get; set; }

            [XmlElement("Template")]
            public List<TemplateNode> Templates { get; set; }

            [XmlElement("Include")]
            public List<IncludeNode> Includes { get; set; }
        }

        [XmlType("Include")]
        public class IncludeNode
        {
            [XmlAttribute]
            public string Path { get; set; }
        }

        [XmlType("Template")]
        [XmlRoot("Template")]
        public class TemplateNode
        {
            [XmlElement("Selector")]
            public List<SelectorNode> Selectors { get; set; }

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

            [XmlAttribute]
            public MatchSyntaxMode FileNameSyntax { get; set; }

            public bool IsMatched(string fileName)
            {
                if (regex == null)
                {
                    switch (FileNameSyntax)
                    {
                        case MatchSyntaxMode.Glob:
                            regex = new Regex("^" + FileName.Replace("*", "(.*)") + "$");
                            break;
                        case MatchSyntaxMode.Regex:
                            regex = new Regex(FileName);
                            break;
                        default:
                            throw Ensure.Exception.NotSupported($"Not supported value '{FileNameSyntax}' for '{nameof(FileNameSyntax)}'.");
                    }
                }

                return regex.IsMatch(fileName);
            }
        }

        public enum MatchSyntaxMode
        {
            Glob,
            Regex
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
