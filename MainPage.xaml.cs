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
using Windows.Networking.NetworkOperators;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Text;

namespace UWPPhotoLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<Photo> photos;
        private ObservableCollection<Photo> favphotos;
        private ObservableCollection<ProfileContent> profileContents;



        public MainPage()
        {
            this.InitializeComponent();
            photos = new ObservableCollection<Photo>();
            favphotos = new ObservableCollection<Photo>();
            profileContents = new ObservableCollection<ProfileContent>();
            PhotoManager.GetPhotosFromAssets(photos);
            PhotoManager.GetProfile(profileContents);
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

        void EditPage_Click(object sender, RoutedEventArgs e)
        {
            if (!MenuPopup.IsOpen) { MenuPopup.IsOpen = true; }
        }
        private async void EditDescription_Click(object sender, RoutedEventArgs e)
        {
            TextBox input = new TextBox()
            {
                Height = (double)App.Current.Resources["TextControlThemeMinHeight"],
                PlaceholderText = "Photos of Art I Like, etc."
            };
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Update Description",
                MaxWidth = this.ActualWidth,
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Cancel",
                Content = input
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder Profile = await localFolder.CreateFolderAsync("Profile", CreationCollisionOption.OpenIfExists);
                var descriptionFile = await Profile.CreateFileAsync("description.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
                File.WriteAllText(descriptionFile.Path, input.Text);
                //PhotoManager.GetProfile(profileContents);

                
                
            }
        }
        private void Submit_Popup(object sender, RoutedEventArgs e)
        {
            if (MenuPopup.IsOpen) { MenuPopup.IsOpen = false; }

            //Debug.WriteLine(profileContents.ToList());
            //PhotoManager.GetProfile(profileContents);
            ProfileGridView.ItemsSource = profileContents;
            
           
        }

        private async void EditCoverButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            Debug.WriteLine(file.Name);
            if (file != null)
            {
                

                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var image = new BitmapImage();
                image.SetSource(stream);
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                //StorageFolder Profile = await localFolder.CreateFolderAsync("Profile", CreationCollisionOption.OpenIfExists);
                StorageFolder CoverPhotos = await localFolder.CreateFolderAsync("CoverPhotos", CreationCollisionOption.OpenIfExists);
                string coverFileName = "coverphoto_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                await file.CopyAsync(CoverPhotos, coverFileName, NameCollisionOption.ReplaceExisting);
                PhotoManager.GetProfile(profileContents);

            }
           
        }
        private async void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                Debug.WriteLine(file.Name);
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                //StorageFolder Profile = await localFolder.CreateFolderAsync("Profile", CreationCollisionOption.OpenIfExists);
                StorageFolder ProfilePhotos = await localFolder.CreateFolderAsync("ProfilePhotos", CreationCollisionOption.OpenIfExists);
                string profileFileName = "profilephoto_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                await file.CopyAsync(ProfilePhotos, profileFileName, NameCollisionOption.ReplaceExisting);
                PhotoManager.GetProfile(profileContents);

            }
        }

    }

}
