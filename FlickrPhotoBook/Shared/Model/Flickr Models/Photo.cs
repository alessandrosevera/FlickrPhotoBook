using System;
namespace FlickrPhotoBook.Model
{
    public class Photo
    {
        #region auto-properties

        public string Id { get; }
        public string Owner { get; }
        public string Secret { get; }
        public string Server { get; }
        public string Title { get; }

        #endregion

        #region ctor(s)

        public Photo(string id, string owner, string secret, string server, string title)
        {
            Id = id;
            Owner = owner;
            Secret = secret;
            Server = server;
            Title = title;
        }

        #endregion
    }
}

