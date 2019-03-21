using Prism.Commands;
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
        private string _clockTime;
        public string ClockTime
        {
            get => _clockTime;
            set => SetProperty(ref _clockTime, value);
        }

        private string _clockDate;
        public string ClockDate
        {
            get => _clockDate;
            set => SetProperty(ref _clockDate, value);
        }

        public ICommand RadioCommand { get; }
        public ICommand AirConditionerCommand { get; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
            RadioCommand = new DelegateCommand(NavigateToRadioPage);
            AirConditionerCommand = new DelegateCommand(NavigateToAirConditionerPage);
            Task.Run(async () => 
            {
                while (true)
                {
                    ClockDate = DateTime.Now.ToShortDateString();
                    ClockTime = DateTime.Now.ToShortTimeString();
                    await Task.Delay(1000);
                }
            });
        }

        private void NavigateToAirConditionerPage()
        {
            NavigationService.NavigateAsync("AirConditionerPage");
        }

        private void NavigateToRadioPage()
        {
            NavigationService.NavigateAsync("RadioPage");
        }
    }
}
