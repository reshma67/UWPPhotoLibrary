using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWPPhotoLibrary.Models;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPPhotoLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Photo> fixedphotos;
        private ObservableCollection<Photo> photos;
        private ObservableCollection<Photo> favphotos;
        public MainPage()
        {
            this.InitializeComponent();
            fixedphotos = new ObservableCollection<Photo>();
            photos = new ObservableCollection<Photo>();
            favphotos = new ObservableCollection<Photo>();
            PhotoManager.GetPhotos(photos);
        }

        private void PhotoView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null)
            {
                return;
            }
            var photo = checkBox.DataContext as Photo;
            if (photo == null)
            {
                return;
            }
            photo.isFavorite = true;
        }

        private void homebutton_Click(object sender, RoutedEventArgs e)
        {
            PhotoView.ItemsSource = photos;
        }

        private void favoritebutton_Click(object sender, RoutedEventArgs e)
        {
            PhotoManager.getFavoritePhotos(photos, favphotos);
            PhotoView.ItemsSource= favphotos;
        }
    }
}
