// Guids.cs
// MUST match guids.h
using System;

namespace OregonStateUniversity.SlackCheckIn
{
    static class GuidList
    {
        public const string guidSlackCheckInPkgString = "92826d9f-e6c6-4528-8d55-4169832c9577";
        public const string guidSlackCheckInCmdSetString = "309f37b9-d361-4343-a0fa-26127b0c4d17";
        public static readonly Guid guidSlackCheckInNotification = new Guid("3e774394-068d-40dd-8b34-bc09e8947b8b");
        public static readonly Guid guidSlackCheckInCmdSet = new Guid(guidSlackCheckInCmdSetString);
    };
}