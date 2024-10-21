using System;
using Android.Views;
using Android.Views.InputMethods;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using FlickrPhotoBook.Ux.Controls;

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

    protected override AppCompatEditText CreatePlatformView()
    {
        var platformView = base.CreatePlatformView();

        platformView.Background = null;
        platformView.ImeOptions = (ImeAction)ImeFlags.NoExtractUi;

        ViewGroup.MarginLayoutParams layoutParams =
            new ViewGroup.MarginLayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        if (platformView.LayoutParameters is not null)
        {
            layoutParams = new ViewGroup.MarginLayoutParams(platformView.LayoutParameters);
        }

        layoutParams.SetMargins(0, 0, 0, 0);
        platformView.LayoutParameters = layoutParams;
        platformView.SetPadding(0, 0, 0, 0);

        return platformView;
    }

    #endregion

    #region helper methods

    private static void MapPlaceholderFontFamily(BorderlessEntryHandler arg1, BorderlessEntry arg2)
    {
    }

    #endregion
}