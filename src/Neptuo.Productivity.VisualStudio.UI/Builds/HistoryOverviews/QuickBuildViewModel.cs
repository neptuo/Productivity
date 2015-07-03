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
    public class QuickBuildViewModel : ObservableObject, IDisposable, IEventHandler<ProjectCountEstimated>, IEventHandler<ProjectBuildFinished>, IEventHandler<BuildFinished>
    {
        private readonly IEventRegistry events;
        private readonly Int32Key buildKey;
        private readonly BuildTimeFormatter buildTimeFormatter = new BuildTimeFormatter();
        private readonly HashSet<string> builtProjectNames = new HashSet<string>();

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

        private int? projectCount;
        public int? ProjectCount
        {
            get { return projectCount; }
            set
            {
                if(projectCount != value)
                {
                    projectCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int builtProjectCount;
        public int BuiltProjectCount
        {
            get { return builtProjectCount; }
            set
            {
                if (builtProjectCount != value)
                {
                    builtProjectCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
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

            events.Subscribe<ProjectCountEstimated>(this);
            events.Subscribe<BuildFinished>(this);
            events.Subscribe<ProjectBuildFinished>(this);
        }

        public Task HandleAsync(ProjectCountEstimated payload)
        {
            if (payload.Key == buildKey)
            {
                ProjectCount = payload.EstimatedProjectCount;
                Description = PrepareDescription();
            }

            return Task.FromResult(true);
        }

        public Task HandleAsync(ProjectBuildFinished payload)
        {
            if (payload.Key.BuildKey == buildKey)
            {
                if (builtProjectNames.Add(payload.Model.Name))
                {
                    BuiltProjectCount++;
                    Description = PrepareDescription();
                }
            }

            return Task.FromResult(true);
        }

        public Task HandleAsync(BuildFinished payload)
        {
            if (buildKey == payload.Key)
            {
                ElapsedMilliseconds = payload.Model.ElapsedMilliseconds;
                Description = PrepareDescription();
            }

            return Task.FromResult(true);
        }

        private string PrepareDescription()
        {
            long? lengthValue = ElapsedMilliseconds;
            if (lengthValue == null)
            {
                if (ProjectCount == null)
                    return String.Format("Building {0} of...", BuiltProjectCount + 1);
                else
                    return String.Format("Building {0} of {1}...", BuiltProjectCount + 1, ProjectCount);
            }

            return buildTimeFormatter.Format(lengthValue.Value);
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

        #endregion

        protected void DisposeManagedResources()
        {
            events.UnSubscribe<ProjectCountEstimated>(this);
            events.UnSubscribe<BuildFinished>(this);
            events.UnSubscribe<ProjectBuildFinished>(this);
        }
    }
}
