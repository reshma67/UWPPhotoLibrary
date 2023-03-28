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

            //collection holding the photos
            photos = new ObservableCollection<Photo>();
            
            //collection holding the favorite photos
            favphotos = new ObservableCollection<Photo>();
            
            //Profile information holding the details of profile photo, cover photo descriptions
            profileContents = new ObservableCollection<ProfileContent>();

            //Funcction to render the photos to gallery
            PhotoManager.GetPhotosFromAssets(photos);

            //Function to render the Profile Photo, cover Photo and description
            PhotoManager.GetProfile(profileContents);
        }

        private void PhotoView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        //Check box click function to add the photo to favourites list
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


        //Home button for displaying all the available Photos
        private void homebutton_Click(object sender, RoutedEventArgs e)
        {
            PhotoManager.GetPhotosFromAssets(photos);
            PhotoView.ItemsSource = photos;
        }

        //Favorite button for displaying favorite photos
        private void favoritebutton_Click(object sender, RoutedEventArgs e)
        {
            PhotoManager.GetFavoritePhotos(photos, favphotos);
            PhotoView.ItemsSource= favphotos;
        }

        //Function for  unchecking the photo and moving out of the favorite collection
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

        //Function for adding a new photo to the gallery
        private async void Add_Photo_Button(object sender, RoutedEventArgs e)
        {
            await PhotoManager.AddPhotos(photos);
            PhotoManager.GetPhotosFromAssets(photos);
            PhotoView.ItemsSource = photos;

        }

   
        //Function for toggling the pop up to zoomin and zoom out the photo
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

        //Function for opening the popup for editing the profile information
        void EditPage_Click(object sender, RoutedEventArgs e)
        {
            if (!MenuPopup.IsOpen) { MenuPopup.IsOpen = true; }
        }

        //Function for updating the photo description
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
                
            }
        }

        //Function for closing the pop up
        private void Submit_Popup(object sender, RoutedEventArgs e)
        {
            if (MenuPopup.IsOpen) { MenuPopup.IsOpen = false; }
            ProfileGridView.ItemsSource = profileContents;
            
           
        }

        //Function for updating the coverphoto
        private async void EditCoverButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                

                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var image = new BitmapImage();
                image.SetSource(stream);
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder CoverPhotos = await localFolder.CreateFolderAsync("CoverPhotos", CreationCollisionOption.OpenIfExists);
                string coverFileName = "coverphoto_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                await file.CopyAsync(CoverPhotos, coverFileName, NameCollisionOption.ReplaceExisting);
                PhotoManager.GetProfile(profileContents);

            }
           
        }

        // Function for updating the profile photo
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
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder ProfilePhotos = await localFolder.CreateFolderAsync("ProfilePhotos", CreationCollisionOption.OpenIfExists);
                string profileFileName = "profilephoto_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                await file.CopyAsync(ProfilePhotos, profileFileName, NameCollisionOption.ReplaceExisting);
                PhotoManager.GetProfile(profileContents);

            }
        }

    }

}
