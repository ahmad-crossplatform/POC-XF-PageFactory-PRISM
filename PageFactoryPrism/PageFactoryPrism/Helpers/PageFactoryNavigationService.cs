using System;
using System.Threading.Tasks;
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
}