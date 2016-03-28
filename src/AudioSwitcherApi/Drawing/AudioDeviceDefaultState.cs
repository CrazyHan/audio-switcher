// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Drawing
{
    [Flags]
    public enum AudioDeviceDefaultState
    {
        None = 0,
        Multimedia = 1,
        Communications = 2,
        All = Multimedia | Communications,
    }

    public static class AudioDeviceDefaultStateExtensions
    {
        public static bool IsSet(this AudioDeviceDefaultState state, AudioDeviceDefaultState flag)
        {
            return (state & flag) == flag;
        }
    }
}
