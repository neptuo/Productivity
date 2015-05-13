// Guids.cs
// MUST match guids.h
using System;

namespace Neptuo.Productivity.VisualStudio
{
    public static class MyConstants
    {
        public const string PackageString = "99b35d3b-db21-443d-83d6-aba92f91491d";
        public const string CommandSetString = "37fa6823-584f-4b27-b4c4-f19ad81189ff";

        public static readonly Guid CommandSetGuid = new Guid(CommandSetString);

        public static class CommandSet
        {
            public const int UnderscoreRemover = 0x100;
            public const int DuplicateLineDown = 0x101;
            public const int DuplicateLineUp = 0x102;
        }

        public static class Feature
        {
            public const string MainCategory = "Neptuo Productivity";
            public const string GeneralPage = "General";
            public const string FriendlyNamespaces = "C# Friendly namespaces";
        }
    }
}