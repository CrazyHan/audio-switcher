// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.UI;
using AudioSwitcher.Drawing;

namespace AudioSwitcher.UI.ViewModels
{
    internal class AudioDeviceViewModel
    {
        private readonly AudioDevice _device;

        public AudioDeviceViewModel(AudioDevice device)
        {
            _device = device;
        }

        public AudioDevice Device
        {
            get { return _device; }
        }

        public AudioDeviceDefaultStatuses DefaultState
        {
            get;
            private set;
        }

        public AudioDeviceState State
        {
            get;
            private set;
        }

        public AudioDeviceKind Kind
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public string FriendlyName
        {
            get;
            private set;
        }

        public string DeviceStateFriendlyName
        {
            get;
            private set;
        }

        public Image Image
        {
            get;
            private set;
        }

        public bool IsVisible
        {
            get;
            private set;
        }

        public void UpdateStatus(AudioDeviceManagerFacade deviceManager)
        {
            DefaultState = deviceManager.GetUnderlyingManager().CalculateDeviceDefaultStatuses(_device);
            Kind = _device.Kind;
            State = _device.State;
            IsVisible = CalculateIsVisible();

            // Only do work such as get text, icons, etc if we're visible
            if (IsVisible)
            {   
                Description = TryGetOrDefault(_device.TryGetDeviceDescription, Description);
                FriendlyName = TryGetOrDefault(_device.TryDeviceFriendlyName, FriendlyName);
                DeviceStateFriendlyName = GetDeviceStateFriendlyName();

                Image image;
                if (DeviceImage.TryGetImage(deviceManager.GetUnderlyingManager(), _device,out image))
                {
                    Image = image;
                }
            }
        }

        private bool CalculateIsVisible()
        {
            bool isVisible = true;

            switch (Kind)
            {
                case AudioDeviceKind.Playback:
                    isVisible &= Settings.Default.ShowPlaybackDevices;
                    break;

                case AudioDeviceKind.Recording:
                    isVisible &= Settings.Default.ShowRecordingDevices;
                    break;
            }

            switch (State)
            {
                case AudioDeviceState.Active:
                    break;

                case AudioDeviceState.Disabled:
                    isVisible &= Settings.Default.ShowDisabledDevices;
                    break;

                case AudioDeviceState.Unplugged:
                    isVisible &= Settings.Default.ShowUnpluggedDevices;
                    break;

                default:
                case AudioDeviceState.NotPresent:
                    isVisible &= Settings.Default.ShowNotPresentDevices;
                    break;
            }

            return isVisible;
        }

        private string GetDeviceStateFriendlyName()
        {
            // To mimic the Sound control panel, we display a device's 
            // default state first, and only then fall back to the actual
            // device's state if it's not a default device.

            if (DefaultState.IsSet(AudioDeviceDefaultStatuses.All))
            {
                return Resources.DeviceState_DefaultDevice;
            }

            if (DefaultState.IsSet(AudioDeviceDefaultStatuses.Multimedia))
            {
                return Resources.DeviceState_DefaultMultimediaDevice;
            }

            if (DefaultState.IsSet(AudioDeviceDefaultStatuses.Communications))
            {
                return Resources.DeviceState_DefaultCommunicationsDevice;
            }

            switch (State)
            {
                case AudioDeviceState.Active:
                    return Resources.DeviceState_Active;

                case AudioDeviceState.Disabled:
                    return Resources.DeviceState_Disabled;

                case AudioDeviceState.NotPresent:
                    return Resources.DeviceState_NotPresent;

                case AudioDeviceState.Unplugged:
                    return Resources.DeviceState_Unplugged;
            }

            return String.Empty;
        }

        private static string TryGetOrDefault(TryDelegate getter, string defaultValue)
        {
            string result;
            if (getter(out result))
            {
                return result;
            }

            return defaultValue;
        }

        private delegate bool TryDelegate(out string result);
    }
}
