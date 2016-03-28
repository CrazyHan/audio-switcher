using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAudioSwitcherClient
{
	class Program
	{
		static void Main(string[] args)
		{
			AudioSwitcher.Audio.AudioDeviceManagerApi api = new AudioSwitcher.Audio.AudioDeviceManagerApi();
			var audioDevices = api.GetAudioDevices(AudioSwitcher.Audio.AudioDeviceKind.All, AudioSwitcher.Audio.AudioDeviceState.All);
			var defaultCommunicationDevice = api.GetDefaultAudioDevice(AudioSwitcher.Audio.AudioDeviceKind.Playback, AudioSwitcher.Audio.AudioDeviceRole.Communications);
			var defaultConsoleDevice = api.GetDefaultAudioDevice(AudioSwitcher.Audio.AudioDeviceKind.Playback, AudioSwitcher.Audio.AudioDeviceRole.Console);
			var defaultMultimediaDevice = api.GetDefaultAudioDevice(AudioSwitcher.Audio.AudioDeviceKind.Playback, AudioSwitcher.Audio.AudioDeviceRole.Multimedia);
			var defaultRecordingCommunicationDevice = api.GetDefaultAudioDevice(AudioSwitcher.Audio.AudioDeviceKind.Recording, AudioSwitcher.Audio.AudioDeviceRole.Communications);
			var defaultRecordingConsoleDevice = api.GetDefaultAudioDevice(AudioSwitcher.Audio.AudioDeviceKind.Recording, AudioSwitcher.Audio.AudioDeviceRole.Console);
			var defaultRecordingMultimediaDevice = api.GetDefaultAudioDevice(AudioSwitcher.Audio.AudioDeviceKind.Recording, AudioSwitcher.Audio.AudioDeviceRole.Multimedia);
		}
	}
}
