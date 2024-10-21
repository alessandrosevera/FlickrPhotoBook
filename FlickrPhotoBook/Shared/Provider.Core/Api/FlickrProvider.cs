using System;
using FlickrPhotoBook.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FlickrPhotoBook.Provider.Core
{
    public interface FlickrProvider
    {
        Task<(CallResult result, IList<Photo> items)> GetRecentPhotos(int page = 1, int resultsPerPage = 21);
        Task<(CallResult result, IList<Photo> items)> SearchPhotos(string text, int page = 1, int resultsPerPage = 21);
        Task<(CallResult result, PhotoInfo? item)> GetPhotoInfo(string photoId);
    }
}

