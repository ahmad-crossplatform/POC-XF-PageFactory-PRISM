using System.Windows.Input;
using PageFactoryPrism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace PageFactoryPrism.ViewModels
{
    public class FirstPageViewModel : ViewModelBase
    {
        public string Something { get; set; }
        public FirstPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            LoginCommand = new DelegateCommand(DoLogin).ObservesProperty(()=> Something);
           
        }

        private async void DoLogin()
        {
            await NavigationService.NavigateAsync(typeof(ProducedPage1ViewModel).AssemblyQualifiedName); 
        }

        public ICommand LoginCommand { get; set; }
    }
}
