using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;

namespace Cadi.UI.ViewModels
{
	public class BluetoothPageViewModel : ViewModelBase
	{
		private bool isMuted;
		public bool IsMuted
		{
			get => isMuted;
			set => SetProperty(ref isMuted, value, 
				() => RaisePropertyChanged(nameof(ActiveVolume)));
		}

		private int activeVolume;
		public int ActiveVolume
		{
			get => IsMuted ? 0 : activeVolume;
			set {
				if (IsMuted) return;

				SetProperty(ref activeVolume, value,
					() => IsMuted = false);
			}
		}

		private bool isMusicView;
		public bool IsMusicView {
			get => isMusicView;
			private set => SetProperty(ref isMusicView, value); }

		private bool isPhoneView;
		public bool IsPhoneView
		{
			get => isPhoneView;
			private set => SetProperty(ref isPhoneView, value);
		}

		public override bool ShowBluetooth => false;

		public DelegateCommand SwitchToPhoneViewCommand =>
			new DelegateCommand(() => SwitchToPhoneView());
		public DelegateCommand SwitchToMusicViewCommand =>
			new DelegateCommand(() => SwitchToMusicView());

		public DelegateCommand MuteVolumeCommand =>
			new DelegateCommand(() => IsMuted = !IsMuted);
		public DelegateCommand LowerVolumeCommand =>
			new DelegateCommand(() => ActiveVolume = Math.Max(ActiveVolume - 1, 0));
		public DelegateCommand RaiseVolumeCommand =>
			new DelegateCommand(() => ActiveVolume = Math.Min(ActiveVolume + 1, 255));


		private void SwitchToPhoneView()
		{
			IsMusicView = false;
			IsPhoneView = true;
		}

		private void SwitchToMusicView()
		{
			IsPhoneView = false;
			IsMusicView = true;
		}

		public BluetoothPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService, eventAggregator)
		{
			SwitchToPhoneView();
		}
	}
}
