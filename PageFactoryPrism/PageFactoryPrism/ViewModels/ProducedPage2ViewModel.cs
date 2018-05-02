using PageFactoryPrism.Attributes;
using Prism.Navigation;

namespace PageFactoryPrism.ViewModels
{
    public interface IPage2ViewModel
    {
    }

    [Title("Produced Page 2")]
    public class ProducedPage2ViewModel:ViewModelBase, IPage2ViewModel
    {
        public ProducedPage2ViewModel(INavigationService navigationService) : base(navigationService)
        {
            WelcomeMessage = "This is Page 2";
        }
        
        [Text]
        public string WelcomeMessage { get; set; }
    }
}