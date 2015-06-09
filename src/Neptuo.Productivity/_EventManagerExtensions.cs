using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Pipelines.Events
{
    public static class _EventManagerExtensions
    {
        public static void Publish<T>(this IEventDispatcher eventDispatcher, T payload)
        {
            Ensure.NotNull(eventDispatcher, "eventDispatcher");
            eventDispatcher.PublishAsync(payload).RunSynchronously();
        }
    }
}
