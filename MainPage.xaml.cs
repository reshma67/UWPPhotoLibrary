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
using System.Diagnostics;

namespace UWPPhotoLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Photo> photos;
        private ObservableCollection<Photo> favphotos;
        public MainPage()
        {
            this.InitializeComponent();
            photos = new ObservableCollection<Photo>();
            favphotos = new ObservableCollection<Photo>();
            PhotoManager.GetPhotosFromAssets(photos);
        }

        private void PhotoView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var photo = checkBox.DataContext as Photo;
            if (checkBox == null)
            {
                return;
            }  
            if (photo == null)
            {
                return;
            }
            if(checkBox.IsChecked == true)
            {
                photo.IsFavorite = true;
                File.WriteAllText(photo.ObjectStateLocation, "true");
            }
        }

        private void homebutton_Click(object sender, RoutedEventArgs e)
        {
            PhotoManager.GetPhotosFromAssets(photos);
            PhotoView.ItemsSource = photos;
        }

        private void favoritebutton_Click(object sender, RoutedEventArgs e)
        {
            PhotoManager.GetFavoritePhotos(photos, favphotos);
            PhotoView.ItemsSource= favphotos;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var photo = checkBox.DataContext as Photo;
            if (photo == null)
            {
                return;
            }
            photo.IsFavorite = false;
            try
            {
                File.WriteAllText(photo.ObjectStateLocation, "false");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void Add_Photo_Button(object sender, RoutedEventArgs e)
        {
            await PhotoManager.AddPhotos(photos);
            PhotoManager.GetPhotosFromAssets(photos);
            PhotoView.ItemsSource = photos;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
         
        }
        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var image = (sender as Image);
            if (image != null)
            {
                if (photoPopup.IsOpen == false)
                {
                    photoPopup.Tag = image.Source;
                    photoPopup.IsOpen = true;
                }
                else
                {
                    photoPopup.IsOpen = false;

                }
            }
        }

    }

}
