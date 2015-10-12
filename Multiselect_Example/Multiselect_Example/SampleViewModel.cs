using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multiselect_Example
{
    public class SampleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<SampleListviewItem> _items;

        public string Title
        {
            get { return "Multiselect ListView"; }
        }
        public ObservableCollection<SampleListviewItem> Items
        {
            get
            {
                return this._items;
            }
            set
            {
                if (this._items != value)
                {
                    this._items = value;
                    this.OnPropertyChanged("Items");
                }
            }
        }
        /// <summary>
        /// Switch of <c>AutoSelect</c> on the <c>CustomSelectableCell</c> and bind this Command on the cell to use single selection
        /// Command="{Binding Source={x:Reference _contentPage}, Path=BindingContext.CellTappedCommand}"
        /// AutoSelect="False"
        /// </summary>
        public Command CellTappedCommand
        {
            get
            {
                return new Command<CustomSelectableCell>(cell => 
                {
                    var item = (SampleListviewItem)cell.BindingContext;
                    var selectedItem = this._items.SingleOrDefault(i => i.IsSelected);

                    if (selectedItem != null && !selectedItem.Equals(item))
                    {
                        selectedItem.IsSelected = false;
                    }
                    item.IsSelected = !item.IsSelected;
                });
            }
        }
        public Command DeleteSelectedCommand
        {
            get
            {
                return new Command(() => 
                {
                    // There seems to be a bug, need to reset datasource
                    var tempCollection = new ObservableCollection<SampleListviewItem>(this._items);

                    SampleListviewItem item;

                    while ((item = tempCollection.FirstOrDefault(i => i.IsSelected)) != null)
                    {
                        tempCollection.Remove(item);
                    }
                    this.Items = tempCollection;
                });
            }
        }
        public SampleViewModel()
        {
            this.Items = new ObservableCollection<SampleListviewItem>();

            for (int i = 1; i <= 20; i++)
            {
                this._items.Add(new SampleListviewItem { Text = string.Format("Item #{0}", i) });
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var ev = this.PropertyChanged;

            if (ev != null)
            {
                ev(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
