using Neptuo.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds.Events
{
    public class ProjectBuildStarted
    {
        public ProjectKey Key { get; private set; }
        public string ProjectName { get; private set; }

        public ProjectBuildStarted(ProjectKey key, string projectName)
        {
            Ensure.Condition.NotNullOrEmpty(key, "key");
            Ensure.NotNullOrEmpty(projectName, "projectName");
            Key = key;
            ProjectName = projectName;
        }
    }
}
