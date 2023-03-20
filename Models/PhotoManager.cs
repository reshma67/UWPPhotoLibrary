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
        public static async void GetPhotos(ObservableCollection<Photo> photos)
        {
            photos.Clear();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder picturesLibrary = KnownFolders.PicturesLibrary;
            StorageFolder specificFolder = await picturesLibrary.GetFolderAsync("MyPhotos");
            StorageFolder myPhotosFolder = await localFolder.CreateFolderAsync("MyPhotos", CreationCollisionOption.OpenIfExists);

            var files = await specificFolder.GetFilesAsync();
            foreach (var file in files)
            {
                StorageFile newFile = await myPhotosFolder.CreateFileAsync(file.Name, CreationCollisionOption.ReplaceExisting);
                await file.CopyAndReplaceAsync(newFile);
            }
            foreach (var file in await myPhotosFolder.GetFilesAsync())
            {
                var name = file.DisplayName;
                var path = file.Path;
                photos.Add(new Photo(name, path));
                //Debug.WriteLine(name + " " + path);
            }
        }

        public static void getcollectionofPhotos(ObservableCollection<Photo> photos)
        {

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
