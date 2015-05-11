using Microsoft.VisualStudio.Language.Intellisense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense.Completions
{
    public interface ICSharpStringCompletionProvider
    {
        IList<Completion> GetCompletionList(string textValue, string contextName);
    }
}
