using PageFactoryPrism.Helpers;
using PageFactoryPrism.ViewModels;
using Prism;
using Prism.Ioc;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using PageFactoryPrism.Views;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PageFactoryPrism
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        public App() : this(null)
        {
        }
        public App(IPlatformInitializer initializer) : base(initializer)
        {


        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync($"NavigationPage/FirstPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            /*INavigationService has to be registered in both modes, named and unnamed.*/
            containerRegistry.Register<INavigationService, PageFactoryNavigationService>(NavigationServiceName);
            containerRegistry.Register<INavigationService, PageFactoryNavigationService>();

            containerRegistry.Register<IPageFactory, PageFactory>();       

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<FirstPage, FirstPageViewModel>();
            containerRegistry.RegisterForNavigation<Page3>();
        }
    }
}
