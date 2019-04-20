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
using System.Diagnostics;

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
                if (projectCount != value)
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

        private string elapsed;
        public string Elapsed
        {
            get { return elapsed; }
            set
            {
                if (elapsed != value)
                {
                    elapsed = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string buildState;
        public string BuildState
        {
            get { return buildState; }
            set
            {
                if (buildState != value)
                {
                    buildState = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool? isSuccessful;
        public bool? IsSuccessful
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

            _ = UpdateCurrentElapsedAsync();
        }

        private async Task UpdateCurrentElapsedAsync()
        {
            if (IsSuccessful != null)
                return;

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (IsSuccessful == null)
            {
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                UpdateElapsed();

                await Task.Delay(1000);
            }
        }

        public Task HandleAsync(ProjectCountEstimated payload)
        {
            if (payload.Key == buildKey)
            {
                ProjectCount = payload.EstimatedProjectCount;
                UpdateBuildState();
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
                    UpdateBuildState();
                }
            }

            return Task.FromResult(true);
        }

        public Task HandleAsync(BuildFinished payload)
        {
            if (buildKey == payload.Key)
            {
                IsSuccessful = payload.Model.Projects
                    .Where(p => p.IsSuccessful != null)
                    .Select(p => p.IsSuccessful.Value)
                    .All(p => p);

                ElapsedMilliseconds = payload.Model.ElapsedMilliseconds;

                UpdateElapsed();
                UpdateBuildState();
            }

            return Task.FromResult(true);
        }

        internal void UpdateElapsed()
        {
            long? lengthValue = ElapsedMilliseconds;
            Elapsed = buildTimeFormatter.Format(lengthValue.Value);
        }

        internal void UpdateBuildState()
        {
            if (IsSuccessful == null)
            {
                if (ProjectCount == null)
                    BuildState = String.Format("({0}/..)", BuiltProjectCount + 1);
                else
                    BuildState = String.Format("({0}/{1})", BuiltProjectCount + 1, ProjectCount);
            }
            else
            {
                BuildState = null;
            }
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
