﻿using Neptuo.Observables;
using Neptuo.Productivity.VisualStudio.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neptuo.Productivity.VisualStudio.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (path != value)
                {
                    path = value;
                    RaisePropertyChanged();
                    RecomputeActivePath();
                }
            }
        }

        private string projectPath;
        public string ProjectPath
        {
            get { return projectPath; }
            set
            {
                if (projectPath != value)
                {
                    projectPath = value;
                    RaisePropertyChanged();
                    RecomputeActivePath();
                }
            }
        }

        private string inactivePath;
        public string InactivePath
        {
            get { return inactivePath; }
            private set
            {
                if (inactivePath != value)
                {
                    inactivePath = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string activePath;
        public string ActivePath
        {
            get { return activePath; }
            private set
            {
                if (activePath != value)
                {
                    activePath = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged();
                    RecomputeActivePath();
                }
            }
        }

        private bool isFile;
        public bool IsFile
        {
            get { return isFile; }
            set
            {
                if (isFile != value)
                {
                    isFile = value;
                    RaisePropertyChanged();
                }
            }
        }

        public AddNewItemCommand Add { get; private set; }

        public MainViewModel(IFileService files)
        {
            IsFile = true;
            Add = new AddNewItemCommand(this, files);
        }

        private void RecomputeActivePath()
        {
            string name = Name;
            string activePath = Path;
            string inactivePath = String.Empty;

            if (name == null)
                name = String.Empty;

            if (!String.IsNullOrEmpty(activePath))
            {
                if (!TryEvaluate(out var newItem))
                    return;

                activePath = newItem.path;
                name = newItem.name;

                if (activePath.EndsWith("/") || activePath.EndsWith(@"\"))
                    activePath = activePath.Substring(0, activePath.Length - 1);

                if (activePath.Length > newItem.path.Length)
                    inactivePath = activePath.Substring(newItem.path.Length);

                while (name.StartsWith("../") || name.StartsWith(@"..\"))
                {
                    name = name.Substring(3);

                    int indexOfSlash = activePath.LastIndexOf('/');
                    if (indexOfSlash == -1)
                        indexOfSlash = activePath.LastIndexOf('\\');

                    if (indexOfSlash >= 0)
                    {
                        inactivePath = activePath.Substring(indexOfSlash) + inactivePath;
                        activePath = activePath.Substring(0, indexOfSlash);
                    }
                    else if (activePath.Length > 0)
                    {
                        inactivePath = activePath + inactivePath;
                        activePath = String.Empty;
                    }
                    else
                    {
                        inactivePath = "../" + inactivePath;
                    }
                }

                if (activePath.Length > 0 && !activePath.EndsWith("/") && !activePath.EndsWith(@"\"))
                    activePath += "/";

                if (inactivePath.StartsWith("/") || inactivePath.StartsWith(@"\"))
                    inactivePath = inactivePath.Substring(1);

                if (inactivePath.Length > 0 && !inactivePath.EndsWith("/") && !inactivePath.EndsWith(@"\"))
                    inactivePath += "/";
            }

            IsFile = !(Name ?? String.Empty).EndsWith("/") && !(Name ?? String.Empty).EndsWith(@"\");
            ActivePath = activePath.Replace(@"\", "/");
            InactivePath = inactivePath.Replace(@"\", "/");
        }

        public void SetPaths(string path, string projectPath)
        {
            this.path = path;
            this.projectPath = projectPath;
            RecomputeActivePath();
        }

        public bool TryEvaluate(out (string path, string name) result)
        {
            result = default;

            string name = Name;
            string path = Path;

            if (name == null)
                name = String.Empty;

            if (name.StartsWith("~/"))
            {
                if (String.IsNullOrEmpty(ProjectPath))
                    return false;

                name = name.Substring(2);
                path = ProjectPath;
            }

            result = (path, name);
            return true;
        }
    }
}
