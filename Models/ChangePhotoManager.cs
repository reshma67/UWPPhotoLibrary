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
    internal class ChangePhotoManager
    {
        /*public static async void GetLocalPhoto(ObservableCollection<Photo> photos) 
        {
            photos.Clear();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder inputPhotos = await localFolder.CreateFolderAsync("InputPhotos", CreationCollisionOption.ReplaceExisting);
            IReadOnlyList<StorageFile> fileList = await inputPhotos.GetFilesAsync();
            foreach (var file in fileList) 
            {
                var objectStateLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{file.DisplayName}.txt");
                
                var photo = new Photo(file.DisplayName, file.Path, false);
                photos.Add(photo);
                photo.ObjectStateLocation = objectStateLocation;
            }
        }
        public static async void PickNewPhoto(ObservableCollection<ProfileContent> photos) 
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
                foreach(ProfileContent item in photos) 
                { 
                    if (file.Name == item.NewFileName) 
                    {
                        Debug.WriteLine("FileName already exists");
                        flag = true;
                        break;
                    }
                }
                if (flag == false) 
                {
                    var objectStateLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{file.DisplayName}.txt");
                    var photo = new ProfileContent(file.DisplayName, file.Path, "");
                    photos.Add(photo);
                    photo.ObjectStateLocation = objectStateLocation;
                    File.WriteAllText(photo.ObjectStateLocation, "false");
                    StorageFolder localFile = ApplicationData.Current.LocalFolder;
                    StorageFolder inputPhotos = await localFile.CreateFolderAsync("InputPhotos", CreationCollisionOption.OpenIfExists);
                    await file.CopyAsync(inputPhotos, file.Name, NameCollisionOption.ReplaceExisting);
                }
                else 
                {
                    Debug.WriteLine("FileName already exists");
                }
            }
            else 
            {
                Debug.WriteLine("Operation Cancelled");
            }

        }

        internal static Task PickNewPhoto(ObservableCollection<Photo> photos)
        {
            throw new NotImplementedException();
        }
    }
}*/
