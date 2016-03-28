using AudioSwitcher.Audio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioSwitcher.Drawing
{
	public static class DeviceImage
	{
		private static readonly Size IconSize = DpiServices.Scale(new Size(48, 48));

		public static bool TryGetImage(AudioDeviceManager deviceManager, AudioDevice device, out Image image)
		{
			string iconPath;
			if (device.TryGetDeviceClassIconPath(out iconPath))
			{
				image = GetImage(deviceManager, device, iconPath);
				return true;
			}
			else
			{
				image = null;
				return false;
			}
		}

		private static Image GetImage(AudioDeviceManager deviceMananger, AudioDevice device, string iconPath)
		{
			Image deviceImage = GetDeviceImage(device, iconPath);
			if (deviceImage == null)
				return null;

			Image overlayImage = GetOverlayImage(deviceMananger, device);
			if (overlayImage == null)
				return deviceImage;

			using (deviceImage)
			using (overlayImage)
			{
				// Makes a copy
				return DrawingServices.CreateOverlayedImage(deviceImage, overlayImage, deviceImage.Size);
			}
		}

		private static Image GetDeviceImage(AudioDevice device, string iconPath)
		{
			using (Icon icon = GetIconFromDeviceIconPath(iconPath))
			{
				if (icon == null)
					return null;

				Image image = icon.ToBitmap();
				if (device.State == AudioDeviceState.Active)
					return image;

				using (image)
				{
					return ToolStripRenderer.CreateDisabledImage(image);
				}
			}
		}

		private static Icon GetIconFromDeviceIconPath(string iconPath)
		{
			if (String.IsNullOrEmpty(iconPath))
				return null;

			Icon icon;
			if (String.IsNullOrEmpty(iconPath) || !ShellIcon.TryExtractIconByIdOrIndex(iconPath, IconSize, out icon))
				return new Icon(Icons.FallbackDevice, IconSize);

			return icon;
		}

		private static Image GetOverlayImage(AudioDeviceManager deviceManager, AudioDevice device)
		{
			var defaultState = CalculateDeviceDefaultState(deviceManager, device);
			if (defaultState.IsSet(AudioDeviceDefaultState.Multimedia))
			{   // Sound control panel shows the same icon between all and multimedia
				return Icons.DefaultMultimediaDevice;
			}

			if (defaultState.IsSet(AudioDeviceDefaultState.Communications))
			{
				return Icons.DefaultCommunicationsDevice;
			}

			switch (device.State)
			{
				case AudioDeviceState.Disabled:
					return Icons.Disabled;

				case AudioDeviceState.NotPresent:
					return Icons.NotPresent;

				case AudioDeviceState.Unplugged:
					return Icons.Unplugged;
			}

			return null;
		}

		public static AudioDeviceDefaultState CalculateDeviceDefaultState(AudioDeviceManager deviceManager, AudioDevice device)
		{
			AudioDeviceDefaultState state = AudioDeviceDefaultState.None;

			if (deviceManager.IsDefaultAudioDevice(device, AudioDeviceRole.Multimedia))
			{
				state |= AudioDeviceDefaultState.Multimedia;
			}

			if (deviceManager.IsDefaultAudioDevice(device, AudioDeviceRole.Communications))
			{
				state |= AudioDeviceDefaultState.Communications;
			}

			return state;
		}
	}
}
