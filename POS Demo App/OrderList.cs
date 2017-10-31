using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace POS_Demo_App
{
    class OrderList
    {
        public string OrderMenuName { get; set; }
        public int OrderMenuQty { get; set; }
        public int OrderMenuPrice { get; set; }
        public BitmapImage UpArrow { get; set; }
        public BitmapImage DownArrow { get; set; }
        public BitmapImage ExitImage { get; set; }

        public ICommand upButtonClicked;
        public ICommand UpButtonClicked
        {
            get
            {
                return upButtonClicked ?? (upButtonClicked = new CommandHandler(() => incQty(), true));
            }
        }

        public void incQty()
        {
            
            this.OrderMenuQty++;
            
        }
    }
}
