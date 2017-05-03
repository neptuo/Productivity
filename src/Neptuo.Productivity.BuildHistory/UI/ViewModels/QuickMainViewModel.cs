﻿using Neptuo.Observables.Collections;
using Neptuo.Models.Domains;
using Neptuo.Events;
using Neptuo.Events.Handlers;
using Neptuo.Observables;
using Neptuo.Productivity.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.UI.ViewModels
{
    public class QuickMainViewModel : ObservableObject, IDisposable, IEventHandler<BuildStarted>, IEventHandler<BuildFinished>
    {
        private readonly IEventHandlerCollection events;
        private readonly BuildTimeFormatter buildTimeFormatter = new BuildTimeFormatter();

        public ObservableCollection<QuickBuildViewModel> Builds { get; private set; }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int buildCount;
        private long totalElapsedMilliseconds;

        public event Action<QuickMainViewModel, string> TitleChanged;

        public QuickMainViewModel(IEventHandlerCollection events)
        {
            Ensure.NotNull(events, "events");
            this.events = events;

            Builds = new ObservableCollection<QuickBuildViewModel>();
            Title = String.Format("Build History");

            events.Add<BuildStarted>(this);
            events.Add<BuildFinished>(this);
        }

        private void UpdateTitle()
        {
            if (totalElapsedMilliseconds == 0)
                Title = String.Format("Build History ({0})", buildCount);
            else
                Title = String.Format("Build History ({0}):   {1}", buildCount, buildTimeFormatter.Format(totalElapsedMilliseconds));

            if (TitleChanged != null)
                TitleChanged(this, Title);
        }

        Task IEventHandler<BuildStarted>.HandleAsync(BuildStarted payload)
        {
            Builds.Insert(0, new QuickBuildViewModel(events, payload.Key, payload.Scope, payload.Action, payload.StartedAt));
            buildCount++;

            // TODO: Uncomment and implement.
            //while (Builds.Count > configuration.BuildHistoryOverviewCount)
            //    Builds.RemoveAt(configuration.BuildHistoryOverviewCount);

            UpdateTitle();
            return Task.FromResult(true);
        }

        Task IEventHandler<BuildFinished>.HandleAsync(BuildFinished payload)
        {
            if (payload.Model.ElapsedMilliseconds != null)
            {
                totalElapsedMilliseconds += payload.Model.ElapsedMilliseconds.Value;
                UpdateTitle();
            }

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
            events.Remove<BuildStarted>(this);
            events.Remove<BuildFinished>(this);
        }

        #endregion
    }
}
