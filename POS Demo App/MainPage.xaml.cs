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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace POS_Demo_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            CloudStorageAccount account = new CloudStorageAccount(new StorageCredentials("sa_name", "connection_string"),true);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("images");
            //var blobList = container.

            List <Person> persons = new List<Person>()
            {
                new Person{Name="Anuska Sharama",Age=21,Email="anuska@xyz.com",Image="imagepath"},
                new Person {Name="Asin",Age=26,Email="asin@xyz.com",Image="asin.jpg"},
                new Person{Name="Deepika",Age=25,Email="deepika@xyz.com",Image="deepika.jpg"}
            };
            MenuGridView.ItemsSource = persons;
        }

        private void OnMenuClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
