using System;
using System.Threading.Tasks;
using Flurl.Http;
using FlickrPhotoBook.Model;

namespace FlickrPhotoBook.Extensions
{
    public static class CallExceptionExtensions
    {
        #region access methods

        public static bool IsTokenExpiredExcption(this Exception exception)
        {
            var isTokenExpiredExcption = false;

            if (exception is Flurl.Http.FlurlHttpException httpException)
            {
                isTokenExpiredExcption = httpException.Call.Response?.StatusCode == (int)System.Net.HttpStatusCode.Unauthorized;
            }

            return isTokenExpiredExcption;
        }

        public static bool IsHttpStatusCodeException(this Exception exception, System.Net.HttpStatusCode httpStatusCode)
        {
            var isHttpStatusCodeException = false;

            if (exception is Flurl.Http.FlurlHttpException httpException)
            {
                isHttpStatusCodeException = httpException.Call.Response.StatusCode == (int)httpStatusCode;
            }

            return isHttpStatusCodeException;
        }

        public static CallResult ToCallStatus(this Exception ex)
        {
            var result = CallResult.Unknown;
            if (ex is FlurlHttpException fhex)
            {

                if (fhex.Call.Response?.StatusCode is null)
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        result = CallResult.ServerUnreachable;
                    }
                    else
                    {
                        result = CallResult.NoConnection;
                    }
                }
                else switch (fhex.Call.Response.StatusCode)
                    {
                        case (int)System.Net.HttpStatusCode.NotFound:
                            result = CallResult.NotFound;
                            break;
                        case (int)System.Net.HttpStatusCode.Unauthorized:
                            result = CallResult.Unauthorized;
                            break;
                        case (int)System.Net.HttpStatusCode.Conflict:
                            result = CallResult.Conflict;
                            break;
                        case (int)System.Net.HttpStatusCode.BadRequest:
                            result = CallResult.BadRequest;
                            break;
                        case (int)System.Net.HttpStatusCode.Forbidden:
                            result = CallResult.Forbidden;
                            break;
                        default:
                            result = CallResult.ServerError;
                            break;
                    }
            }
            else
            {
                result = CallResult.ServerError;
            }

            return result;
        }

        public static async Task<string> GetErrorResponse(this Exception ex)
        {
            string errorResponse = null;
            if (ex is FlurlHttpException fhex)
            {
                errorResponse = await fhex.GetResponseStringAsync();
            }

            return errorResponse;
        }

        #endregion
    }
}