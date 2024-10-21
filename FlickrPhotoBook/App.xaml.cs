using FlickrPhotoBook.Ux;
using Flurl.Http;

namespace FlickrPhotoBook;

public partial class App : Application
{
    #region auto-properties

    private static App? Instance { get; set; }
    private bool WasOnStartCalled { get; set; }
    private static bool WasOperateCalled { get; set; }
    
    public static IServiceProvider ServiceContainer { get; private set; }
    
    #endregion

    #region ctor(s)

    public App()
    {
        InitializeComponent();

        Instance = this;
        
        MainPage = new PreWorkflowPage();
    }

    #endregion

    #region access methods

    public static void Operate(IServiceProvider serviceProvider)
    {
        ServiceContainer = serviceProvider;
        
        WasOperateCalled = true;
        Instance?.ArbitrateStart();
    }

    #endregion

    #region overrides

    protected override void OnStart()
    {
        base.OnStart();
        
        var displayInfo = DeviceDisplay.MainDisplayInfo;
        var height = displayInfo.Height / displayInfo.Density;
        var width = displayInfo.Width / displayInfo.Density;
        ScalableUi.Scale(height, width, DeviceInfo.Platform);
        
        WasOnStartCalled = true;
        ArbitrateStart();
    }

    #endregion
    
    #region helper methods

    private void ArbitrateStart()
    {
        if (WasOnStartCalled && WasOperateCalled)
        {
            RiseAndShine();
        }
    }

    private void RiseAndShine()
    {
        MainPage = new NavigationPage(new RootPage());
    }

    #endregion
}