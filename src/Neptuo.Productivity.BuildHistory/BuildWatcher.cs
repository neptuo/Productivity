﻿using Neptuo.Activators;
using Neptuo.Observables.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class BuildWatcher
    {
        private readonly IFactory<BuildModel, BuildModelActivatorContext> buildFactory;

        public ObservableCollection<BuildModel> History { get; private set; }

        public BuildWatcher(IFactory<BuildModel, BuildModelActivatorContext> buildFactory)
        {
            Ensure.NotNull(buildFactory, "buildFactory");
            this.buildFactory = buildFactory;
            History = new ObservableCollection<BuildModel>();
        }

        public BuildProgress StartNew(BuildScope scope, BuildType type)
        {
            BuildModel model = buildFactory.Create(new BuildModelActivatorContext(scope, type));
            BuildProgress result = new BuildProgress(model);

            History.Insert(0, result.Model);
            return result;
        }
    }
}
