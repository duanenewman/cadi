using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cadi.UI.ViewModels;
using Prism.Events;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace Cadi.UI.GTK
{
    public class Program
    {
        private static IEventAggregator EventAggregator { get; set; }

        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            Forms.Init();

            var app = new App(new GtkInitializer());
            EventAggregator = app.Container.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<ExitEvent>().Subscribe(() => Gtk.Application.Quit());

            var window = new FormsWindow();
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
