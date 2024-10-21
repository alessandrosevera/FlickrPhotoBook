using System;
using System.Collections.Generic;
using System.Linq;
using FlickrPhotoBook.Dto;
using FlickrPhotoBook.Model;

namespace FlickrPhotoBook.Xform
{
    public static class ModelXformExtension
    {
        #region access methods

        public static IList<Photo> ToModels(this DtoPhotoCollection dtoCollection)
        {
            if (dtoCollection.Photo is null) return null;
            return dtoCollection.Photo.Select(dto => dto.ToModel()).ToList();
        }

        public static Photo ToModel(this DtoPhoto dto)
        {
            return new Photo(
                id: dto.Id,
                owner: dto.Owner,
                secret: dto.Secret,
                server: dto.Server,
                title: dto.Title);
        }

        public static PhotoInfo ToModel(this DtoPhotoInfo dto)
        {
            return new PhotoInfo(
                id: dto.Id,
                secret: dto.Secret,
                server: dto.Server,
                originalSecret: dto.OriginalSecret,
                originalFormat: dto.OriginalFormat,
                title: dto.Title.Content,
                description: dto.Description.Content,
                ownerUsername: dto.Owner.Username,
                ownerRealname: dto.Owner.Realname);
        }

        #endregion
    }
}

