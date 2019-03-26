using Prism;
using Prism.Ioc;

namespace Cadi.UI.GTK
{
    public class GtkInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.RegisterSingleton<IGpioService, RaspberryPiGpioService>();
        }
    }
}
