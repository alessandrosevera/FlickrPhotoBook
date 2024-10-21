using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FlickrPhotoBook.Dto;
using FlickrPhotoBook.Model;
using FlickrPhotoBook.Provider.Core;
using FlickrPhotoBook.Xform;
using PropertyChanged;

namespace FlickrPhotoBook.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class PhotoInfoViewModel
    {
        #region auto-properties

        private FlickrProvider FlickrProvider { get; }
        private bool IsActing { get; set; }

        public DisplayPhotoInfo Info { get; private set; }
        public bool IsLoading { get; set; }

        public string ErrorSnackbarMessage { get; private set; }
        public bool IsErrorSnackbarVisible { get; private set; }

        public ICommand SnackbarTappedCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand BackCommand { get; }

        #endregion

        #region ctor(s)

        public PhotoInfoViewModel(FlickrProvider flickrProvider)
        {
            FlickrProvider = flickrProvider;

            Info = new DisplayPhotoInfo();

            SnackbarTappedCommand = new Command(() => IsErrorSnackbarVisible = false);
            LoadCommand = new Command<DisplayPhoto>(async (DisplayPhoto photo) => await OnLoad(photo));
            BackCommand = new Command(async () => await OnBack());

            IsLoading = true;
        }

        #endregion

        #region command delegates

        private async Task OnLoad(DisplayPhoto photo)
        {
            IsLoading = true;

            var passedInfo = photo.ToInfo();
            Info.Title = passedInfo.Title;
            // Info.CompositeUrl = passedInfo.CompositeUrl;
            Info.Id = passedInfo.Id;

            var response = await FlickrProvider.GetPhotoInfo(passedInfo.Id);
            if (response.result == Model.CallResult.Ok)
            {
                if (response.item is not null)
                {
                    var loadedInfo = response.item.ToDisplay();
                    Info.Title = loadedInfo.Title;
                    Info.CompositeUrl = loadedInfo.CompositeUrl;
                    Info.Description = loadedInfo.Description;
                    Info.Id = loadedInfo.Id;
                    Info.OwnerUsername = loadedInfo.OwnerUsername;
                }
                else
                {
                    ErrorSnackbarMessage = "Dati foto non trovati.";
                    IsErrorSnackbarVisible = true;
                }
            }
            else
            {
                switch (response.result)
                {
                    case CallResult.ServerUnreachable:
                    case CallResult.NoConnection:
                        ErrorSnackbarMessage = "Server non raggiungibile.";
                        break;
                    case CallResult.Unauthorized:
                        ErrorSnackbarMessage = "Operazione non consentita.";
                        break;
                    default:
                        ErrorSnackbarMessage = "Si è verificato un errore.";
                        break;
                }
                IsErrorSnackbarVisible = true;

                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    await MainThread.InvokeOnMainThreadAsync(() => IsErrorSnackbarVisible = false);
                });
            }

            IsLoading = false;
        }

        private async Task OnBack()
        {
            await MainThread.InvokeOnMainThreadAsync(async () => await Application.Current!.MainPage!.Navigation.PopModalAsync());
        }

        #endregion
    }
}

