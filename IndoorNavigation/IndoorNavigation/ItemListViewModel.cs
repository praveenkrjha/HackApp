using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using JDA.Entities.Response;
using System.Collections.ObjectModel;
using IndoorNavigation.Helpers;

namespace IndoorNavigation
{
    public class ItemListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MyProducts> _Products;
        private ObservableCollection<MyProducts> _Facilities;
        private MyProducts _SelectedFacilitate;
        public MyProducts SelectedFacilitate
        {
            get
            {
                return _SelectedFacilitate;
            }
            set
            {
                _SelectedFacilitate = value;
                OnPropertyChanged("SelectedFacilitate");
                if (_SelectedFacilitate != null)
                {

                }
            }
        }
        private MyProducts _SelectedProduct;
        public MyProducts SelectedProduct
        {
            get
            {
                return _SelectedProduct;
            }
            set
            {
                _SelectedProduct = value;
                OnPropertyChanged("SelectedProduct");
                if(_SelectedProduct != null)
                {

                }
            }
        }
        
        public ObservableCollection<MyProducts> Products
        {
            get
            {
                return _Products;
            }
            set
            {
                _Products = value;
                OnPropertyChanged("Products");
            }
        }
        public ObservableCollection<MyProducts> Facilities
        {
            get
            {
                return _Facilities;
            }
            set
            {
                _Facilities = value;
                OnPropertyChanged("Facilities");
            }
        }
        public ItemListViewModel()
        {
            LoadItems();
        }

        private void LoadItems()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var restClient = new RestClient();
                    var resp = await restClient.GetAsync<ServiceResponse<ProductsAndFacilities>>(AppConstants.BaseUrl + "Product");
                    if (resp.IsSuccess && resp.Data != null)
                    {
                        Products = new ObservableCollection<MyProducts>();
                        foreach(var item in resp.Data.Products)
                        {
                            var pd = new MyProducts
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Description = item.Description,
                                LocationX = item.LocationX,
                                LocationY = item.LocationY,
                            };
                            switch(pd.Name)
                            {
                                case "Water Bottle":
                                    pd.Source = "waterbottle.png";
                                    break;
                                case "Fridge":
                                    pd.Source = "fridge.png";
                                    break;
                                case "TV":
                                    pd.Source = "tv.png";
                                    break;
                                default:
                                    break;
                            }
                            Products.Add(pd);

                        }
                        Facilities = new ObservableCollection<MyProducts>();
                        foreach (var item in resp.Data.Facilities)
                        {
                            var pd = new MyProducts
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Description = item.Description,
                                LocationX = item.LocationX,
                                LocationY = item.LocationY,
                            };
                            switch (pd.Name)
                            {
                                case "Entry":
                                    pd.Source = "openeddoor.png";
                                    break;
                                case "Exit":
                                    pd.Source = "closeddoor.png";
                                    break;
                                default:
                                    break;
                            }
                            Facilities.Add(pd);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
