// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Linq;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.UI.ViewModels;
using AudioSwitcher.Drawing;

namespace AudioSwitcher.UI.Commands
{
    internal abstract class SetAsDefaultDeviceCommandBase : Command<AudioDeviceViewModel>
    {
        private readonly AudioDeviceManagerFacade _manager;
        private readonly AudioDeviceRole _role;

        protected SetAsDefaultDeviceCommandBase(AudioDeviceManagerFacade manager, AudioDeviceRole role)
        {
            _manager = manager;
            _role = role;
        }

        public override void Refresh(AudioDeviceViewModel argument)
        {
            IsChecked = IsDefaultDevice(argument.DefaultState);
            IsEnabled = argument.State == AudioDeviceState.Active && !IsChecked;
        }

        private bool IsDefaultDevice(AudioDeviceDefaultStatuses defaultState)
        {
            switch (_role)
            {
                case AudioDeviceRole.Communications:
                    return defaultState.IsSet(AudioDeviceDefaultStatuses.Communications);

                default:
                    Debug.Assert(_role == AudioDeviceRole.Multimedia);
                    return defaultState.IsSet(AudioDeviceDefaultStatuses.Multimedia);
            }
        }

        public override void Run(AudioDeviceViewModel argument)
        {
            if (argument.Device.IsActive)
                _manager.SetDefaultAudioDevice(argument.Device, _role);
        }
    }
}
