// Guids.cs
// MUST match guids.h
using System;

namespace Neptuo.Productivity.VisualStudio
{
    public static class MyConstants
    {
        public const string PackageString = "6b896283-2b15-4f82-bb4e-186c94784f79";
        public const string CommandSetString = "e4a2c479-8fe3-4778-9dca-6a6f3ef5948c";
        
        public static readonly Guid CommandSetGuid = new Guid(CommandSetString);

        public static class CommandSet
        {
            public const int DuplicateLineDown = 0x101;
            public const int DuplicateLineUp = 0x102;
        }
    }
}