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
        }
    }
}