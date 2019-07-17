using Prism.Commands;
using Prism.Events;
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
		public override bool ShowHome => false;

        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService, eventAggregator)
        {
            Title = "Main Page";
        }

    }
}
