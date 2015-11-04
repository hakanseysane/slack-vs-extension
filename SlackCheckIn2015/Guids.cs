// Guids.cs
// MUST match guids.h
using System;

namespace OregonStateUniversity.SlackCheckIn
{
    static class GuidList
    {
        public const string guidSlackCheckInPkgString = "92826d9f-e6c6-4528-8d55-4169832c9578";
        public const string guidSlackCheckInCmdSetString = "309f37b9-d361-4343-a0fa-26127b0c4d18";

        public static readonly Guid guidSlackCheckInCmdSet = new Guid(guidSlackCheckInCmdSetString);
    };
}