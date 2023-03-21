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
                    Debug.WriteLine($"File {objectPath} already exists.");
                    var photo = new Photo(name, path,bool.Parse(fileContents));
                    photos.Add(photo);
                    //StorageFile newFile = await objectFolder.CreateFileAsync(name, CreationCollisionOption.FailIfExists);
                    //File.WriteAllText(photo.objectPath,$"{photo.isFavorite}");
                    photo.objectPath = objectPath;
                    Debug.WriteLine(";;;;;;;;;;;" + bool.Parse(fileContents));
                }
                else
                {
                    Debug.WriteLine($"File {objectPath} does not exist.");
                    var photo = new Photo(name, path, false);
                    photos.Add(photo);
                    //StorageFile newFile = await objectFolder.CreateFileAsync(name, CreationCollisionOption.FailIfExists);
                    //File.WriteAllText(photo.objectPath,$"{photo.isFavorite}");
                    photo.objectPath = objectPath;
                    File.WriteAllText(photo.objectPath, "false");
                }

            }
        }

        public static void getFavoritePhotos(ObservableCollection<Photo> photos, ObservableCollection<Photo> favphotos)
        {
            favphotos.Clear();
            var photolist = new List<Photo>();
            foreach (var photo in photos)
            {
                if (photo.isFavorite == true) photolist.Add(photo);
            }
            photolist.ForEach(photo => favphotos.Add(photo));
        }

        public static async 
        Task
addPhotos(ObservableCollection<Photo> photos)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            Debug.WriteLine("Hereee");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            bool flag = false;
            if (file != null)
            {
                foreach(Photo item in photos)
                {
                    Debug.WriteLine("Filename"+file.Name);
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
                    //StorageFile newFile = await objectFolder.CreateFileAsync(name, CreationCollisionOption.FailIfExists);
                    //File.WriteAllText(photo.objectPath,$"{photo.isFavorite}");
                    photo.objectPath = objectPath;
                    File.WriteAllText(photo.objectPath, "false");
                    Debug.WriteLine("File added");
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFolder myFolder = await localFolder.CreateFolderAsync("MyFolder", CreationCollisionOption.OpenIfExists);
                    await file.CopyAsync(myFolder, file.Name, NameCollisionOption.ReplaceExisting);
                    Debug.WriteLine(objectPath);
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
