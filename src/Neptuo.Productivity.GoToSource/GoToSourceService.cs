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
    [Export(typeof(GoToSourceService))]
    public class GoToSourceService
    {
        [ImportMany]
        public IEnumerable<Lazy<IPathParser, IOrderable>> Parsers { get; set; }

        private bool isParsersOrdered;

        private void EnsureParsersOrder()
        {
            if (isParsersOrdered)
                return;

            Parsers = Orderer.Order(Parsers);
            isParsersOrdered = true;
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

        public bool TryRun(string line, int index)
        {
            return TryParse(line, index, out string path) && TryRun(Map(path));
        }

        private bool TryParse(string line, int index, out string path)
        {
            EnsureParsersOrder();
            foreach (var parser in Parsers)
            {
                if (parser.Value.TryParse(line, index, out path))
                    return true;
            }

            path = null;
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
