﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds
{
    public interface IBuildModelAware
    {
        BuildModel Model { get; }
    }
}
