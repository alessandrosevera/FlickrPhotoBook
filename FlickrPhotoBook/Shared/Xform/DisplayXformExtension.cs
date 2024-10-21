using System;
using System.Collections.Generic;
using System.Linq;
using FlickrPhotoBook.Model;
using FlickrPhotoBook.ViewModel;

namespace FlickrPhotoBook.Xform
{
    public static class DisplayXformExtension
    {
        public static DisplayPhoto ToDisplay(this Photo model)
        {
            string compositeUrl = string.Format("https://live.staticflickr.com/{0}/{1}_{2}_{3}.jpg", model.Server, model.Id, model.Secret, FlickrConstants.SizeSuffix.Small.Large.Suffix);
            return new DisplayPhoto(
                id: model.Id,
                compositeUrl: compositeUrl,
                secret: model.Secret,
                server: model.Server,
                title: model.Title);
        }

        public static IList<DisplayPhoto> ToDisplays(this IList<Photo> models)
        {
            if (models is null) return null;
            return models.Select(model => model.ToDisplay()).ToList();
        }

        public static DisplayPhotoInfo ToInfo(this DisplayPhoto photo)
        {
            string compositeUrl = string.Format("https://live.staticflickr.com/{0}/{1}_{2}_{3}.jpg", photo.Server, photo.Id, photo.Secret, FlickrConstants.SizeSuffix.Large.Small.Suffix);
            return new DisplayPhotoInfo(
                id: photo.Id,
                compositeUrl: compositeUrl,
                description: null,
                ownerUsername: null,
                title: photo.Title);
        }

        public static DisplayPhotoInfo ToDisplay(this PhotoInfo model)
        {
            string compositeUrl = string.Format("https://live.staticflickr.com/{0}/{1}_{2}_{3}.jpg", model.Server, model.Id, model.Secret, FlickrConstants.SizeSuffix.Large.Small.Suffix);
            return new DisplayPhotoInfo(
                id: model.Id,
                compositeUrl: compositeUrl,
                description: model.Description,
                ownerUsername: model.OwnerUsername,
                title: model.Title);
        }
    }
}

