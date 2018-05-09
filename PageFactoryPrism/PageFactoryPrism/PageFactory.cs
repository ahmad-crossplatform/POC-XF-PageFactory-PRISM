using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PageFactoryPrism.Attributes;
using PageFactoryPrism.CustomControls;
using PageFactoryPrism.ViewModels;
using Prism.Ioc;
using Xamarin.Forms;

namespace PageFactoryPrism
{
    public interface IPageFactory
    {
        Page CreatePage<T>() where T : INotifyPropertyChanged;
        Page CreatePage(Type type);
        Page CreatePage(INotifyPropertyChanged viewModel, Type type);
        Task NavigateAsync(Page nextPage, bool isAnimated = true);
        Task NavigateAsync<TDestination>(bool isAnimated = true) where TDestination : INotifyPropertyChanged;
        Task<bool> ShowAlert(string title, string message, string ok, string cancel = "");
    }

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
            var page = new ContentPage
            {
                BindingContext = viewModel
            };
            if (viewModel.GetType().CustomAttributes.Any(c => c.AttributeType == typeof(TitleAttribute))) // Page Level
            {
                var titleAttribte = viewModel.GetType().CustomAttributes.Single(c => c.AttributeType == typeof(TitleAttribute));
                page.Title = titleAttribte.ConstructorArguments[0].Value.ToString();
            }
            page.Content = CreateView(viewModel);
            return page;
        }

        public static View CreateView(INotifyPropertyChanged viewModel)
        {
            var tableView = new TableView { HasUnevenRows = true };
            var tableRoot = new TableRoot();
            var tableSection = new TableSection();

            var stackLayout = new StackLayout();
            foreach (var propertyInfo in viewModel.GetType().GetProperties())
            {
                if (!propertyInfo.CustomAttributes.Any()) continue;
                var child = new RequiredCell();
                var isRequired = false;
                var title = "";

                foreach (var attribute in propertyInfo.CustomAttributes)
                {
                    switch (attribute.AttributeType.Name)
                    {
                        case nameof(TitleAttribute):
                            title = attribute.ConstructorArguments[0].Value.ToString();
                            break;
                        case nameof(RequiredAttribute):
                            isRequired = true;
                            break;
                        case nameof(LabelAttribute):
                            child = new RequiredLabelCell();
                            child.SetBinding(RequiredLabelCell.TextProperty, propertyInfo.Name, BindingMode.TwoWay);
                            break;
                        case nameof(EntryAttribute):
                            child = new RequiredEntryCell();
                            child.SetBinding(RequiredEntryCell.TextProperty, propertyInfo.Name, BindingMode.TwoWay);
                            break;
                        case nameof(LongTextAttribute):
                            child = new RequiredEditorCell();
                            child.SetBinding(RequiredEditorCell.TextProperty, propertyInfo.Name, BindingMode.TwoWay);
                            break;
                        case nameof(SwitchAttribute):
                            child = new RequiredSwitchCell();
                            child.SetBinding(RequiredSwitchCell.IsToggledProperty, propertyInfo.Name, BindingMode.TwoWay);
                            break;
                        case nameof(DateAttribute):
                            child = new RequiredDatePickerCell();
                            child.SetBinding(RequiredDatePickerCell.DateProperty, propertyInfo.Name, BindingMode.TwoWay);
                            break;
                        case nameof(CommandAttribute):
                            var commandName = attribute.ConstructorArguments[0].Value.ToString();
                            var command = (ICommand)viewModel.GetType().GetProperties().Single(p => p.Name == commandName).GetValue(viewModel);
                            child.Command = command;
                            break;
                    }
                }
                child.Title = title;
                child.IsRequired = isRequired;
                //Add the button
                if (propertyInfo.PropertyType == typeof(ICommand) || propertyInfo.PropertyType == typeof(Command))
                {
                    var command = (ICommand)propertyInfo.GetValue(viewModel);
                    var button = new Button
                    {
                        Text = title,
                        Command = command
                    };
                    child = new RequiredCell
                    {
                        View = button
                    };
                }
                tableSection.Add(child);
            }
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            stackLayout.Children.Add(tableView);
            return stackLayout; 
        }

        public async Task NavigateAsync<TDestination>(bool isAnimated = true) where TDestination : INotifyPropertyChanged
        {
            var nextPage = CreatePage<TDestination>();
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.CurrentPage.Navigation.PushAsync(nextPage, isAnimated);
            }
        }

        public async Task NavigateAsync(Page nextPage, bool isAnimated = true) 
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.CurrentPage.Navigation.PushAsync(nextPage, isAnimated);
            }
        }
        
        
        public async Task<bool> ShowAlert(string title, string message, string ok, string cancel ="")
        {
            if (!(Application.Current.MainPage is NavigationPage navigationPage)) return false;
            if (!string.IsNullOrEmpty(cancel))
            {
                return await navigationPage.CurrentPage.DisplayAlert(title, message, ok, cancel);
            }
            return await navigationPage.CurrentPage.DisplayAlert(title, message, null, ok);
        }
    }
}