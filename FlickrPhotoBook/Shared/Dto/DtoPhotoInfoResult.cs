using System;
namespace FlickrPhotoBook.Dto
{
    public class DtoPhotoInfoResult
    {
        #region auto-properties

        public DtoPhotoInfo Photo { get; }

        #endregion

        #region ctor(s)

        public DtoPhotoInfoResult(DtoPhotoInfo photo)
        {
            Photo = photo;
        }

        #endregion
    }
}

