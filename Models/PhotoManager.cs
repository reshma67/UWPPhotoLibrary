using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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
                photos.Add(new Photo(name, path));
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
    }
}
