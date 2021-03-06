﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace Cadi.UI.ViewModels
{
    public class RadioPageViewModel: ViewModelBase
    {
		public override bool ShowRadio => false;

		private string _station = "88.7";
        public string Station
        {
            get => _station;
            set => SetProperty(ref _station, value);
        }

        private string _amFm = "FM";
        public string AmFm
        {
            get => _amFm;
            set => SetProperty(ref _amFm, value);
        }

        public ICommand GoBackCommand { get; }
        public ICommand PresetCommand { get; }
        public RadioPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService, eventAggregator)
        {
            GoBackCommand = new DelegateCommand(GoBackCommandExecute);
            PresetCommand = new DelegateCommand<string>(PresetCommandExecute);
        }

        private void PresetCommandExecute(string presetNumber)
        {
            Station = (int.Parse(presetNumber) * 0.8 + 88.7).ToString();
        }

        private void GoBackCommandExecute()
        {
            NavigationService.NavigateAsync("MainPage");
        }
    }
}
