using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DVDDatabase
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        // Data context for the local database
        private static DVDDataContext _dvdDB=null;
        public static DVDDataContext dvdDB
        {
            get
            {
                if (_dvdDB == null)
                    _dvdDB = new DVDDataContext(DVDDataContext.DBConnectionString);
                return _dvdDB;
            }
        }

        public static string ApplicationName = "DVD Database";

        // Define an observable collection property that controls can bind to.
        private ObservableCollection<DVDItem> _dvdItems;

        public ObservableCollection<DVDItem> DVDItems
        {
            get
            {
                return _dvdItems;
            }
            set
            {
                if (_dvdItems != value)
                {
                    _dvdItems = value;
                    //NotifyPropertyChanged("DVDItems");
                }
            }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            // Data context and observable collection are children of the main page.
            this.DataContext = this;

        }



        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedFrom(e);

            // Save changes to the database.
            dvdDB.SubmitChanges();
        }



        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string DisableBackstr = "";
            bool DisableBack=false;
            if (NavigationContext.QueryString.TryGetValue("RemoveLastBack", out DisableBackstr))
            {
                bool Parsed;
                DisableBack = bool.TryParse(DisableBackstr, out Parsed);
                DisableBack = DisableBack && Parsed;
            }

            if (DisableBack)
            {
                NavigationService.RemoveBackEntry();
                NavigationService.RemoveBackEntry();
            }

            // Define the query to gather all of the dvd items.
            var dvdItemsInDB = from DVDItem dvd in dvdDB.DVDItems select dvd;

            // Execute the query and place the results into a collection.
            DVDItems = new ObservableCollection<DVDItem>(dvdItemsInDB);

            // Call the base method.
            base.OnNavigatedTo(e);
        }




        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + ((DVDDatabase.DVDItem) MainListBox.SelectedItem).DVDItemId, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!App.ViewModel.IsDataLoaded)
            //{
            //    App.ViewModel.LoadData();
            //}
        }

        private void SettingsClick(object sender, EventArgs e)
        {

        }

        private void AddDVD_Click(object sender, EventArgs e)
        {
            // Navigate to the new Item page
            NavigationService.Navigate(new Uri("/AddItemPage.xaml", UriKind.Relative));

        }

        private void Search_Click(object sender, EventArgs e)
        {

        }
    }


}