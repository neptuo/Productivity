﻿using Microsoft.VisualStudio.Shell;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Neptuo.Productivity.VisualStudio.Views
{
    public class AddNewItemWindow : Window
    {
        public MainViewModel ViewModel => (MainViewModel)DataContext;

        public AddNewItemWindow(MainViewModel viewModel)
        {
            Ensure.NotNull(viewModel, "viewModel");

            Title = "Add new item...";
            Content = new AddNewItemView();
            DataContext = viewModel;
            SizeToContent = SizeToContent.WidthAndHeight;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            SetResourceReference(BackgroundProperty, VsBrushes.BackgroundKey);

            ViewModel.Add.Executed += OnAddExecuted;
        }

        private void OnAddExecuted()
        {
            ViewModel.Name = null;
            Close();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape)
            {
                if (String.IsNullOrEmpty(ViewModel.Name))
                    Close();
                else
                    ViewModel.Name = null;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            ViewModel.Add.Executed -= OnAddExecuted;
        }
    }
}
