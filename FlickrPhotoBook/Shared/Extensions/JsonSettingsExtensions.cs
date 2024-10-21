using Newtonsoft.Json;

namespace FlickrPhotoBook.Extensions;

public class JsonSettingsExtensions
{
    public static JsonSerializerSettings DefaultSettings { get; } = new JsonSerializerSettings
    {
        MissingMemberHandling = MissingMemberHandling.Ignore
    };
}