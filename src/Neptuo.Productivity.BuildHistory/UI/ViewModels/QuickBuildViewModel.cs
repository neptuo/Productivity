using Neptuo.Models.Domains;
using Neptuo.Models.Keys;
using Neptuo.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neptuo.Events.Handlers;
using Neptuo.Observables;
using Neptuo.Productivity.Events;
using Neptuo;

namespace Neptuo.Productivity.UI.ViewModels
{
    public class QuickBuildViewModel : ObservableObject, IDisposable, IEventHandler<ProjectCountEstimated>, IEventHandler<ProjectBuildFinished>, IEventHandler<BuildFinished>
    {
        private readonly IEventHandlerCollection events;
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

        private BuildType action;
        public BuildType Action
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

        private bool isSuccessful;
        public bool IsSuccessful
        {
            get { return isSuccessful; }
            set
            {
                if (isSuccessful != value)
                {
                    isSuccessful = value;
                    RaisePropertyChanged();
                }
            }
        }

        public QuickBuildViewModel(IEventHandlerCollection events, Int32Key buildKey, BuildScope scope, BuildType action, DateTime? startedAt)
        {
            Ensure.NotNull(events, "events");
            Ensure.Condition.NotEmptyKey(buildKey);
            this.events = events;
            this.buildKey = buildKey;
            Scope = scope;
            Action = action;
            StartedAt = startedAt;

            events.Add<ProjectCountEstimated>(this);
            events.Add<BuildFinished>(this);
            events.Add<ProjectBuildFinished>(this);
        }

        public Task HandleAsync(ProjectCountEstimated payload)
        {
            if (payload.Key == buildKey)
            {
                ProjectCount = payload.EstimatedProjectCount;
                PrepareDescription();
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
                    PrepareDescription();
                }
            }

            return Task.FromResult(true);
        }

        public Task HandleAsync(BuildFinished payload)
        {
            if (buildKey == payload.Key)
            {
                ElapsedMilliseconds = payload.Model.ElapsedMilliseconds;
                PrepareDescription();

                IsSuccessful = payload.Model.Projects
                    .Where(p => p.IsSuccessful != null)
                    .Select(p => p.IsSuccessful.Value)
                    .All(p => p);
            }

            return Task.FromResult(true);
        }

        public void PrepareDescription()
        {
            long? lengthValue = ElapsedMilliseconds;
            if (lengthValue == null)
            {
                if (ProjectCount == null)
                    Description = String.Format("Building {0} of...", BuiltProjectCount + 1);
                else
                    Description = String.Format("Building {0} of {1}...", BuiltProjectCount + 1, ProjectCount);
            }

            Description = buildTimeFormatter.Format(lengthValue.Value);
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
            events.Remove<ProjectCountEstimated>(this);
            events.Remove<BuildFinished>(this);
            events.Remove<ProjectBuildFinished>(this);
        }
    }
}
