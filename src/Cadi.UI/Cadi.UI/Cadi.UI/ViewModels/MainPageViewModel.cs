using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cadi.UI.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
		public override bool ShowHome => false;

		private bool leftTurnActive;
		public bool LeftTurnActive
		{
			get { return leftTurnActive; }
			set { SetProperty(ref leftTurnActive, value); }
		}

		private bool rightTurnActive;
		public bool RightTurnActive
		{
			get { return rightTurnActive; }
			set { SetProperty(ref rightTurnActive, value); }
		}

		public ICarSubSystem CarSubSystem { get; }

		private SubscriptionToken leftTurnEventSubToken;

		public MainPageViewModel(
			ICarSubSystem carSubSystem,
			INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService, eventAggregator)
        {
            Title = "Main Page";
			CarSubSystem = carSubSystem;
			LeftTurnActive = CarSubSystem.IsLeftTurnSignalOn;
		}

		protected override void OnNavigatedToBase(INavigationParameters parameters)
		{
			leftTurnEventSubToken = EventAggregator.GetEvent<LeftTurnSignalChangedEvent>().Subscribe(isOn => LeftTurnActive = isOn);
		}

		protected override void OnNavigatedFromBase(INavigationParameters parameters)
		{
			EventAggregator.GetEvent<LeftTurnSignalChangedEvent>().Unsubscribe(leftTurnEventSubToken);
		}
	}
}
