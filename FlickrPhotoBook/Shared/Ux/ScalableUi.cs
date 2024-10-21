using System;

namespace FlickrPhotoBook.Ux
{
    public static class ScalableUi
    {
        #region const

        private const int PHOTO_PER_ROW = 3;
        private const int COLLECTION_PHOTO_INNER_MARGIN = 5;
        private const int COLLECTION_PHOTO_OUTER_MARGIN = 10;

        #endregion

        #region properties

        public static bool DidScale = false;

        public static double PhotoItemSize;

        #endregion

        #region access methods

        public static void Scale(double deviceHeight, double deviceWidth, DevicePlatform platform)
        {
            if (DidScale) return;

            DidScale = true;

            PhotoItemSize = (deviceWidth - (COLLECTION_PHOTO_OUTER_MARGIN * 2) - (COLLECTION_PHOTO_INNER_MARGIN * (PHOTO_PER_ROW - 1))) / PHOTO_PER_ROW;
        }

        #endregion
    }
}

