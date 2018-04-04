using ListeningApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListeningApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListenerView : ContentPage
	{
        ListenerViewModel _vm;
		public ListenerView ()
		{
            _vm = new ListenerViewModel();
            this.BindingContext = _vm;
			InitializeComponent ();
		}
	}
}