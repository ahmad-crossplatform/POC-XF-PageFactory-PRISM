# Integrating PageFactory in a Prism Project

## Intro
The [Page Factory concept](https://github.com/ahmad-crossplatform/XamarinFormsPageFactoryPOC) would be useless if does not work with MVVM frameworks. 
Here we are doing a POC of how the Page Pactory can work as along with Prism.  We chose Prism because it is one of the biggest and most known frameworks , and because we use it usually. 

## Steps for Integration 
### Overriding NavigationService
We had to implement our own version of `PageNavigationService` where we override `GetCurrentPage`and `CreatePage`  to be able to integrate the `PageFactory`.  **Notice** that `PageFactory` is injected into the service.  

```C#
public class PageFactoryNavigationService:PageNavigationService
{
	private readonly IPageFactory _pageFactory;

	public PageFactoryNavigationService(IPageFactory pageFactory, IContainerExtension container, IApplicationProvider applicationProvider, IPageBehaviorFactory pageBehaviorFactory, ILoggerFacade logger) : base(container, applicationProvider, pageBehaviorFactory, logger)
	{
		_pageFactory = pageFactory;
	}
    protected override Page GetCurrentPage()
    {
        return _applicationProvider?.MainPage?.Navigation?.NavigationStack[0]; 
    }
    protected override Page CreatePage(string segment)
    {
        if (Type.GetType(segment) != null) //in this case it should get the fully qualified assembly name
        {
            var type = Type.GetType(segment);   
            var page = _pageFactory.CreatePage(type);
            return page;
        }
        else
        {
            return base.CreatePage(segment);
        }
    }
}
```

### Modification on Page
We have modified the PageFactory class so it takes the designated viewmodel from the container, this way the container will resolve all the dependencies injected in the constructor . 

```C#
  public class PageFactory : IPageFactory
    {
        private readonly IContainerExtension _containerProvider;

        public PageFactory(IContainerExtension containerProvider)
        {
            _containerProvider = containerProvider;
        }
        public  Page CreatePage<T> () where T: INotifyPropertyChanged
        {         
            return CreatePage(typeof(T)); 
        }
        
        public  Page CreatePage (Type type)
        {
            var viewModel = _containerProvider.Resolve(type); 
            return CreatePage((ViewModelBase)viewModel, type); 
        }
         public Page CreatePage(INotifyPropertyChanged viewModel, Type type)
        {
        //........ The rest of the class 
     }
```
### Registrations on `App.cs` 
Here we simply Register `PageFactory`, `PageFactoryNavigationService`, the viewmodels ,  and the other pages of the app. 
**Note:** *We do not need to register the ViewModels which are fed to the PageFactory as they are resolved by the container*  
**Note:** *We need to register `PageFactoryNavigationService` twice; once with a named index and once without to make sure that Prism will always refer to our implementation.*

```C#
 public partial class App : PrismApplication
    {
    	// Other Implementations and overrides 
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
```
### Modifications on ViewModels 

No much of modification needed here except that when navigating to another Page you can decide whether you want to use original implementation for `NavigationService` or ours. To use our implementation you have to pass the `AssemblyQualifiedName` of the designated  ViewModel to the `NavigtateAsync` method. 
```c#
  private async void DoPrismNavigation()
        {
            await NavigationService.NavigateAsync(nameof(Page3)); 
        }

        private async void DoPageFactoryNavigation()
        {
            await NavigationService.NavigateAsync(typeof(ProducedPage2ViewModel).AssemblyQualifiedName);
        }
```
