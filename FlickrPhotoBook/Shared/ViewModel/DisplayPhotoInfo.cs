using System;
using PropertyChanged;

namespace FlickrPhotoBook.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class DisplayPhotoInfo
    {
        #region auto-properties

        public string Id { get; set; }
        public string CompositeUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OwnerUsername { get; set; }

        #endregion

        #region ctor(s)

        public DisplayPhotoInfo() { }

        public DisplayPhotoInfo(string id, string compositeUrl, string title,
            string description, string ownerUsername)
        {
            Id = id;
            CompositeUrl = compositeUrl;
            Title = title;
            Description = description;
            OwnerUsername = ownerUsername;
        }

        #endregion
    }
}

