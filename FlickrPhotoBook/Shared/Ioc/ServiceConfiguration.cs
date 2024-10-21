using FlickrPhotoBook.Provider;
using FlickrPhotoBook.Provider.Core;
using FlickrPhotoBook.ViewModel;

namespace FlickrPhotoBook.Ioc;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<FlickrProvider, FlickrProviderApi>()
            .AddSingleton<ConfigurationProvider, ConfigurationProviderLocalFile>()
            .AddTransient<PhotoListViewModel>()
            .AddTransient<PhotoInfoViewModel>();
    }
}