using System;
using System.Collections.Generic;
using FlickrPhotoBook.Service;
using FlickrPhotoBook.Service.Core;
using FlickrPhotoBook.ViewModel;

namespace FlickrPhotoBook.Ux
{
    public partial class PhotoInfoPage : ContentPage
    {
        #region auto-properties

        private PhotoInfoViewModel OwnContext { get; }
        private DisplayPhoto PassedPhoto { get; set; }

        #endregion

        #region ctor(s)

        public PhotoInfoPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            if (DeviceInfo.Platform == DevicePlatform.Android) ActuateSafeArea();

            OwnContext = App.ServiceContainer.GetService<PhotoInfoViewModel>()!;
            BindingContext = OwnContext;
        }

        #endregion

        #region access methods

        public void Initialize(DisplayPhoto photo)
        {
            PassedPhoto = photo;
        }

        #endregion

        #region overrides

        protected override void OnAppearing()
        {
            base.OnAppearing();

            OwnContext.LoadCommand.Execute(PassedPhoto);
        }

        #endregion

        #region helper methods

        private void ActuateSafeArea()
        {
            // var service = App.Container.Resolve<SafeAreaInsetsService>();
            // var safeArea = service.GetSafeAreaInsets();
            // AbsoluteContent.Padding = new Thickness(0, safeArea.Top + 10, 0, safeArea.Bottom + 10);
        }

        #endregion

        #region event handlers

        private void HandleSnackbarTapped(object? sender, TappedEventArgs e)
        {
            OwnContext?.SnackbarTappedCommand?.Execute(null);
        }

        #endregion
        
    }
}

