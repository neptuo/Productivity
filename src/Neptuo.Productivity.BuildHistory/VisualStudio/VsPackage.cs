﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Events;
using Neptuo.Productivity.UI.ViewModels;
using Neptuo.Productivity.UI.Views;
using Neptuo.Productivity.VisualStudio.Commands;
using Neptuo.Productivity.VisualStudio.Options;
using Neptuo.Productivity.VisualStudio.UI;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Neptuo.Productivity.VisualStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration(ProductInfo.Name, ProductInfo.Description, VersionInfo.Version, IconResourceID = 400)]
    [Guid(PackageGuids.PackageString)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(QuickWindow))]
    [ProvideOptionPage(typeof(ConfigurationPage), "Neptuo Productivity", "Build History", 0, 0, true)]
    public sealed partial class VsPackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();

            DefaultEventManager eventManager = new DefaultEventManager();
            DTE dte = (DTE)GetService(typeof(DTE));
            IMenuCommandService commandService = (IMenuCommandService)GetService(typeof(IMenuCommandService));

            ServiceFactory.EventHandlers = eventManager;
            ServiceFactory.QuickConfiguration = (IQuickConfiguration)GetDialogPage(typeof(ConfigurationPage));

            OverviewCommand.Initialize(this, dte, commandService);
            BuildService.Initialize(dte, eventManager);
        }
    }
}