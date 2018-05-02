using System;
using Prism.Behaviors;
using Prism.Common;
using Prism.Ioc;
using Prism.Logging;
using Prism.Navigation;
using Xamarin.Forms;

namespace PageFactoryPrism.Helpers
{
    public class PageFactoryNavigationService:PageNavigationService
    {
        private readonly IPageFactory _pageFactory;

        public PageFactoryNavigationService(IPageFactory pageFactory, IContainerExtension container, IApplicationProvider applicationProvider, IPageBehaviorFactory pageBehaviorFactory, ILoggerFacade logger) : base(container, applicationProvider, pageBehaviorFactory, logger)
        {
            _pageFactory = pageFactory;
        }
        protected override Page GetCurrentPage()
        {
            if (Application.Current.MainPage == null) return base.GetCurrentPage(); // If it is the first page . 
            if (_applicationProvider.MainPage is NavigationPage navigationPage)
            {
                var currentPage = navigationPage.CurrentPage;
                return currentPage; 
            }
            return base.GetCurrentPage();
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
}