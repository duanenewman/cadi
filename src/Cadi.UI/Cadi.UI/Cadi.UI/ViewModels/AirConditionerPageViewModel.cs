using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace Cadi.UI.ViewModels
{
    public class AirConditionerPageViewModel : ViewModelBase
    {
		public override bool ShowAirConditioner => false;

		public ICommand ToggleACCommand { get; }
        public ICarSubSystem CarSubSystem { get; }

		private SubscriptionToken eventSubKey;

		private bool isOn;
		public bool IsOn
		{
			get { return isOn; }
			set { SetProperty(ref isOn, value, () => RaisePropertyChanged(nameof(ACState))); }
		}

		private string _acState = "Auto";
        public string ACState
        {
            get { return IsOn ? _acState : "Off"; }
            private set { SetProperty(ref _acState, value); }
        }

		private string fanSpeed = "Low";

		public string FanSpeed
		{
			get { return fanSpeed; }
            private set { SetProperty(ref fanSpeed, value); }
		}

		public DelegateCommand AutomaticCommand =>
			new DelegateCommand(() => { ACState = "Auto"; IsOn = true; });

		public DelegateCommand ColdCommand =>
			new DelegateCommand(() => { ACState = "Cool"; IsOn = true; });

		public DelegateCommand HeatCommand =>
			new DelegateCommand(() => { ACState = "Heat"; IsOn = true; });

		public DelegateCommand SpeedCommand =>
			new DelegateCommand(() => UpdateFanSpeed());

		private void UpdateFanSpeed()
		{
			if (!IsOn)
			{
				IsOn = true;
				return;
			}

			switch (FanSpeed)
			{
				case "Low":
					FanSpeed = "Medium";
					IsOn = true;
					break;
				case "Medium":
					FanSpeed = "High";
					IsOn = true;
					break;
				case "High":
					IsOn = false;
					FanSpeed = "Low";
					break;
			}
		}

		public AirConditionerPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator , ICarSubSystem carSubSystem) : base(navigationService, eventAggregator)
        {
            ToggleACCommand = new DelegateCommand(ToggleACCommandExecute);
            CarSubSystem = carSubSystem;
            IsOn = CarSubSystem.IsAcOn;
        }

        private void SetAcState(bool isOn)
        {
			IsOn = isOn;
			//ACState = isOn ? "On" : "Off";
        }

        private void ToggleACCommandExecute()
        {
            var nextState = !CarSubSystem.IsAcOn;

            CarSubSystem.IsAcOn = nextState;
        }

        protected override void OnNavigatedToBase(INavigationParameters parameters)
        {
            eventSubKey = EventAggregator.GetEvent<AcStateChangedEvent>().Subscribe(isOn => SetAcState(isOn));
        }

		protected override void OnNavigatedFromBase(INavigationParameters parameters)
        {
            EventAggregator.GetEvent<AcStateChangedEvent>().Unsubscribe(eventSubKey);
        }

    }


}
