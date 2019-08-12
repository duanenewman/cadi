using System;
using Prism.Events;
using Prism.Navigation;

namespace Cadi.UI.ViewModels
{
	public class RearviewPageViewModel : ViewModelBase
	{
		public override bool ShowRearview => false;

		public RearviewPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService, eventAggregator)
		{
		}

	}
}
