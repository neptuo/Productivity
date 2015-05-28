using Neptuo.Collections.ObjectModel;
using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds
{
    public class BuildModel : ObservableObject
    {
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
                if(action != value)
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

        private DateTime? finishedAt;
        public DateTime? FinishedAt
        {
            get { return finishedAt; }
            set
            {
                if (finishedAt != value)
                {
                    finishedAt = value;
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

        private int allProjectsCount;
        public int AllProjectsCount
        {
            get { return allProjectsCount; }
            set
            {
                if (allProjectsCount != value)
                {
                    allProjectsCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<BuildProjectModel> Projects { get; private set; }

        public BuildModel(BuildScope scope, BuildAction action)
        {
            Scope = scope;
            Action = action;
            Projects = new ObservableCollection<BuildProjectModel>();
        }

        public BuildModel(BuildScope scope, BuildAction action, DateTime startedAt)
            : this(scope, action)
        {
            StartedAt = startedAt;
        }
    }
}
