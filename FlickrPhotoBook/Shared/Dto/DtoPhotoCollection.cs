using System;
namespace FlickrPhotoBook.Dto
{
    public class DtoPhotoCollection
    {
        #region auto-properties

        public int Page { get; }
        public int Pages { get; }
        public int PerPage { get; }
        public int Total { get; }
        public DtoPhoto[] Photo { get; }

        #endregion

        #region ctor(s)

        public DtoPhotoCollection(int page, int pages, int perpage, int total, DtoPhoto[] photo)
        {
            Page = page;
            Pages = pages;
            PerPage = perpage;
            Total = total;
            Photo = photo;
        }

        #endregion
    }
}

