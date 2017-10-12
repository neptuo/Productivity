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
        private readonly IFileService files;
        private readonly ITemplateService templates;

        public AddNewItemCommand(MainViewModel viewModel, IFileService files, ITemplateService templates)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Ensure.NotNull(files, "service");
            Ensure.NotNull(templates, "templates");
            this.viewModel = viewModel;
            this.files = files;
            this.templates = templates;

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
                if (!files.IsValidFileName(viewModel.Name))
                    return false;

                if (files.FileExists(path))
                    return false;
            }
            else
            {
                if (!files.IsValidDirectoryName(viewModel.Name))
                    return false;

                if (files.DirectoryExists(path))
                    return false;
            }

            return true;
        }

        public override void Execute()
        {
            if (CanExecute())
            {
                string path = Path.Combine(viewModel.Path, viewModel.Name);

                if (viewModel.IsFile)
                {
                    ITemplate template = templates.GetTemplate(path);
                    files.CreateFile(path, template);
                }
                else
                {
                    files.CreateDirectory(path);
                }
            }
        }
    }
}
