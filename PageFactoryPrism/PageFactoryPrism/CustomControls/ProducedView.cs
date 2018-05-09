using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PageFactoryPrism.CustomControls
{
	public class ProducedView : ContentView
	{
	

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is INotifyPropertyChanged viewModel)
            {
                Content =  PageFactory.CreateView(viewModel);
            }
        }
    }
}