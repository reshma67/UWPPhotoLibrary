﻿using System;
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
        public Boolean IsFavorite { get; set; }
        public string ObjectStateLocation { get; set; }
        public Photo() { }
        
        public Photo(string filename, string filepath, bool isChecked) { 
            FileName = filename;
            FilePath = filepath;
            IsFavorite = isChecked;
        }
    }
}
