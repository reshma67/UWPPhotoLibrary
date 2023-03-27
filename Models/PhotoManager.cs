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
using Windows.UI.Xaml.Media.Imaging;
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
            StorageFolder myPhotos = await localFolder.CreateFolderAsync("MyPhotos", CreationCollisionOption.OpenIfExists);
            IReadOnlyList<StorageFile> fileList = await myPhotos.GetFilesAsync();
            foreach (var file in fileList)
            {
                var objectStateLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{file.DisplayName}.txt");
                if (File.Exists(objectStateLocation))
                {
                    string isChecked = File.ReadAllText(objectStateLocation);
                    var photo = new Photo(file.DisplayName, file.Path,bool.Parse(isChecked));
                    photos.Add(photo);
                    photo.ObjectStateLocation = objectStateLocation;
                }
                else
                {
                    var photo = new Photo(file.DisplayName, file.Path, false);
                    photos.Add(photo);
                    photo.ObjectStateLocation = objectStateLocation;
                    File.WriteAllText(photo.ObjectStateLocation, "false");
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
                    var objectStateLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{file.DisplayName}.txt");
                    var photo = new Photo(file.DisplayName,file.Path, false);
                    photos.Add(photo);
                    photo.ObjectStateLocation = objectStateLocation;
                    File.WriteAllText(photo.ObjectStateLocation, "false");
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFolder myPhotos = await localFolder.CreateFolderAsync("MyPhotos", CreationCollisionOption.OpenIfExists);
                    await file.CopyAsync(myPhotos, file.Name, NameCollisionOption.ReplaceExisting);
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

        public static async void GetProfile(ObservableCollection<ProfileContent> profileContents)
        {
            profileContents.Clear();
            StorageFile coverphoto;
            StorageFile profilephoto;
            var profilePath = "";
            var coverfilePath = "";
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder Profile = await localFolder.CreateFolderAsync("Profile", CreationCollisionOption.OpenIfExists);
            var descriptionFile = await Profile.CreateFileAsync("description.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            var Description = await File.ReadAllTextAsync(descriptionFile.Path, Encoding.UTF8);
            //coverphoto = await Profile.CreateFileAsync("coverphoto.jpg", CreationCollisionOption.OpenIfExists);
            //profilephoto = await Profile.CreateFileAsync("profilephoto.jpg", CreationCollisionOption.OpenIfExists);

            StorageFolder CoverPhotos = await localFolder.CreateFolderAsync("CoverPhotos", CreationCollisionOption.OpenIfExists);
            StorageFolder ProfilePhotos = await localFolder.CreateFolderAsync("ProfilePhotos", CreationCollisionOption.OpenIfExists);

            var coverFiles = await CoverPhotos.GetFilesAsync();
            var profileFiles = await ProfilePhotos.GetFilesAsync();
            var latestProfileFile = profileFiles.OrderByDescending(f => f.DateCreated).FirstOrDefault(f => f.Name.StartsWith("profilephoto_"));
            var latestCoverFile = coverFiles.OrderByDescending(f => f.DateCreated).FirstOrDefault(f => f.Name.StartsWith("coverphoto_"));
            if (latestProfileFile != null)
            {
                profilePath = latestProfileFile.Path;
                // Add profile content to collection
            }
            if (latestCoverFile != null)
            {
                coverfilePath = latestCoverFile.Path;
                // Add profile content to collection
            }

            //var coverPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Profile", "coverphoto.jpg");
            //var profilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Profile", "profilephoto.jpg");

            var profileContent = new ProfileContent();

            if (Description != "")
            {
                profileContent.Description = Description;
            }
            if (latestCoverFile != null)
            {
                profileContent.CoverPhoto = coverfilePath;
            }

            if (latestProfileFile != null)
            {
                profileContent.ProfilePhoto = profilePath;
            }
            
            profileContents.Add(profileContent);

            Debug.WriteLine(profileContent.Description);
            Debug.WriteLine(profileContent.CoverPhoto);
            Debug.WriteLine(profileContent.ProfilePhoto);

        }


    }
}
