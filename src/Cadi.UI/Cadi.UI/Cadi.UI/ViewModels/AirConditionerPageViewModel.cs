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

        private string _acState;
        private SubscriptionToken eventSubKey;

        public string ACState
        {
            get { return _acState; }
            set { SetProperty(ref _acState, value); }
        }

        public AirConditionerPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator , ICarSubSystem carSubSystem) : base(navigationService, eventAggregator)
        {
            ToggleACCommand = new DelegateCommand(ToggleACCommandExecute);
            CarSubSystem = carSubSystem;
            SetAcState(CarSubSystem.IsAcOn);
        }

        private void SetAcState(bool isOn)
        {
            ACState = isOn ? "On" : "Off";
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
