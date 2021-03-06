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
using Neptuo;
using System.ComponentModel;

namespace Neptuo.Productivity.UI.ViewModels
{
    public class QuickMainViewModel : ObservableObject, IDisposable, IEventHandler<BuildStarted>, IEventHandler<BuildFinished>
    {
        private readonly IEventHandlerCollection events;
        private readonly IQuickConfiguration configuration;
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
        private long longestElapsedMilliseconds;

        public event Action<QuickMainViewModel, string> TitleChanged;

        public QuickMainViewModel(IEventHandlerCollection events, IQuickConfiguration configuration)
        {
            Ensure.NotNull(events, "events");
            Ensure.NotNull(configuration, "configuration");
            this.events = events;
            this.configuration = configuration;

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
            QuickBuildViewModel build = new QuickBuildViewModel(events, payload.Key, payload.Scope, payload.Action, payload.StartedAt);
            build.PropertyChanged += OnBuildPropertyChanged;
            Builds.Insert(0, build);
            buildCount++;

            while (Builds.Count > configuration.QuickOverviewCount)
                Builds.RemoveAt(configuration.QuickOverviewCount);

            UpdateTitle();
            return Task.FromResult(true);
        }

        private void OnBuildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(QuickBuildViewModel.ElapsedMilliseconds))
                UpdateRelativeDuration();
        }

        internal void UpdateRelativeDuration()
        {
            long? longestElapsedMilliseconds = Builds.Where(b => b.ElapsedMilliseconds != null).Max(b => b.ElapsedMilliseconds);
            if (longestElapsedMilliseconds != null)
            {
                if (longestElapsedMilliseconds.Value > this.longestElapsedMilliseconds)
                    this.longestElapsedMilliseconds = longestElapsedMilliseconds.Value;

                foreach (QuickBuildViewModel item in Builds)
                    item.UpdateRelativeDuration(this.longestElapsedMilliseconds);
            }
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
