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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace POS_Demo_App
{
    public sealed partial class MainPage : Page
    {
        static ObservableCollection<Person> persons = new ObservableCollection<Person>();

        public MainPage()
        {
            this.InitializeComponent();

            CloudStorageAccount account = new CloudStorageAccount(new StorageCredentials("SA_NAME", "SA_KEY"),true);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("NAME_OF_CONTAINER");
            ListBlobsSegmentedInFlatListing(container);

            MenuGridView.ItemsSource = persons;
        }
        
        async public static Task ListBlobsSegmentedInFlatListing(CloudBlobContainer container)
        {
            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;
            
            //Call ListBlobsSegmentedAsync and enumerate the result segment returned, while the continuation token is non-null.
            //When the continuation token is null, the last page has been returned and execution can exit the loop.
            do
            {
                //This overload allows control of the page size. You can return all remaining results by passing null for the maxResults parameter,
                //or by calling a different overload.
                resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);
                foreach (var blobItem in resultSegment.Results)
                {
                    persons.Add(new Person { Name = "dd", Age = 12, Email = "dd", Image = blobItem.StorageUri.PrimaryUri.ToString() });
                }
                
                //Get the continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }
            while (continuationToken != null);
        }
        
        private void OnMenuClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
