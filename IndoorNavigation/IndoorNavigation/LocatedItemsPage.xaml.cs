using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IndoorNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocatedItemsPage : ContentPage
    {
        ItemListViewModel Vm;
        public LocatedItemsPage()
        {
            InitializeComponent();
            Vm = new ItemListViewModel();
            this.BindingContext = Vm;
            LstView.HeightRequest = Vm.model.Count * 80;
        }
    }
}