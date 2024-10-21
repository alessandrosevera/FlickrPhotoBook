using System;
namespace FlickrPhotoBook.Dto
{
    public class DtoPhotoInfo
    {
        #region auto-properties

        public string Id { get; }
        public string Secret { get; }
        public string Server { get; }
        public int Farm { get; }
        public string OriginalSecret { get; }
        public string OriginalFormat { get; }
        public DtoInfoContent Title { get; }
        public DtoInfoContent Description { get; }
        public string Media { get; }
        public DtoPhotoOwner Owner { get; }

        #endregion

        #region ctor(s)

        public DtoPhotoInfo(string id, string secret, string server, int farm,
            string originalsecret, string originalformat, DtoInfoContent title, DtoInfoContent description,
            string media, DtoPhotoOwner owner)
        {
            Id = id;
            Secret = secret;
            Server = server;
            Farm = farm;
            OriginalSecret = originalsecret;
            OriginalFormat = originalformat;
            Title = title;
            Description = description;
            Media = media;
            Owner = owner;
        }

        #endregion
    }

    public class DtoInfoContent
    {
        #region auto-properties

        [Newtonsoft.Json.JsonProperty("_content")]
        public string Content { get; }

        #endregion

        #region ctor(s)

        public DtoInfoContent(string _content)
        {
            Content = _content;
        }

        #endregion
    }

    public class DtoPhotoOwner
    {
        #region auto-properties

        public string Username { get; }
        public string Realname { get; }

        #endregion

        #region ctor(s)

        public DtoPhotoOwner(string username, string realname)
        {
            Username = username;
            Realname = realname;
        }

        #endregion
    }
}

