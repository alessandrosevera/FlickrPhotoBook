using System;
namespace FlickrPhotoBook.Dto
{
    public class DtoPhotoCollectionResult
    {
        #region auto-properties

        public DtoPhotoCollection Photos { get; }
        public string Stat { get; }

        #endregion

        #region ctor(s)

        public DtoPhotoCollectionResult(DtoPhotoCollection photos, string stat)
        {
            Photos = photos;
            Stat = stat;
        }

        #endregion
    }
}

