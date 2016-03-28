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

			var defaultStatuses = deviceMananger.CalculateDeviceDefaultStatuses(device);
			Image overlayImage = GetOverlayImage(defaultStatuses, device.State);
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

		private static Image GetOverlayImage(AudioDeviceDefaultStatuses defaultStatuses, AudioDeviceState state)
		{
			if (defaultStatuses.IsSet(AudioDeviceDefaultStatuses.Multimedia))
			{   // Sound control panel shows the same icon between all and multimedia
				return Icons.DefaultMultimediaDevice;
			}

			if (defaultStatuses.IsSet(AudioDeviceDefaultStatuses.Communications))
			{
				return Icons.DefaultCommunicationsDevice;
			}

			switch (state)
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
	}
}
