using Foundation;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using FlickrPhotoBook.Ux.Controls;
using UIKit;

namespace Itacall.Handlers;

public class BorderlessEntryHandler : EntryHandler
{
    #region properties

    public static IPropertyMapper<IEntry, BorderlessEntryHandler> OwnMapper =
        new PropertyMapper<BorderlessEntry, BorderlessEntryHandler>(ViewMapper)
        {

            // From Entry.
            [nameof(IEntry.Background)] = MapBackground,
            [nameof(IEntry.CharacterSpacing)] = MapCharacterSpacing,
            [nameof(IEntry.ClearButtonVisibility)] = MapClearButtonVisibility,
            [nameof(IEntry.Font)] = MapFont,
            [nameof(IEntry.IsPassword)] = MapIsPassword,
            [nameof(IEntry.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
            [nameof(IEntry.VerticalTextAlignment)] = MapVerticalTextAlignment,
            [nameof(IEntry.IsReadOnly)] = MapIsReadOnly,
            [nameof(IEntry.IsTextPredictionEnabled)] = MapIsTextPredictionEnabled,
            [nameof(IEntry.Keyboard)] = MapKeyboard,
            [nameof(IEntry.MaxLength)] = MapMaxLength,
            [nameof(IEntry.Placeholder)] = MapPlaceholder,
            [nameof(IEntry.PlaceholderColor)] = MapPlaceholderColor,
            [nameof(IEntry.ReturnType)] = MapReturnType,
            [nameof(IEntry.Text)] = MapText,
            [nameof(IEntry.TextColor)] = MapTextColor,
            [nameof(IEntry.CursorPosition)] = MapCursorPosition,
            [nameof(IEntry.SelectionLength)] = MapSelectionLength,
            // From BorderlessEntry
            [nameof(BorderlessEntry.PlaceholderFontFamily)] = MapPlaceholderFontFamily
        };

    #endregion

    #region ctor(s)

    public BorderlessEntryHandler() : base(OwnMapper)
    {
    }

    #endregion

    #region overrides

    protected override MauiTextField CreatePlatformView()
    {
        var platformView = base.CreatePlatformView();

        platformView.AutocorrectionType = UIKit.UITextAutocorrectionType.No;
        platformView.BorderStyle = UIKit.UITextBorderStyle.None;
        platformView.Layer.BorderWidth = 0;

        return platformView;
    }

    #endregion

    #region helper methods

    private static void MapPlaceholderFontFamily(BorderlessEntryHandler arg1, BorderlessEntry arg2)
    {
        if (!string.IsNullOrEmpty(arg2.PlaceholderFontFamily) && arg2.PlaceholderFontFamily != arg2.FontFamily &&
            !string.IsNullOrEmpty(arg2.Placeholder))
        {
            // var descriptor = new UIFontDescriptor().CreateWithFamily(arg2.PlaceholderFontFamily);
            // var placeholderFont = UIFont.FromDescriptor(descriptor2, (float)arg2.FontSize);

            var placeholderFont = UIFont.FromName(arg2.PlaceholderFontFamily, (float)arg2.FontSize);
            if (placeholderFont is null)
            {
                bool isFontFound =
                    Application.Current.Resources.TryGetValue(arg2.PlaceholderFontFamily, out object fontResource);
                if (isFontFound && fontResource is OnPlatform<string> onPlatformFontNames)
                {
                    string fontName = onPlatformFontNames.Platforms
                        ?.Where(platform => platform.Platform.Contains(nameof(DevicePlatform.iOS)))
                        .Select(platform => platform.Value as string).FirstOrDefault();
                    if (!string.IsNullOrEmpty(fontName))
                    {
                        placeholderFont = UIFont.FromName(fontName, (float)arg2.FontSize);
                    }
                }
            }

            arg1.PlatformView.AttributedPlaceholder = new NSAttributedString(arg2.Placeholder, placeholderFont,
                arg2.PlaceholderColor.ToPlatform());

        }
    }

    #endregion
}