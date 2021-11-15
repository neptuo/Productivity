using Neptuo.Observables.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.ViewModels.Commands
{
    public class AddNewItemCommand : Command
    {
        private readonly MainViewModel viewModel;
        private readonly IFileService files;

        public event Action<bool> Executed;

        public AddNewItemCommand(MainViewModel viewModel, IFileService files)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Ensure.NotNull(files, "service");
            this.viewModel = viewModel;
            this.files = files;

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.Path) || e.PropertyName == nameof(viewModel.Name) || e.PropertyName == nameof(viewModel.IsFile))
                RaiseCanExecuteChanged();
        }

        public override bool CanExecute()
        {
            if (String.IsNullOrEmpty(viewModel.Name))
                return false;

            if (String.IsNullOrEmpty(viewModel.Path))
                return false;

            if (!viewModel.TryEvaluate(out var newItem))
                return false;

            (string path, string name) = newItem;

            path = Path.Combine(viewModel.Path, name);
            if (viewModel.IsFile)
            {
                if (!files.IsValidFileName(name))
                    return false;

                if (files.FileExists(path))
                    return false;
            }
            else
            {
                if (!files.IsValidDirectoryName(name))
                    return false;

                if (files.DirectoryExists(path))
                    return false;
            }

            return true;
        }

        public override void Execute()
        {
            if (CanExecute())
                Executed?.Invoke(true);
        }
    }
}
