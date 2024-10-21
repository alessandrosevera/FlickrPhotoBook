using System;
using System.Collections.Generic;
using FlickrPhotoBook;
using FlickrPhotoBook.Service;
using FlickrPhotoBook.Service.Core;
using FlickrPhotoBook.ViewModel;

namespace FlickrPhotoBook.Ux
{
    public partial class RootPage : ContentPage
    {
        #region auto-properties

        private bool WasInitialized { get; set; }
        private PhotoListViewModel OwnContext { get; }

        #endregion

        #region ctor(s)

        public RootPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            if (DeviceInfo.Platform == DevicePlatform.Android) ActuateSafeArea();

            OwnContext = App.ServiceContainer.GetService<PhotoListViewModel>()!;
            BindingContext = OwnContext;
        }

        #endregion

        #region overrides

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!WasInitialized)
            {
                WasInitialized = true;
                OwnContext.FirstLoadCommand.Execute(null);
            }
            
            _ = ActuateRefreshAndCollectionViewPlatformDimensionFixes();
        }

        private async Task ActuateRefreshAndCollectionViewPlatformDimensionFixes()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            while (this.CollectionViewFixer.Height == -1) await Task.Delay(250);

#if IOS
            PhotosCollectionView.HeightRequest = PhotosCollectionView.MinimumHeightRequest = this.CollectionViewFixer.Height;
#elif ANDROID
            PhotosRefreshView.HeightRequest = PhotosRefreshView.MinimumHeightRequest =
                PhotosCollectionView.HeightRequest = PhotosCollectionView.MinimumHeightRequest = this.CollectionViewFixer.Height;
#endif
        }
        
        #endregion

        #region helper methods

        private void ActuateSafeArea()
        {
            // var service = App.Container.Resolve<SafeAreaInsetsService>();
            // var safeArea = service.GetSafeAreaInsets();
            // GridContent.Padding = new Thickness(0, safeArea.Top + 10, 0, safeArea.Bottom + 10);
        }

        #endregion

        #region event handlers

        private void HandleSearchTextChanged(object? sender, TextChangedEventArgs e)
        {
            OwnContext.SearchTextChangedCommand.Execute(e.NewTextValue);
        }

        private void HandlePhotoTapped(object? sender, System.EventArgs e)
        {
            if (e is TappedEventArgs args)
            {
                OwnContext.TapCommand.Execute(args.Parameter);
            }
        }
        #endregion
    }
}

