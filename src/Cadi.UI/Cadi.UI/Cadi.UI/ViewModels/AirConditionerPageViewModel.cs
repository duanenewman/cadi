using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace Cadi.UI.ViewModels
{
    public class AirConditionerPageViewModel : ViewModelBase
    {
        //public GPIOPinDriver GPIO { get; set;  }
        public ICommand GoBackCommand { get; }
        public ICommand ToggleACCommand { get; }
        public IGpioService GpioService { get; }

        private string _acState;

        public string ACState
        {
            get { return _acState; }
            set { SetProperty(ref _acState, value); }
        }

        public AirConditionerPageViewModel(INavigationService navigationService, IGpioService GpioService) : base(navigationService)
        {
            GoBackCommand = new DelegateCommand(GoBackCommandExecute);
            ToggleACCommand = new DelegateCommand(ToggleACCommandExecute);
            this.GpioService = GpioService;

            GpioService.ExportPin(Pin.GPIO18, GPIODirection.Out, GPIOState.Low);
            GpioService.ExportPin(Pin.GPIO17, GPIODirection.In, GPIOState.Low);
        }

        private bool WasPin17Closed = false;
        private bool WatchingPin;

        private void ToggleACCommandExecute()
        {
            var nextState =
                GpioService.GetPinState(Pin.GPIO18) == GPIOState.High
                ? GPIOState.Low
                : GPIOState.High;

            GpioService.SetPinState(Pin.GPIO18, nextState);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //GPIO = new GPIOPinDriver(GPIOPinDriver.Pin.GPIO18, GPIOPinDriver.GPIODirection.Out, GPIOPinDriver.GPIOState.Low);
            WatchingPin = true;
            Task.Run(async () =>
            {
                while (WatchingPin)
                {
                    var isPin17Closed = GpioService.GetPinState(Pin.GPIO17) == GPIOState.High;

                    var changed = (!WasPin17Closed && isPin17Closed) || (WasPin17Closed && !isPin17Closed);
                    if (changed)
                    {
                        if (isPin17Closed)
                        {
                            var currentState = GpioService.GetPinState(Pin.GPIO18);

                            GpioService.SetPinState(Pin.GPIO18,
                                currentState == GPIOState.Low ? GPIOState.High : GPIOState.Low);
                        }

                        WasPin17Closed = isPin17Closed;
                    }

                    await Task.Delay(100);
                }
            });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            //GPIO.Dispose();
            //GPIO = null;
            WatchingPin = false;
        }

        private void GoBackCommandExecute()
        {
            NavigationService.NavigateAsync("MainPage");
        }
    }


}
