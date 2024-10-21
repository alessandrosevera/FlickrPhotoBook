using System;
namespace FlickrPhotoBook.Model
{
    public enum CallResult
    {
        Unknown,
        Ok,
        ServerError,
        Unauthorized,
        NoConnection,
        ServerUnreachable,
        Expired,
        Conflict,
        Forbidden,
        NotFound,
        BadRequest,
        Gone
    }
}
