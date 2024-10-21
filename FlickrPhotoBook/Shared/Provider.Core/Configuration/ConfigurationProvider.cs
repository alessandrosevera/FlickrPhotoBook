using System;
using System.Threading.Tasks;
using FlickrPhotoBook.Model;

namespace FlickrPhotoBook.Provider.Core
{
    public interface ConfigurationProvider
    {
        Task<Configuration> GetAppConfiguration();
    }
}
