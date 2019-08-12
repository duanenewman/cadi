using Prism;
using Prism.Ioc;
using System;
using System.Runtime.InteropServices;

namespace Cadi.UI.GTK
{
    public class GtkInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
			// Register any platform specific implementations
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				containerRegistry.RegisterSingleton<IGpioService, DesktopGpioService>();
			}
			else if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				containerRegistry.RegisterSingleton<IGpioService, RaspberryPiGpioService>();
			}
			else
			{
				containerRegistry.RegisterSingleton<IGpioService, DesktopGpioService>();
			}
		}
    }
}
