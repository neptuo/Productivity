using EnvDTE;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Productivity.Parsers;
using Neptuo.Productivity.Processors;
using Neptuo.Productivity.Processors.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    /// <summary>
    /// A service responsible of navigating to source.
    /// </summary>
    [Export(typeof(GoToSourceService))]
    public class GoToSourceService
    {
        [ImportMany]
        public IEnumerable<Lazy<IPathReader, IOrderable>> Readers { get; set; }

        private bool isReadersOrdered;

        private void EnsureReadersOrder()
        {
            if (isReadersOrdered)
                return;

            Readers = Orderer.Order(Readers);
            isReadersOrdered = true;
        }

        [ImportMany]
        public IEnumerable<Lazy<IPathProcessor, IOrderable>> Processors { get; set; }

        private bool isProcessorsOrdered;

        private void EnsureProcessorsOrder()
        {
            if (isProcessorsOrdered)
                return;

            Processors = Orderer.Order(Processors);
            isProcessorsOrdered = true;
        }

        [ImportMany]
        public IEnumerable<Lazy<IPathMapper, IOrderable>> Mappers { get; set; }

        private bool isMappersOrdered;

        private void EnsureMappersOrder()
        {
            if (isMappersOrdered)
                return;

            Mappers = Orderer.Order(Mappers);
            isMappersOrdered = true;
        }

        public bool TryRun(TextDocument textDocument)
        {
            EnsureReadersOrder();
            foreach (var reader in Readers)
            {
                if (reader.Value.TryRead(textDocument, out string path) && TryRun(Map(path)))
                    return true;
            }

            return false;
        }
        
        private string Map(string path)
        {
            EnsureMappersOrder();
            foreach (var mapper in Mappers)
                path = mapper.Value.Map(path);

            return path;
        }

        private bool TryRun(string path)
        {
            EnsureProcessorsOrder();
            foreach (var processor in Processors)
            {
                if (processor.Value.TryRun(path))
                    return true;
            }

            return false;
        }
    }
}
