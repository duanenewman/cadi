using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Cadi.UI.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IEventAggregator EventAggregator { get; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ICommand RadioCommand { get; }
        public ICommand AirConditionerCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand NavigateToHomeCommand { get; }

        public ViewModelBase(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            NavigationService = navigationService;
            EventAggregator = eventAggregator;
            RadioCommand = new DelegateCommand(NavigateToRadioPage);
            AirConditionerCommand = new DelegateCommand(NavigateToAirConditionerPage);
            ExitCommand = new DelegateCommand(ExitCommandExecute);
            NavigateToHomeCommand = new DelegateCommand(NavigateToHomeCommandExecute);
        }

        private void NavigateToAirConditionerPage()
        {
            NavigationService.NavigateAsync("AirConditionerPage");
        }

        private void NavigateToRadioPage()
        {
            NavigationService.NavigateAsync("RadioPage");
        }

        private void ExitCommandExecute()
        {
            EventAggregator.GetEvent<ExitEvent>().Publish();
        }
        private void NavigateToHomeCommandExecute()
        {
            NavigationService.NavigateAsync("MainPage");
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
