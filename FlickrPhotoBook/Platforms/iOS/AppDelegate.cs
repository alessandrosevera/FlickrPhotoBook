using Foundation;
using UIKit;

namespace FlickrPhotoBook;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        UIApplication.SharedApplication.IdleTimerDisabled = true;
        
        return base.FinishedLaunching(application, launchOptions);
    }
}