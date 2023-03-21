using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Shapes;
using Path = System.IO.Path;

namespace UWPPhotoLibrary.Models
{
    internal static class PhotoManager
    {
        
        public static async void GetPhotosFromAssets(ObservableCollection<Photo> photos)
        {
            photos.Clear();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder myFolder = await localFolder.CreateFolderAsync("MyFolder", CreationCollisionOption.OpenIfExists);
            IReadOnlyList<StorageFile> fileList = await myFolder.GetFilesAsync();
            foreach (var file in fileList)
            {
                var name = file.DisplayName;
                var path = file.Path;
                var objectPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{name}.txt");
                if (File.Exists(objectPath))
                {
                    string fileContents = File.ReadAllText(objectPath);
                    var photo = new Photo(name, path,bool.Parse(fileContents));
                    photos.Add(photo);
                    photo.ObjectPath = objectPath;
                }
                else
                {
                    var photo = new Photo(name, path, false);
                    photos.Add(photo);
                    photo.ObjectPath = objectPath;
                    File.WriteAllText(photo.ObjectPath, "false");
                }

            }
        }

        public static void GetFavoritePhotos(ObservableCollection<Photo> photos, ObservableCollection<Photo> favphotos)
        {
            favphotos.Clear();
            var photolist = new List<Photo>();
            foreach (var photo in photos)
            {
                if (photo.IsFavorite == true) photolist.Add(photo);
            }
            photolist.ForEach(photo => favphotos.Add(photo));
        }

        public static async 
        Task
AddPhotos(ObservableCollection<Photo> photos)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            bool flag = false;
            if (file != null)
            {
                foreach(Photo item in photos)
                {
                    if (file.Name == item.FileName)
                    {
                        Debug.WriteLine("FileName already exist");
                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                {
                    var name = file.DisplayName;
                    var path = file.Path;
                    var objectPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{name}.txt");
                    var photo = new Photo(name, path, false);
                    photos.Add(photo);
                    photo.ObjectPath = objectPath;
                    File.WriteAllText(photo.ObjectPath, "false");
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFolder myFolder = await localFolder.CreateFolderAsync("MyFolder", CreationCollisionOption.OpenIfExists);
                    await file.CopyAsync(myFolder, file.Name, NameCollisionOption.ReplaceExisting);
                }
                else
                {
                    Debug.WriteLine("FileName already exist");
                }
            }
            else
            {
                Debug.WriteLine("Operation cancelled.");
            }
        }

    }
}
