using System;
using Microsoft.Maui.Controls;

namespace FlickrPhotoBook.Ux.Controls
{
    public partial class BorderlessEntry : Entry
    {
        #region nested classes

        public class SizeChangedEventArgs : EventArgs
        {
            public double Width { get; }
            public double Height { get; }

            public SizeChangedEventArgs(double width, double height)
            {
                Width = width;
                Height = height;
            }
        }

        #endregion
        
        #region bindable properties

        public static BindableProperty PlaceholderFontFamilyProperty = BindableProperty.Create(nameof(PlaceholderFontFamily), typeof(string), typeof(BorderlessEntry), defaultValue: null, propertyChanged: (bindable, oldVal, newVal) =>
        {
        });

        #endregion

        #region properties

        public string PlaceholderFontFamily
        {
            get => (string)GetValue(PlaceholderFontFamilyProperty);
            set => SetValue(PlaceholderFontFamilyProperty, value);
        }

        #endregion

        #region events

        public event EventHandler<SizeChangedEventArgs> OnSizeChanged; 

        #endregion

        #region event properties

        public event EventHandler<FocusEventArgs> OnFocusChanged;

        #endregion

        #region ctor

        public BorderlessEntry()
        {
            //InitializeComponent();
        }

        #endregion

        #region overrides

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            OnSizeChanged?.Invoke(this, new SizeChangedEventArgs(width, height));
        }

        #endregion

        #region event handler

        public void RaiseFocusChanged(bool isFocused)
        {
            OnFocusChanged?.Invoke(this, new FocusEventArgs(this, isFocused));
        }

        #endregion
    }
}