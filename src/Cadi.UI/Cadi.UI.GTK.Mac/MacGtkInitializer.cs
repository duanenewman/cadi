using Prism;
using Prism.Ioc;

namespace Cadi.UI.GTK.Mac
{
    public class MacGtkInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
			containerRegistry.RegisterSingleton<IGpioService, DesktopGpioService>();
		}
    }
}
