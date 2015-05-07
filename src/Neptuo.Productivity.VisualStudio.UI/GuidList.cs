﻿// Guids.cs
// MUST match guids.h
using System;

namespace Neptuo.Productivity.VisualStudio.UI
{
    static class GuidList
    {
        public const string PackageString = "99b35d3b-db21-443d-83d6-aba92f91491d";
        public const string CommandSetString = "37fa6823-584f-4b27-b4c4-f19ad81189ff";

        public static readonly Guid CommandSet = new Guid(CommandSetString);

        public static class CommandSet1
        {
            public const int Command1 = 0x100;
        }
    };
}