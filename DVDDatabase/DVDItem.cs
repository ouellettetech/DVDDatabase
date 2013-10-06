using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DVDDatabase
{

    [Table]
    public class DVDItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _DVDItemId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int DVDItemId
        {
            get
            {
                return _DVDItemId;
            }
            set
            {
                if (_DVDItemId != value)
                {
                    NotifyPropertyChanging("DVDItemId");
                    _DVDItemId = value;
                    NotifyPropertyChanged("DVDItemId");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _dvdItemTitle;

        [Column]
        public string DVDItemTitle
        {
            get
            {
                return _dvdItemTitle;
            }
            set
            {
                if (_dvdItemTitle != value)
                {
                    NotifyPropertyChanging("DVDItemName");
                    _dvdItemTitle = value;
                    NotifyPropertyChanged("DVDItemName");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _dvdItemDesc;

        [Column]
        public string DVDItemDesc
        {
            get
            {
                return _dvdItemDesc;
            }
            set
            {
                if (_dvdItemDesc != value)
                {
                    NotifyPropertyChanging("DVDItemDesc");
                    _dvdItemDesc = value;
                    NotifyPropertyChanged("DVDItemDesc");
                }
            }
        }

        // Define item Genre: private field, public property and database column.
        private string _dvdItemGenre;

        [Column]
        public string DVDItemGenre
        {
            get
            {
                return _dvdItemGenre;
            }
            set
            {
                if (_dvdItemGenre != value)
                {
                    NotifyPropertyChanging("DVDItemGenre");
                    _dvdItemGenre = value;
                    NotifyPropertyChanged("DVDItemGenre");
                }
            }
        }


        // Define item Format: private field, public property and database column.
        //Porbably will make this an enum later....
        private string _dvdItemFormat;

        [Column]
        public string DVDItemFormat
        {
            get
            {
                return _dvdItemFormat;
            }
            set
            {
                if (_dvdItemFormat != value)
                {
                    NotifyPropertyChanging("DVDItemFormat");
                    _dvdItemFormat = value;
                    NotifyPropertyChanged("DVDItemFormat");
                }
            }
        }

        // Define item Format: private field, public property and database column.
        private string _dvdItemISBN;

        [Column]
        public string DVDItemISBN
        {
            get
            {
                return _dvdItemISBN;
            }
            set
            {
                if (_dvdItemFormat != value)
                {
                    NotifyPropertyChanging("DVDItemISBN");
                    _dvdItemISBN = value;
                    NotifyPropertyChanged("DVDItemISBN");
                }
            }
        }
        
        // DefinePicture value: private field, public property and database column.

        [Column(DbType = "IMAGE", Name = "DVDPic")]
        private Byte[] _dvdpicture
        {
            get;
            set;
        }
        
        [Column(Name = "DVDPicHeight")]
        private int _dvdpichight
        {
            get;
            set;
        }

        [Column(Name = "DVDPicWidth")]
        private int _dvdpicwidth
        {
            get;
            set;
        }

        public BitmapImage DVDBitMapPicture
        {
            get
            {

                // load 'imageBytes' byte array to data base ...
                BitmapImage bitmapImage = new BitmapImage();
                if (_dvdpicture == null)
                    return null;
                MemoryStream ms = new MemoryStream(_dvdpicture);
                bitmapImage.SetSource(ms);
                return bitmapImage;
            }
            set
            {

            }
        }

        public byte[] DVDPicture
        {
            get
            {
                return _dvdpicture;
            }
            set
            {
                if (_dvdpicture != value)
                {
                    NotifyPropertyChanging("DVDPicture");
                    _dvdpicture = value;
                    NotifyPropertyChanged("DVDPicture");
                }
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

}
