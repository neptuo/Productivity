﻿using Neptuo.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds.Events
{
    public class ProjectCountEstimated : IBuildKeyAware
    {
        public Int32Key Key { get; private set; }
        public int EstimatedProjectCount { get; private set; }

        public ProjectCountEstimated(Int32Key key, int estimatedProjectCount)
        {
            Ensure.Condition.NotNullOrEmpty(key, "key");
            Ensure.Positive(estimatedProjectCount, "estimatedProjectCount");
            Key = key;
            EstimatedProjectCount = estimatedProjectCount;
        }
    }
}
