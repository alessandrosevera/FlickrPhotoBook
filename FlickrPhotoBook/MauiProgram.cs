using FFImageLoading.Maui;
using FlickrPhotoBook.Ioc;
using Itacall.Handlers;
using Microsoft.Extensions.Logging;
using FlickrPhotoBook.Ux.Controls;

namespace FlickrPhotoBook;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseFFImageLoading()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Montserrat-Black.ttf", "MontserratBlack");
                fonts.AddFont("Montserrat-Bold.ttf", "MontserratBold");
                fonts.AddFont("Montserrat-ExtraBold.ttf", "MontserratExtraBold");
                fonts.AddFont("Montserrat-ExtraLight.ttf", "MontserratExtraLight");
                fonts.AddFont("Montserrat-Light.ttf", "MontserratLight");
                fonts.AddFont("Montserrat-Medium.ttf", "MontserratMedium");
                fonts.AddFont("Montserrat-Regular.ttf", "MontserratRegular");
                fonts.AddFont("Montserrat-SemiBold.ttf", "MontserratSemiBold");
                fonts.AddFont("Montserrat-Thin.ttf", "MontserratThin");
                fonts.AddFont("icomoon.ttf", "CustomIcon");
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(BorderlessEntry), typeof(BorderlessEntryHandler));
            });
        
        builder.Services.ConfigureAppServices();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        
        App.Operate(app.Services);
        
        return app;
    }
}