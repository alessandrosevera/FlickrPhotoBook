using System;
using PropertyChanged;

namespace FlickrPhotoBook.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class DisplayPhoto
    {
        #region auto-properties

        public string Id { get; }
        public string Secret { get; }
        public string Server { get; }

        public string CompositeUrl { get; }
        public string Title { get; }

        #endregion

        #region ctor(s)

        public DisplayPhoto(string id, string compositeUrl, string secret, string server, string title)
        {
            Id = id;
            CompositeUrl = compositeUrl;
            Title = title;
            Secret = secret;
            Server = server;
        }

        #endregion
    }
}

