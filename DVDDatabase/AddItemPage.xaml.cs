using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using WP7_Barcode_Library;
using com.google.zxing;


namespace DVDDatabase
{
    public partial class NewItem : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;
        byte[] NewDVDImage = null;
        BarcodeFormat[] BarcodeFormatsToUse = new BarcodeFormat[]
        {
            //BarcodeFormat.UPC_A,
            //BarcodeFormat.UPC_E,
            BarcodeFormat.UPC_EAN
        };

        public NewItem()
        {
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            photoChooserTask.ShowCamera = true;
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedFrom(e);

            // Save changes to the database.
            MainPage.dvdDB.SubmitChanges();
        }

        private void SaveNewItem(object sender, EventArgs e)
        {
            // Create a new DVD item based on the text boxes
            DVDItem newDVD = new DVDItem { 
                DVDItemTitle = newItemTitle.Text,
                DVDItemDesc = NewItemDescription.Text,
                DVDItemGenre = NewItemGenre.Text,
                DVDItemISBN = NewItemISBN.Text,
               DVDItemFormat = NewItemFormat.Text,
               DVDPicture = NewDVDImage
            };

            // Add a to-do item to the local database.
            MainPage.dvdDB.DVDItems.InsertOnSubmit(newDVD);


           // Save new Item to the DB
            NavigationService.Navigate(new Uri("/MainPage.xaml?RemoveLastBack=True", UriKind.Relative));
        }

        private void CancelNewItem(object sender, EventArgs e)
        {
            // Possibly Clear all text boxes???

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/MainPage.xaml?RemoveLastBack=True", UriKind.Relative));
        }

        private void addphoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                photoChooserTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("An error occurred.");
            }
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                NewDVDImage = new byte[e.ChosenPhoto.Length];
                e.ChosenPhoto.Read(NewDVDImage, 0, NewDVDImage.Length);
                BitmapImage bitmapImage = new BitmapImage();
                MemoryStream ms = new MemoryStream(NewDVDImage);
                bitmapImage.SetSource(ms);
                 
                //MessageBox.Show(e.ChosenPhoto.Length.ToString());
                MessageBoxResult m = MessageBox.Show("Does this picture have a barcode?", "Proccess Picture", MessageBoxButton.OKCancel);
                if (m == MessageBoxResult.OK)
                {
                    GetBarcode(bitmapImage);
                }
                else
                {
                //Code to display the photo on the page in an image control named myImage.
                 addDVDImage.Source = bitmapImage;
                }
            }
        }

        void GetBarcode(BitmapImage e)
        {
            WP7BarcodeManager.ScanMode = BarcodeFormat.UPC_EAN;
            ErrorText.Text = "";
            NewItemISBN.Text = "";
            WP7BarcodeManager.ScanBarcode(e, new Action<BarcodeCaptureResult>(Barcode_Results));
        }

        int linelength = 40;
        private string SplitError(string ErrorMessage)
        {
            if(ErrorMessage==null)
                return null;
            string newString="";
            int lastendline=0;
            while ((lastendline+linelength) < ErrorMessage.Length)
            {
                int lastspace = ErrorMessage.Substring(lastendline,linelength).LastIndexOf(" ");
                if (lastspace != -1)
                {
                    newString = newString + ErrorMessage.Substring(lastendline, lastspace) + "\n";
                    lastendline = lastendline + lastspace + 1;
                }
            }
            if (lastendline < ErrorMessage.Length)
                newString = newString + ErrorMessage.Substring(lastendline);
            return newString;
        }

        /// <summary>
        /// Callback method that processes results returned by the WP7BarcodeManager. Results are also stored at WP7BarcodeManager.LastCaptureResults.
        /// </summary>
        /// <param name="BCResults">Object that holds all the results of processing the barcode. Results are also stored at WP7BarcodeManager.LastCaptureResults.</param>
        public void Barcode_Results(WP7_Barcode_Library.BarcodeCaptureResult Results)
        {
            //if (Results.BarcodeImage != null)
            //{
            //    imgCapture.Source = Results.BarcodeImage; //Display image
            //}
            //else
            //{
            //    //No image captured
            //}
            if (Results.State == WP7_Barcode_Library.CaptureState.Success)
            {
                NewItemISBN.Text = Results.BarcodeText; //Use results
                ErrorText.Text = Results.BarcodeFormat.ToString();
            }
            else //Error occured
            {
                BarcodeFormat NextFormat= GetNextFormat(Results.BarcodeFormat);
                if (NextFormat != null)
                {
                    WP7BarcodeManager.ScanMode = NextFormat;
                    WP7BarcodeManager.ScanBarcode(Results.BarcodeImage, new Action<BarcodeCaptureResult>(Barcode_Results));
                }
                else
                {
                    ErrorText.Text = SplitError(Results.ErrorMessage);
                }
            }
        }
        public BarcodeFormat GetNextFormat(BarcodeFormat CurrentFormat)
        {
            for (int i = 0; i < BarcodeFormatsToUse.Length; i++)
            {
                if (CurrentFormat == BarcodeFormatsToUse[i])
                    if (++i < BarcodeFormatsToUse.Length)
                    {
                        return BarcodeFormatsToUse[i];
                    }
            }
            return null;
        }
        
    }
}