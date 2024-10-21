using System.Collections.ObjectModel;
using System.Windows.Input;
using FlickrPhotoBook.Model;
using FlickrPhotoBook.Provider.Core;
using FlickrPhotoBook.Service;
using FlickrPhotoBook.Ux;
using FlickrPhotoBook.Xform;
using PropertyChanged;
using VisualStateLayout;

namespace FlickrPhotoBook.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class PhotoListViewModel
    {
        #region nested enum

        private enum OriginDataType
        {
            Recent,
            Search
        }

        #endregion

        #region const

        private const int DEBOUNCER_QUERY_MIN_LENGTH = 3;

        #endregion

        #region fields

        private int _page = 1;
        private OriginDataType _dataType;

        #endregion

        #region auto-properties

        private FlickrProvider FlickrProvider { get; }
        private bool IsActing { get; set; }
        private SimpleDebouncer<Photo> TextDebouncer { get; }
        private CancellationTokenSource Cts { get; }

        private bool IsLoadMoreDataDisabled { get; set; }

        public ObservableCollection<DisplayPhoto> Photos { get; private set; }
        public string SearchText { get; set; }
        public bool IsSearchActing { get; private set; }
        public bool IsRefreshing { get; set; }

        public ICommand FirstLoadCommand { get; }
        public ICommand SearchTextChangedCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand LoadMoreDataCommand { get; }
        public ICommand TapCommand { get; }

        public State CurrentState { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region ctor(s)

        public PhotoListViewModel(FlickrProvider flickrProvider)
        {
            FlickrProvider = flickrProvider;

            CurrentState = State.Loading;

            Photos = new ObservableCollection<DisplayPhoto>();

            Cts = new CancellationTokenSource();
            TextDebouncer = new SimpleDebouncer<Photo>(Cts.Token, GetPhotos, SetPhotos, TimeSpan.FromMilliseconds(2000), queryMinLength: DEBOUNCER_QUERY_MIN_LENGTH);

            SearchTextChangedCommand = new Command<string>((string query) => OnTextChanged(query));
            FirstLoadCommand = new Command(async () => await OnFirstLoad());
            RefreshCommand = new Command(async () => await OnRefresh());
            LoadMoreDataCommand = new Command(async () => await OnLoadMoreData());
            TapCommand = new Command<DisplayPhoto>(async (DisplayPhoto tappedPhoto) => await OnTap(tappedPhoto));
        }

        #endregion

        #region helper methods

        private async Task<(CallResult result, IList<Photo> items)> TryAndLoadPhotos()
        {
            (CallResult result, IList<Photo> items) response = default;
            switch (_dataType)
            {
                case OriginDataType.Recent:
                    response = await FlickrProvider.GetRecentPhotos(_page);
                    break;
                case OriginDataType.Search:
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        response = await FlickrProvider.SearchPhotos(SearchText, _page);
                    }
                    else
                    {
                        _dataType = OriginDataType.Recent;
                        response = await FlickrProvider.GetRecentPhotos(_page);
                    }
                    break;
            }

            return response;
        }

        private async Task<IList<Photo>> GetPhotos(string query)
        {
            IsSearchActing = true;
            
            if (CurrentState == State.Error || CurrentState == State.Empty) CurrentState = State.Loading;
            
            _page = 1;
            IsLoadMoreDataDisabled = false;
            (CallResult result, IList<Photo> items) response = default;

            if (string.IsNullOrEmpty(query))
            {
                _dataType = OriginDataType.Recent;
                response = await FlickrProvider.GetRecentPhotos(_page);
            }
            else
            {
                _dataType = OriginDataType.Search;
                response = await FlickrProvider.SearchPhotos(query, _page);
            }

            if (response.result == CallResult.Ok)
            {
                return response.items;
            }
            else
            {
                IsSearchActing = false;
                return null;
            }
        }

        private async Task SetPhotos(IList<Photo> photos)
        {
            if (!string.IsNullOrEmpty(SearchText) && SearchText.Length >= DEBOUNCER_QUERY_MIN_LENGTH)
            {
                var displayables = photos.ToDisplays();
                if (!(displayables is null))
                {
                    Photos.Clear();
                    Photos = new ObservableCollection<DisplayPhoto>(displayables);
                    CurrentState = State.Success;
                }
                else
                {
                    CurrentState = State.Empty;
                }
            }
            else
            {
                if (CurrentState == State.Error || CurrentState == State.Empty)
                {
                    CurrentState = State.Loading;
                }

                _page = 1;
                _dataType = OriginDataType.Recent;

                var response = await TryAndLoadPhotos();
                if (response.result == CallResult.Ok)
                {
                    var displayables = response.items.ToDisplays();
                    if (!(displayables is null))
                    {
                        Photos.Clear();
                        Photos = new ObservableCollection<DisplayPhoto>(displayables);
                        CurrentState = State.Success;
                    }
                    else
                    {
                        CurrentState = State.Empty;
                    }
                }
                else
                {
                    switch (response.result)
                    {
                        case CallResult.ServerUnreachable:
                        case CallResult.NoConnection:
                            ErrorMessage = "Server non raggiungibile.";
                            break;
                        case CallResult.Unauthorized:
                            ErrorMessage = "Operazione non consentita.";
                            break;
                        default:
                            ErrorMessage = "Si è verificato un errore.";
                            break;
                    }
                    CurrentState = State.Error;
                }
            }
            
            IsSearchActing = false;
        }

        #endregion

        #region command delegates

        private async Task OnRefresh()
        {
            if (IsActing) return;
            IsActing = true;
            IsRefreshing = true;

            if (CurrentState == State.Error || CurrentState == State.Empty)
            {
                CurrentState = State.Loading;
            }

            _page = 1;
            IsLoadMoreDataDisabled = false;

            var response = await TryAndLoadPhotos();
            if (response.result == CallResult.Ok)
            {
                var displayables = response.items.ToDisplays();
                if (!(displayables is null))
                {
                    Photos.Clear();
                    Photos = new ObservableCollection<DisplayPhoto>(displayables);
                    CurrentState = State.Success;
                }
                else
                {
                    CurrentState = State.Empty;
                }
            }
            else
            {
                switch (response.result)
                {
                    case CallResult.ServerUnreachable:
                    case CallResult.NoConnection:
                        ErrorMessage = "Server non raggiungibile.";
                        break;
                    case CallResult.Unauthorized:
                        ErrorMessage = "Operazione non consentita.";
                        break;
                    default:
                        ErrorMessage = "Si è verificato un errore.";
                        break;
                }
                CurrentState = State.Error;
            }

            IsActing = false;
            IsRefreshing = false;
        }

        private async Task OnLoadMoreData()
        {
            if (IsActing || IsLoadMoreDataDisabled) return;
            IsActing = true;

            _page++;

            var response = await TryAndLoadPhotos();
            if (response.result == CallResult.Ok)
            {
                var displayables = response.items.ToDisplays();
                if (!(displayables is null))
                {
                    var currentPhotos = Photos.ToList();
                    currentPhotos.AddRange(displayables);
                    Photos = new ObservableCollection<DisplayPhoto>(currentPhotos);
                }
                else
                {
                    _page--;
                    IsLoadMoreDataDisabled = true;
                }
            }
            else
            {
                _page--;
            }

            IsActing = false;
        }

        private async Task OnFirstLoad()
        {
            if (IsActing) return;
            IsActing = true;

            _page = 1;
            _dataType = OriginDataType.Recent;

            var response = await TryAndLoadPhotos();
            if (response.result == CallResult.Ok)
            {
                var displayables = response.items.ToDisplays();
                if (!(displayables is null))
                {
                    Photos.Clear();
                    Photos = new ObservableCollection<DisplayPhoto>(displayables);
                    CurrentState = State.Success;
                }
                else
                {
                    CurrentState = State.Empty;
                }
            }
            else
            {
                switch (response.result)
                {
                    case CallResult.ServerUnreachable:
                    case CallResult.NoConnection:
                        ErrorMessage = "Server non raggiungibile.";
                        break;
                    case CallResult.Unauthorized:
                        ErrorMessage = "Operazione non consentita.";
                        break;
                    default:
                        ErrorMessage = "Si è verificato un errore.";
                        break;
                }
                CurrentState = State.Error;
            }

            IsActing = false;
        }

        private void OnTextChanged(string newTextValue)
        {
            SearchText = newTextValue;
            TextDebouncer.Debounce(SearchText);
        }

        private async Task OnTap(DisplayPhoto tappedPhoto)
        {
            var infoPage = new PhotoInfoPage();
            infoPage.Initialize(tappedPhoto);
            await MainThread.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.Navigation.PushModalAsync(infoPage));
        }

        #endregion
    }
}

