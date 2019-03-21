using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace Cadi.UI.ViewModels
{
    public class AirConditionerPageViewModel : ViewModelBase
    {
        public ICommand GoBackCommand { get; }
        public AirConditionerPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            GoBackCommand = new DelegateCommand(GoBackCommandExecute);
        }

        private void GoBackCommandExecute()
        {
            NavigationService.NavigateAsync("MainPage");        }
    }
}
