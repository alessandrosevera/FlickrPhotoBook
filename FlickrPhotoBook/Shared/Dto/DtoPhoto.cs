using System;
namespace FlickrPhotoBook.Dto
{
    public class DtoPhoto
    {
        #region auto-properties

        public string Id { get; }
        public string Owner { get; }
        public string Secret { get; }
        public string Server { get; }
        public int Farm { get; }
        public string Title { get; }
        public int IsPublic { get; }
        public int IsFriend { get; }
        public int IsFamily { get; }

        #endregion

        #region ctor(s)

        public DtoPhoto(string id, string owner, string secret, string server,
            int farm, string title, int ispublic, int isfriend, int isfamily)
        {
            Id = id;
            Owner = owner;
            Secret = secret;
            Server = server;
            Farm = farm;
            Title = title;
            IsPublic = ispublic;
            IsFriend = isfriend;
            IsFamily = isfamily;
        }

        #endregion
    }
}

