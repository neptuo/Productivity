using Neptuo;
using Neptuo.Observables.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using Neptuo.Collections.Specialized;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;

namespace Neptuo.Productivity.VisualStudio.ViewModels.Commands
{
    public class AddNewItemCommand : Command
    {
        private readonly MainViewModel viewModel;
        private readonly IFileService files;
        private readonly ITemplateService templates;
        private readonly ICursorService cursor;

        public event Action Executed;

        public AddNewItemCommand(MainViewModel viewModel, IFileService files, ITemplateService templates, ICursorService cursor)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Ensure.NotNull(files, "service");
            Ensure.NotNull(templates, "templates");
            Ensure.NotNull(cursor, "cursor");
            this.viewModel = viewModel;
            this.files = files;
            this.templates = templates;
            this.cursor = cursor;

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
                    ITemplate template = templates.FindTemplate(path) ?? EmptyTemplate.Instance;
                    if (template is IContentTemplate contentTemplate)
                    {
                        // TODO: Parameters.
                        TemplateContent templateContent = contentTemplate.GetContent(new KeyValueCollection());

                        files.CreateFile(path, templateContent.Encoding, templateContent.Content);

                        if (templateContent.Position > 0)
                            cursor.Move(path, templateContent.Position);
                    }
                    //else if (template is IApplicableTemplate applicableTemplate)
                    //{
                    //    applicableTemplate.ApplyAsync(path, parameters);
                    //}
                    else
                        throw Ensure.Exception.NotImplemented();
                }
                else
                {
                    files.CreateDirectory(path);
                }

                Executed?.Invoke();
            }
        }
    }
}
