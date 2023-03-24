using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPPhotoLibrary.Models
{
    internal static class PhotoManager
    {
        
        public static async void GetPhotosFromAssets(ObservableCollection<Photo> photos)
        {
            photos.Clear();
            StorageFolder assetsFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFolder imageFolder = await assetsFolder.GetFolderAsync("Images");
            StorageFolder photosFolder = await imageFolder.GetFolderAsync("Photos");
            IReadOnlyList<StorageFile> fileList = await photosFolder.GetFilesAsync();
            foreach (var file in fileList)
            {
                var name = file.DisplayName;
                var path = file.Path;
                bool isCheck = false;
                photos.Add(new Photo(name, path, isCheck));
                //Debug.WriteLine(name + " " + path);
            }
        }

        public static void getFavoritePhotos(ObservableCollection<Photo> photos, ObservableCollection<Photo> favphotos)
        {
            var photolist = new List<Photo>();
            foreach (var photo in photos)
            {
                if (photo.isFavorite == true) photolist.Add(photo);
            }
            photolist.ForEach(photo => favphotos.Add(photo));
        }
       /* public async static void storeInformation(ProfileContent profileContent) 
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
               // profileImage.ImageSource = image;
            }
            else
            {
                //
            }
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder profilePhotos = await localFolder.CreateFolderAsync("ProfilePhotos", CreationCollisionOption.OpenIfExists);
            await file.CopyAsync(profilePhotos, file.Name, NameCollisionOption.ReplaceExisting);
        } */
    }
}
