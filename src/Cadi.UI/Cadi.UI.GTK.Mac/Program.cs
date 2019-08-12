using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cadi.UI.ViewModels;
using LibVLCSharp.Shared;
using Prism.Events;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace Cadi.UI.GTK.Mac
{
    public class Program
    {
        private static IEventAggregator EventAggregator { get; set; }

        [STAThread]
        public static void Main(string[] args)
        {
			Core.Initialize();

			var app = new GtkApp();
			app.Main(new MacGtkInitializer());

        }
    }
}
