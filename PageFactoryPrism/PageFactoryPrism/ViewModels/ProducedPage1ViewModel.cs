using System.Windows.Input;
using PageFactoryPrism.Attributes;
using PageFactoryPrism.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace PageFactoryPrism.ViewModels
{
    public interface ITestPageViewModel
    {
    }

    [Title("Produced Page 1")]
    public class ProducedPage1ViewModel:ViewModelBase, ITestPageViewModel
    {
        private string _shortDescription;
        private string _shortDescription2;
        private string _longDescription;
        private string _summary;

        public ProducedPage1ViewModel(INavigationService navigationService) : base(navigationService)
        {            
            Summary = "Something";
            PageFactoryNavigateCommand = new Command(DoPageFactoryNavigation);
            PrismNavigateCommand = new DelegateCommand(DoPrismNavigation);
        }

        [Entry, Title("Give a short description"), Required]
        public string ShortDescription2
        {
            get => _shortDescription2;
            set
            {
                if (value == _shortDescription2) return;
                _shortDescription2 = value;
                RaisePropertyChanged();
            }
        }


        private async void DoPrismNavigation()
        {
            await NavigationService.NavigateAsync(nameof(Page3)); 
        }

        private async void DoPageFactoryNavigation()
        {
            await NavigationService.NavigateAsync(typeof(ProducedPage2ViewModel).AssemblyQualifiedName);
        }


        [LongText,  Title("Describe the problem in details")]
        public string LongDescription
        {
            get => _longDescription;
            set
            {
                if (value == _longDescription) return;
                _longDescription = value;
                RaisePropertyChanged();
            }
        }


        [Label, Title("Summary") , Required]
        public string Summary
        {
            get => _summary;
            set
            {
                if (value == _summary) return;
                _summary = value;
                RaisePropertyChanged();
            }
        }

        [Entry, Title("Give a short description"), Required]
        public string ShortDescription
        {
            get => _shortDescription;
            set
            {
                if (value == _shortDescription) return;
                _shortDescription = value;
                RaisePropertyChanged();
            }
        }

     

        [Title("PageFactory Navigate")]
        public ICommand PageFactoryNavigateCommand { get; set; }

        [Title("Prism Navigate ")]
        public ICommand PrismNavigateCommand { get; set; }

    }
}