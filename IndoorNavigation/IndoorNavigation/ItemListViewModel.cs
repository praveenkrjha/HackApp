using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace IndoorNavigation
{
    public class ItemListViewModel : INotifyPropertyChanged
    {
        public List<ItemListModel> model { get; set; }
        public ItemListViewModel()
        {
            model = new List<ItemListModel> {
                new ItemListModel{ItemName = "Water Bottle", Source = "waterbottle.png"},
                 new ItemListModel{ItemName = "Ice Cream", Source = "icecream.png"},
                  new ItemListModel{ItemName = "Soft Drinks", Source = "softdrink.png"},
            };
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
