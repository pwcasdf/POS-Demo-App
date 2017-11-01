using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace POS_Demo_App
{
    class OrderList : INotifyPropertyChanged
    {
        public string OrderMenuName { get; set; }
        public int OrderMenuQty { get; set; }
        public int OrderMenuPrice { get; set; }
        public int sumEachMenu { get; set; }
        public int sumTotal { get; set; }
        public string Image { get; set; }
        public BitmapImage UpArrow { get; set; }
        public BitmapImage DownArrow { get; set; }
        public BitmapImage DeleteList { get; set; }

        public ICommand upButtonClicked;
        public ICommand downButtonClicked;
        public ICommand deleteButtonClicked;

        public event PropertyChangedEventHandler PropertyChanged;

        static ObservableCollection<TotalAmount> sum = new ObservableCollection<TotalAmount>();

        public ICommand UpButtonClicked
        {
            get
            {
                return upButtonClicked ?? (upButtonClicked = new CommandHandler(() => incQty(), true));
            }
        }

        public ICommand DownButtonClicked
        {
            get
            {
                return downButtonClicked ?? (downButtonClicked = new CommandHandler(() => decQty(), true));
            }
        }

        public ICommand DeleteButtonClicked
        {
            get
            {
                return deleteButtonClicked ?? (deleteButtonClicked = new CommandHandler(() => deleteList(), true));
            }
        }
        
        public void incQty()
        {
            this.OrderMenuQty++;
            this.sumEachMenu = this.OrderMenuPrice * this.OrderMenuQty;
            OnPropertyChanged("OrderMenuQty");
            OnPropertyChanged("sumEachMenu");

            foreach (var a in sum)
            {
                if (a.menuName == this.OrderMenuName)
                {
                    a.totalEach = this.sumEachMenu;
                }
            }

            int totalSum = 0;

            foreach (var a in sum)
            {
                totalSum = +a.totalEach;
                a.totalSum = totalSum;
            }
        }

        public void decQty()
        {
            this.OrderMenuQty--;
            this.sumEachMenu = this.OrderMenuPrice * this.OrderMenuQty;
            OnPropertyChanged("OrderMenuQty");
            OnPropertyChanged("sumEachMenu");

            foreach (var a in sum)
            {
                if (a.menuName == this.OrderMenuName)
                {
                    a.totalEach = this.sumEachMenu;
                }
            }

            int totalSum = 0;

            foreach (var a in sum)
            {
                totalSum = +a.totalEach;
                a.totalSum = totalSum;
            }
        }
                
        public void deleteList()
        {
            //Delete action needs to be added here !!!!!!!!!!!!
        }
        
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
