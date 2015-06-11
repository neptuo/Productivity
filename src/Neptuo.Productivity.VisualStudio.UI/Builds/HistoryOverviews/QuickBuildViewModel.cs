using Neptuo.ComponentModel;
using Neptuo.DomainModels;
using Neptuo.Pipelines.Events;
using Neptuo.Pipelines.Events.Handlers;
using Neptuo.Productivity.Builds;
using Neptuo.Productivity.Builds.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.UI.Builds.HistoryOverviews
{
    public class QuickBuildViewModel : ObservableObject, IDisposable, IEventHandler<BuildFinished>
    {
        private readonly IEventRegistry events;
        private readonly Int32Key buildKey;

        private BuildScope scope;
        public BuildScope Scope
        {
            get { return scope; }
            set
            {
                if (scope != value)
                {
                    scope = value;
                    RaisePropertyChanged();
                }
            }
        }

        private BuildAction action;
        public BuildAction Action
        {
            get { return action; }
            set
            {
                if (action != value)
                {
                    action = value;
                    RaisePropertyChanged();
                }
            }
        }

        private DateTime? startedAt;
        public DateTime? StartedAt
        {
            get { return startedAt; }
            set
            {
                if (startedAt != value)
                {
                    startedAt = value;
                    RaisePropertyChanged();
                }
            }
        }

        private long? elapsedMilliseconds;
        public long? ElapsedMilliseconds
        {
            get { return elapsedMilliseconds; }
            set
            {
                if (elapsedMilliseconds != value)
                {
                    elapsedMilliseconds = value;
                    RaisePropertyChanged();
                }
            }
        }

        public QuickBuildViewModel(IEventRegistry events, Int32Key buildKey, BuildScope scope, BuildAction action, DateTime? startedAt)
        {
            Ensure.NotNull(events, "events");
            Ensure.Condition.NotNullOrEmpty(buildKey, "buildKey");
            this.events = events;
            this.buildKey = buildKey;
            Scope = scope;
            Action = action;
            StartedAt = startedAt;

            events.Subscribe((IEventHandler<BuildFinished>)this);
        }

        public Task HandleAsync(BuildFinished payload)
        {
            if (buildKey == payload.Key)
                ElapsedMilliseconds = payload.Model.ElapsedMilliseconds;

            return Task.FromResult(true);
        }

        #region IDisposable

        private bool isDisposed;

        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;
            DisposeManagedResources();
        }

        protected void DisposeManagedResources()
        {
            events.UnSubscribe((IEventHandler<BuildFinished>)this);
        }

        #endregion
    }
}
