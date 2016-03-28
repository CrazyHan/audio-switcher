// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
// This source file was altered for use in AudioSwitcher.
/*
  LICENSE
  -------
  Copyright (C) 2007 Ray Molenkamp

  This source code is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this source code or the software it produces.

  Permission is granted to anyone to use this source code for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this source code must not be misrepresented; you must not
     claim that you wrote the original source code.  If you use this source code
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original source code.
  3. This notice may not be removed or altered from any source distribution.
*/
using System;
using System.ComponentModel.Composition;

namespace AudioSwitcher.Audio
{
    [Export(typeof(AudioDeviceManagerFacade))]
    public class AudioDeviceManagerFacade : IDisposable
    {
        private AudioDeviceManager _audioDeviceManager;
        public AudioDeviceManagerFacade()
        {
            _audioDeviceManager = new AudioDeviceManager();
        }

        public event EventHandler<AudioDeviceEventArgs> DeviceAdded
        {
            add { _audioDeviceManager.DeviceAdded += value; }
            remove { _audioDeviceManager.DeviceAdded -= value; }
        }
        public event EventHandler<AudioDeviceRemovedEventArgs> DeviceRemoved
        {
            add { _audioDeviceManager.DeviceRemoved += value; }
            remove { _audioDeviceManager.DeviceRemoved -= value; }
        }
        public event EventHandler<AudioDeviceEventArgs> DevicePropertyChanged
        {
            add { _audioDeviceManager.DevicePropertyChanged += value; }
            remove { _audioDeviceManager.DevicePropertyChanged -= value; }
        }
        public event EventHandler<DefaultAudioDeviceEventArgs> DefaultDeviceChanged
        {
            add { _audioDeviceManager.DefaultDeviceChanged += value; }
            remove { _audioDeviceManager.DefaultDeviceChanged -= value; }
        }
        public event EventHandler<AudioDeviceStateEventArgs> DeviceStateChanged
        {
            add { _audioDeviceManager.DeviceStateChanged += value; }
            remove { _audioDeviceManager.DeviceStateChanged -= value; }
        }

        public AudioDeviceCollection GetAudioDevices(AudioDeviceKind kind, AudioDeviceState state)
        {
            return _audioDeviceManager.GetAudioDevices(kind, state);
        }

        public void SetDefaultAudioDevice(AudioDevice device)
        {
            _audioDeviceManager.SetDefaultAudioDevice(device);
        }

        public void SetDefaultAudioDevice(AudioDevice device, AudioDeviceRole role)
        {
            _audioDeviceManager.SetDefaultAudioDevice(device, role);
        }

        public bool IsDefaultAudioDevice(AudioDevice device, AudioDeviceRole role)
        {
            return _audioDeviceManager.IsDefaultAudioDevice(device, role);
        }

        public AudioDevice GetDefaultAudioDevice(AudioDeviceKind kind, AudioDeviceRole role)
        {
            return _audioDeviceManager.GetDefaultAudioDevice(kind, role);
        }

        public AudioDevice GetDevice(string id)
        {
            return _audioDeviceManager.GetDevice(id);
        }

        public void Dispose()
        {
            _audioDeviceManager.Dispose();
        }
    }
}
