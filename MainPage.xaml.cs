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
        private ObservableCollection<String> list;
        public ProfileContent profileContent { get; set; }


        public MainPage()
        {
            this.InitializeComponent();
            photos = new ObservableCollection<Photo>();
            favphotos = new ObservableCollection<Photo>();
            profileContent = new ProfileContent();
            PhotoManager.GetPhotosFromAssets(photos);
            list = new ObservableCollection<String>();
            PhotoManager.GetProfile(list);
            //Debug.WriteLine(list[0]);
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
                profileDescription.Text = input.Text;
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder Profile = await localFolder.CreateFolderAsync("Profile", CreationCollisionOption.OpenIfExists);
                var descriptionFile = await Profile.CreateFileAsync("description.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
                File.WriteAllText(descriptionFile.Path, profileDescription.Text);
            }
        }
        private async void Submit_Popup(object sender, RoutedEventArgs e)
        {
            if (MenuPopup.IsOpen) { MenuPopup.IsOpen = false; }
            var element = sender as FrameworkElement;
            
            
            var profileContent = element?.DataContext as ProfileContent;
            if(profileContent == null)
            {
                profileContent = new ProfileContent();
            }
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder Profile = await localFolder.CreateFolderAsync("Profile", CreationCollisionOption.OpenIfExists);
            var descriptionFile = await Profile.CreateFileAsync("description.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            profileContent.Description = await File.ReadAllTextAsync(descriptionFile.Path, Encoding.UTF8);
            StorageFile coverphoto = await Profile.CreateFileAsync("coverphoto.jpg", CreationCollisionOption.OpenIfExists);
            profileContent.CoverPhoto = coverphoto.Path;
            StorageFile profilephoto = await Profile.CreateFileAsync("profilephoto.jpg", CreationCollisionOption.OpenIfExists);
            profileContent.ProfilePhoto = profilephoto.Path;
        }

        private async void EditCoverButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder Profile = await localFolder.CreateFolderAsync("Profile", CreationCollisionOption.OpenIfExists);
            await file.CopyAsync(Profile, "coverphoto.jpg", NameCollisionOption.ReplaceExisting);



            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var image = new BitmapImage();
                image.SetSource(stream);
                coverImage.Source = image;
            }
            else
            {
                //
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
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder Profile = await localFolder.CreateFolderAsync("Profile", CreationCollisionOption.OpenIfExists);
            await file.CopyAsync(Profile, "profilephoto.jpg", NameCollisionOption.ReplaceExisting);

            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var image = new BitmapImage();
                image.SetSource(stream);
                profileImage.ImageSource = image;
            }
            else
            {
                //
            }
        }

    }

}
