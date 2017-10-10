using Neptuo;
using Neptuo.Observables.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace Neptuo.Productivity.VisualStudio.ViewModels.Commands
{
    public class AddNewItemCommand : Command
    {
        private readonly MainViewModel viewModel;
        private readonly IFileService service;

        public AddNewItemCommand(MainViewModel viewModel, IFileService service)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Ensure.NotNull(service, "service");
            this.viewModel = viewModel;
            this.service = service;

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.Path) || e.PropertyName == nameof(viewModel.Name))
                RaiseCanExecuteChanged();
        }

        public override bool CanExecute()
        {
            if (String.IsNullOrEmpty(viewModel.Name))
                return false;

            if (String.IsNullOrEmpty(viewModel.Path))
                return false;

            string path = Path.Combine(viewModel.Path, viewModel.Name);
            if (viewModel.IsFile)
            {
                if (!service.IsValidFileName(viewModel.Name))
                    return false;

                if (service.FileExists(path))
                    return false;
            }
            else
            {
                if (!service.IsValidDirectoryName(viewModel.Name))
                    return false;

                if (service.DirectoryExists(path))
                    return false;
            }

            return true;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
