// Guids.cs
// MUST match guids.h
using System;

namespace Neptuo.Productivity.VisualStudio
{
    public static class MyConstants
    {
        public const string PackageString = "6b896283-2b15-4f82-bb4e-186c94784f79";
        public const string CommandSetString = "700fe17e-577b-401a-8ae3-7dae52e38a3e";
        
        public static readonly Guid CommandSetGuid = new Guid(CommandSetString);

        public static class CommandSet
        {
            public const int DuplicateLineDown = 0x101;
            public const int DuplicateLineUp = 0x102;
        }
    }
}