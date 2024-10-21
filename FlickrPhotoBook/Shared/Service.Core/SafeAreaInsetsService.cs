using System;
using FlickrPhotoBook.Model;

namespace FlickrPhotoBook.Service.Core
{
    public interface SafeAreaInsetsService
    {
        SafeAreaInsets GetSafeAreaInsets(bool getCached = false);
    }
}
