using Neptuo.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class ProjectKey : KeyBase
    {
        public Int32Key BuildKey { get; private set; }
        public string ProjectName { get; private set; }

        public static ProjectKey Empty(string type)
        {
            return new ProjectKey(type);
        }

        public static ProjectKey Create(Int32Key buildKey, string projectName, string type)
        {
            return new ProjectKey(buildKey, projectName, type);
        }

        private ProjectKey(string type)
            : base(type, true)
        { }

        private ProjectKey(Int32Key buildKey, string projectName, string type)
            : base(type, false)
        {
            Ensure.Condition.NotEmptyKey(buildKey);
            Ensure.NotNullOrEmpty(projectName, "projectName");
            BuildKey = buildKey;
            ProjectName = projectName;
        }

        protected override int CompareValueTo(KeyBase other)
        {
            ProjectKey otherKey = other as ProjectKey;
            if (otherKey == null)
                return -1;

            int isEmptyCompare = otherKey.IsEmpty.CompareTo(IsEmpty);
            if (isEmptyCompare != 0)
                return isEmptyCompare;

            int buildCompare = otherKey.BuildKey.CompareTo(BuildKey);
            if (buildCompare != 0)
                return buildCompare;

            return otherKey.ProjectName.CompareTo(ProjectName);
        }

        protected override bool Equals(KeyBase other)
        {
            ProjectKey otherKey = other as ProjectKey;
            if (otherKey == null)
                return false;

            if (otherKey.IsEmpty != IsEmpty)
                return false;

            if (otherKey.BuildKey != BuildKey)
                return false;

            if (otherKey.ProjectName != ProjectName)
                return false;

            return true;
        }

        protected override int GetValueHashCode()
        {
            int value = 3;
            value ^= BuildKey.GetHashCode();

            if (ProjectName != null)
                value ^= ProjectName.GetHashCode();

            return value;
        }

        protected override string ToStringValue()
        {
            return ProjectName + "+" + BuildKey.ToString();
        }
    }
}
