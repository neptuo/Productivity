using Neptuo.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Events
{
    public class BuildFinished : IBuildKeyAware
    {
        public Int32Key Key { get; private set; }
        public BuildModel Model { get; private set; }

        public BuildFinished(BuildModel model)
        {
            Ensure.NotNull(model, "model");
            Key = model.Key;
            Model = model;
        }
    }
}
