using Neptuo.Productivity.Parsers;
using Neptuo.Productivity.Processors;
using Neptuo.Productivity.Processors.Mappers;
using System;
using System.Collections.Generic;
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
        public IEnumerable<IPathParser> Parsers { get; set; }

        [ImportMany]
        public IEnumerable<IPathProcessor> Processors { get; set; }

        [ImportMany]
        public IEnumerable<IPathMapper> Mappers { get; set; }

        public bool TryRun(string line, int index)
        {
            return TryParse(line, index, out string path) && TryRun(Map(path));
        }

        private bool TryParse(string line, int index, out string path)
        {
            foreach (IPathParser parser in Parsers)
            {
                if (parser.TryParse(line, index, out path))
                    return true;
            }

            path = null;
            return false;
        }

        private string Map(string path)
        {
            foreach (IPathMapper mapper in Mappers)
                path = mapper.Map(path);

            return path;
        }

        private bool TryRun(string path)
        {
            foreach (IPathProcessor processor in Processors)
            {
                if (processor.TryRun(path))
                    return true;
            }

            return false;
        }
    }
}
