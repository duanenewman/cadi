using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

		public virtual bool ShowHome => true;
		public virtual bool ShowRadio => true;
		public virtual bool ShowAirConditioner => true;

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

		private bool RunClock { get; set; }

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

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
			RunClock = false;
			OnNavigatedFromBase(parameters);
		}

		protected virtual void OnNavigatedFromBase(INavigationParameters parameters) { }

		public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
			var task = new Task(async () =>
			{
				RunClock = true;
				while (RunClock)
				{
					ClockDate = DateTime.Now.ToShortDateString();
					ClockTime = DateTime.Now.ToShortTimeString();
					await Task.Delay(1000);
				}
			});
			task.Start();
			OnNavigatedToBase(parameters);
		}

		protected virtual void OnNavigatedToBase(INavigationParameters parameters) { }

		public void OnNavigatingTo(INavigationParameters parameters)
        {
			OnNavigatingToBase(parameters);
		}

		protected virtual void OnNavigatingToBase(INavigationParameters parameters) { }

		public virtual void Destroy()
        {

        }
    }
}
