using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cadi.UI.ViewModels;
using LibVLCSharp.Shared;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace Cadi.UI.GTK
{
    public class GtkApp
    {
        private IEventAggregator EventAggregator { get; set; }

        public void Main(IPlatformInitializer platformInitializer)
        {
            Gtk.Application.Init();
            Forms.Init();

			var app = new App(platformInitializer);
            EventAggregator = app.Container.Resolve<IEventAggregator>();
			EventAggregator.GetEvent<ExitEvent>().Subscribe(() => Gtk.Application.Quit());

			using (var window = new FormsWindow())
			{
				window.LoadApplication(app);
				window.SetApplicationTitle("CADI");

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					window.Fullscreen();
				}

				window.Show();

				Gtk.Application.Run();
			}
        }
    }
}
