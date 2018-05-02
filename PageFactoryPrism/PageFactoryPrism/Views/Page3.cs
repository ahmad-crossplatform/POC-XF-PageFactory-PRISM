using Xamarin.Forms;

namespace PageFactoryPrism.Views
{
	public class Page3 : ContentPage
	{
		public Page3 ()
		{
			Content = new StackLayout {
				Children = {
					new Label { Text = "Welcome to Xamarin.Forms!" }
				}
			};
		}
	}
}