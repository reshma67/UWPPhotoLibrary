using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPPhotoLibrary.Models
{
    internal class Photo
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Boolean isFavorite { get; set; }
        public Photo(string filename, string filepath) { 
            FileName = filename;
            FilePath = filepath;
            isFavorite = false;
        }
    }
}
