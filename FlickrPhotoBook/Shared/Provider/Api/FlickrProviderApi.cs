using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using FlickrPhotoBook.Dto;
using FlickrPhotoBook.Extensions;
using FlickrPhotoBook.Model;
using FlickrPhotoBook.Provider.Core;
using FlickrPhotoBook.Xform;
using Flurl.Http.Newtonsoft;

namespace FlickrPhotoBook.Provider
{
    public class FlickrProviderApi : FlickrProvider
    {
        #region auto-properties

        private ConfigurationProvider ConfigurationProvider { get; }

        #endregion

        #region ctor(s)

        public FlickrProviderApi(ConfigurationProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
        }

        #endregion

        #region FlickrProvider implementation

        public async Task<(CallResult result, PhotoInfo? item)> GetPhotoInfo(string photoId)
        {
            CallResult result = CallResult.Unknown;
            PhotoInfo? item = null;

            var configuration = await ConfigurationProvider.GetAppConfiguration();
            var uri = configuration.ApiRoot
                .SetQueryParam("api_key", configuration.ApiKey)
                .SetQueryParam("format", "json")
                .SetQueryParam("nojsoncallback", true)
                .SetQueryParam("media", "photos")
                .SetQueryParam("method", "flickr.photos.getInfo")
                .SetQueryParam("photo_id", photoId);

            try
            {
                DtoPhotoInfoResult dto = await uri.WithTimeout(TimeSpan.FromSeconds(configuration.ApiCallTimeoutS))
                    .WithSettings(settings => settings.JsonSerializer = new NewtonsoftJsonSerializer(JsonSettingsExtensions.DefaultSettings))
                    .WithHeader("Accept", "application/json")
                    .GetJsonAsync<DtoPhotoInfoResult>();

                result = CallResult.Ok;
                item = dto.Photo.ToModel();
            }
            catch (Exception ex)
            {
                result = ex.ToCallStatus();
            }

            return (result, item);
        }

        public async Task<(CallResult result, IList<Photo> items)> GetRecentPhotos(int page = 1, int resultsPerPage = 21)
        {
            CallResult result = CallResult.Unknown;
            IList<Photo> items = null;

            var configuration = await ConfigurationProvider.GetAppConfiguration();
            var uri = configuration.ApiRoot
                .SetQueryParam("api_key", configuration.ApiKey)
                .SetQueryParam("format", "json")
                .SetQueryParam("nojsoncallback", true)
                .SetQueryParam("media", "photos")
                .SetQueryParam("method", "flickr.photos.getRecent")
                .SetQueryParam("per_page", resultsPerPage)
                .SetQueryParam("page", page);

            try
            {
                DtoPhotoCollectionResult dto = await uri
                    .WithHeader("Accept", "application/json")
                    .GetJsonAsync<DtoPhotoCollectionResult>();

                result = CallResult.Ok;
                items = dto.Photos.ToModels();
            }
            catch (Exception ex)
            {
                result = ex.ToCallStatus();
            }

            return (result, items);
        }

        public async Task<(CallResult result, IList<Photo> items)> SearchPhotos(string text, int page = 1, int resultsPerPage = 21)
        {
            CallResult result = CallResult.Unknown;
            IList<Photo> items = null;

            var configuration = await ConfigurationProvider.GetAppConfiguration();
            var uri = configuration.ApiRoot
                .SetQueryParam("api_key", configuration.ApiKey)
                .SetQueryParam("format", "json")
                .SetQueryParam("nojsoncallback", true)
                .SetQueryParam("media", "photos")
                .SetQueryParam("method", "flickr.photos.search")
                .SetQueryParam("per_page", resultsPerPage)
                .SetQueryParam("page", page)
                .SetQueryParam("text", text);

            try
            {
                DtoPhotoCollectionResult dto = await uri.WithTimeout(TimeSpan.FromSeconds(configuration.ApiCallTimeoutS))
                    .WithHeader("Accept", "application/json")
                    .GetJsonAsync<DtoPhotoCollectionResult>();

                result = CallResult.Ok;
                items = dto.Photos.ToModels();
            }
            catch (Exception ex)
            {
                result = ex.ToCallStatus();
            }

            return (result, items);
        }

        #endregion
    }
}

