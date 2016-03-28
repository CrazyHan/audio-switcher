// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Audio
{
    [Flags]
    public enum AudioDeviceDefaultStatuses
    {
        None = 0,
        Multimedia = 1,
        Communications = 2,
        All = Multimedia | Communications,
    }

    public static class AudioDeviceDefaultStatusesExtensions
	{
        public static bool IsSet(this AudioDeviceDefaultStatuses state, AudioDeviceDefaultStatuses flag)
        {
            return (state & flag) == flag;
        }
    }
}
