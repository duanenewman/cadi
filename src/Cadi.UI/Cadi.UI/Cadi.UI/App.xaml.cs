using Prism;
using Prism.Ioc;
using Cadi.UI.ViewModels;
using Cadi.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Reflection;
using System.Linq;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Cadi.UI
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var carSubsystem = Container.Resolve<ICarSubSystem>();
            carSubsystem.Start();

            await NavigationService.NavigateAsync("MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICarSubSystem, CarSubSystem>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<RadioPage, RadioPageViewModel>();
            containerRegistry.RegisterForNavigation<BluetoothPage, BluetoothPageViewModel>();
            containerRegistry.RegisterForNavigation<AirConditionerPage, AirConditionerPageViewModel>();
            containerRegistry.RegisterForNavigation<RearviewPage, RearviewPageViewModel>();
        }
    }

	[ContentProperty("OsPlatforms")]
	public class OnOsPlatform<T>
	{

		public OnOsPlatform()
		{
			OsPlatforms = new List<On>();
		}

		public IList<On> OsPlatforms
		{
			get;
			set;
		} 

		public T Default
		{
			get;
			set;
		}

		public static implicit operator T(OnOsPlatform<T> onOsPlatform)
		{

			foreach (var on in onOsPlatform.OsPlatforms)
			{
				if (on.Platform == null)
					continue;
				if (!on.Platform.Any(p => RuntimeInformation.IsOSPlatform(OSPlatform.Create(p.ToUpper()))))
					continue;

				return on.Value is T ? (T)on.Value : (T)Convert.ChangeType(on.Value, typeof(T));

			}

			return onOsPlatform.Default;
		}

	}
}
