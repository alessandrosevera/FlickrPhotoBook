using System;
using System.Collections.Generic;

namespace FlickrPhotoBook.Model
{
    public class Configuration
    {
        #region auto-properties

        public string ApiRoot { get; }
        public double ApiCallTimeoutS { get; }
        public string ApiKey { get; }

        #endregion

        #region ctor(s)

        public Configuration(string apiRoot, string apiKey, double apiCallTimeoutS)
        {
            ApiRoot = apiRoot;
            ApiKey = apiKey;
            ApiCallTimeoutS = apiCallTimeoutS;
        }

        #endregion
    }
}
