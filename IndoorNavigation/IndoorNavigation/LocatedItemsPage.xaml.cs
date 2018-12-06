using IndoorNavigation.Helpers;
using JDA.Entities.Response;
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
            if(Vm != null && Vm.Products != null && Vm.Products.Count>0)
            {
                LstView.HeightRequest = Vm.Products.Count * 80;
            }
            else
            {
                //LstView.HeightRequest = 0;
            }
            if (Vm != null && Vm.Facilities != null && Vm.Facilities.Count > 0)
            {
                LstView1.HeightRequest = Vm.Facilities.Count * 80;
            }
            else
            {
                //LstView1.HeightRequest = 0;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var item = ((sender as Grid).BindingContext as MyProducts);
            Navigation.PushAsync(new LocateItemPage(item.LocationX, item.LocationY));
        }
    }
}