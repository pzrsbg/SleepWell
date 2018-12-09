using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SleepWell.Helpers
{
    public static class UrlHelpers
    {
        private static string _roomPhotosFolderRelative = ConfigurationManager.AppSettings["RoomPhotos"];
        public static string RoomPhotosFolderRelative
        {
            get
            {
                return _roomPhotosFolderRelative;
            }
        }

        public static string RoomPhoto(this UrlHelper helper, string imageFilename)
        {
            var imagesFolder = RoomPhotosFolderRelative;
            var path = Path.Combine(imagesFolder, imageFilename);
            var absolutePath = helper.Content(path);
            return absolutePath;
        }
    }
}