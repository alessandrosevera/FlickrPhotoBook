using System;
using FlickrPhotoBook.Service.Core;
using Newtonsoft.Json;
using FlickrPhotoBook.Model;

namespace FlickrPhotoBook.Service
{
    public abstract class SharedSafeAreaInsetsService
    {
        #region auto-properties

        protected DeviceIdentifier DeviceIdentifier { get; }

        #endregion

        #region const

        public static readonly string SafeAreaInsetsKeyPrefix = "AppSafeAreaInsets*";

        #endregion

        #region ctor(s)

        public SharedSafeAreaInsetsService(DeviceIdentifier deviceIdentifier)
        {
            DeviceIdentifier = deviceIdentifier;
        }

        #endregion

        #region access methods

        public SafeAreaInsets? GetValue()
        {
            SafeAreaInsets? insets = null;

            var key = $"{SafeAreaInsetsKeyPrefix}{DeviceIdentifier.DeviceId}";
            var json = Preferences.Get(key, null);
            if (!String.IsNullOrEmpty(json))
            {
                insets = JsonConvert.DeserializeObject<SafeAreaInsets>(json);
            }

            return insets;

        }

        public void SetValue(SafeAreaInsets insets)
        {
            var key = $"{SafeAreaInsetsKeyPrefix}{DeviceIdentifier.DeviceId}";
            var json = JsonConvert.SerializeObject(insets);
            Preferences.Set(key, json);
        }

        #endregion
    }
}
