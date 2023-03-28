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
        public ProfileContent() {
            CoverPhoto = "Assets/Images/Cover-Images/paint-smoke.jpg";
            ProfilePhoto = "Assets/Images/Profile-Images/default-profile-image.png";
            Description = "Add description";

        }
        
        
        
    }
}
