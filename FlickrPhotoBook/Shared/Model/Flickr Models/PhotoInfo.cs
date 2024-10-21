using System;

namespace FlickrPhotoBook.Model
{
    public class PhotoInfo
    {
        #region auto-properties

        public string Id { get; }
        public string Secret { get; }
        public string Server { get; }
        public string OriginalSecret { get; }
        public string OriginalFormat { get; }
        public string Title { get; }
        public string Description { get; }
        public string OwnerUsername { get; }
        public string OwnerRealname { get; }

        #endregion

        #region ctor(s)

        public PhotoInfo(string id, string secret, string server,
            string originalSecret, string originalFormat, string title, string description,
            string ownerUsername, string ownerRealname)
        {
            Id = id;
            Secret = secret;
            Server = server;
            OriginalSecret = originalSecret;
            OriginalFormat = originalFormat;
            Title = title;
            Description = description;
            OwnerUsername = ownerUsername;
            OwnerRealname = ownerRealname;
        }

        #endregion
    }
}

