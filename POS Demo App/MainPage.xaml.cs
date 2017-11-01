using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace POS_Demo_App
{
    public sealed partial class MainPage : Page
    {
        static ObservableCollection<Menu> menus = new ObservableCollection<Menu>();
        static ObservableCollection<OrderList> orderListInfo = new ObservableCollection<OrderList>();
        static ObservableCollection<TotalAmount> sum = new ObservableCollection<TotalAmount>();

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new OrderList();
            
            CloudStorageAccount account = new CloudStorageAccount(new StorageCredentials("SA_NAME", "SA_KEY"),true);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("CONTAINER_NAME");

            //Listing binded images and information from blob container  @pwcasdf
            ListBlobsSegmentedInFlatListing(container);

            MenuGridView.ItemsSource = menus;
        }
        
        async public static Task ListBlobsSegmentedInFlatListing(CloudBlobContainer container)
        {
            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;

            //Call ListBlobsSegmentedAsync and enumerate the result segment returned, while the continuation token is non-null.
            //When the continuation token is null, the last page has been returned and execution can exit the loop.

            string[] separatingChars = { "/", ",", "." };
            string imageUri = "";
            string[] names = { };

            do
            {
                //This overload allows control of the page size. You can return all remaining results by passing null for the maxResults parameter,
                //or by calling a different overload.
                resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);
                foreach (var blobItem in resultSegment.Results)
                {
                    imageUri = blobItem.StorageUri.PrimaryUri.ToString();
                    names = imageUri.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
                    menus.Add(new Menu { Name = names[names.Length-3], Cost = Int32.Parse(names[names.Length-2]), Image = imageUri });
                }
                
                //Get the continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }
            while (continuationToken != null);
        }
        
        private void OnMenuClick(object sender, ItemClickEventArgs e)
        {
            string selectedMenuName = ((Menu)e.ClickedItem).Name;
            int selectedMenuPrice = ((Menu)e.ClickedItem).Cost;

            try
            {
                if(orderListInfo.Count>0)
                {
                    int flag = 0;

                    foreach (var a in orderListInfo)
                    {
                        if (a.OrderMenuName == selectedMenuName)
                        {
                            flag++;
                        }
                    }

                    if (flag == 0)
                    {
                        orderListInfo.Add(new OrderList
                        {
                            OrderMenuName = selectedMenuName,
                            OrderMenuQty = 1,
                            OrderMenuPrice = selectedMenuPrice,
                            sumEachMenu = selectedMenuPrice,
                            UpArrow = new BitmapImage(new Uri("ms-appx:///assets/cute.jpg")),
                            DownArrow = new BitmapImage(new Uri("ms-appx:///assets/cute.jpg")),
                            DeleteList = new BitmapImage(new Uri("ms-appx:///assets/cute.jpg"))
                        });

                        sum.Add(new TotalAmount
                        {
                            menuName = selectedMenuName,
                            totalEach = selectedMenuPrice,
                            totalSum = +selectedMenuPrice
                        });
                    }
                }
                else
                {
                    orderListInfo.Add(new OrderList
                    {
                        OrderMenuName = selectedMenuName,
                        OrderMenuQty = 1,
                        OrderMenuPrice = selectedMenuPrice,
                        sumEachMenu = selectedMenuPrice,
                        UpArrow = new BitmapImage(new Uri("ms-appx:///assets/cute.jpg")),
                        DownArrow = new BitmapImage(new Uri("ms-appx:///assets/cute.jpg")),
                        DeleteList = new BitmapImage(new Uri("ms-appx:///assets/cute.jpg"))
                    });

                    sum.Add(new TotalAmount
                    {
                        menuName = selectedMenuName,
                        totalEach = selectedMenuPrice,
                        totalSum = +selectedMenuPrice
                    });

                    orderListView.ItemsSource = orderListInfo;
                }
            }
            catch
            {

            }
        }

        public void PayButtonClicked(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            foreach(var a in orderListInfo)
            {
                sum = sum + a.sumEachMenu;
            }
            sumTextBlock.Text = sum.ToString();
            orderListInfo.Clear();
        }
    }
}
