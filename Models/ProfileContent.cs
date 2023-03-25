using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPPhotoLibrary.Models
{
    public class ProfileContent
    {
        public String CoverPhoto { get; set; }
        public String ProfilePhoto { get; set; }
        public String Description { get; set; }
        public ProfileContent() { }
        
        /*public ProfileContent(String coverPhoto, String profilePhoto, String description) {
            CoverPhoto = coverPhoto;
            ProfilePhoto= profilePhoto;
            Description = description; */
        
    }
}
