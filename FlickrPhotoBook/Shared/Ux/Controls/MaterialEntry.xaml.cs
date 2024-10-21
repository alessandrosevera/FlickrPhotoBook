using System;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;

namespace FlickrPhotoBook.Ux.Controls
{
    public partial class MaterialEntry : ContentView
    {
        #region bindable properties

        public static new BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor),
            typeof(Color), typeof(MaterialEntry), defaultValue: Color.FromArgb("#FF000000"),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.EntryWrapper.BackgroundColor = (Color)newVal;
            });

        public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string),
            typeof(MaterialEntry), defaultValue: null, propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                //matEntry.EntryField.Text = (string)newVal;
            });

        public static BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard),
            typeof(MaterialEntry), defaultValue: Keyboard.Default, propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.EntryField.Keyboard = (Keyboard)newVal;
            });


        public static BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string),
            typeof(MaterialEntry), defaultValue: null, propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.Forgotten.FontFamily = (string)newVal;
                matEntry.EntryField.FontFamily = (string)newVal;
                if (string.IsNullOrEmpty(matEntry.PlaceholderFontFamily))
                    matEntry.EntryPlaceholder.FontFamily = (string)newVal;
            });

        public static BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor),
            typeof(Color), typeof(MaterialEntry), defaultValue: Color.FromArgb("#80696C6F"),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.EntryField.PlaceholderColor = (Color)newVal;
                matEntry.Forgotten.TextColor = (Color)newVal;
                matEntry.EntryPlaceholder.TextColor = (Color)newVal;
            });

        public static BindableProperty PlaceholderFontFamilyProperty = BindableProperty.Create(
            nameof(PlaceholderFontFamily), typeof(string), typeof(MaterialEntry), defaultValue: null,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.EntryField.PlaceholderFontFamily = (string)newVal;
                matEntry.EntryPlaceholder.FontFamily = (string)newVal;
            });

        public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder),
            typeof(string), typeof(MaterialEntry), defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldVal, newval) =>
            {
                var matEntry = (MaterialEntry)bindable;
                if (!matEntry.MustMantainPlaceholderOnValueSet) matEntry.EntryField.Placeholder = (string)newval;
                matEntry.EntryPlaceholder.Text = (string)newval;
            });

        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color),
            typeof(MaterialEntry), defaultValue: Color.FromArgb("#FF1D204C"),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.EntryField.TextColor = (Color)newVal;
            });

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize),
            typeof(double), typeof(MaterialEntry), defaultValue: 12.0d, propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.EntryField.FontSize = (double)newVal;
                matEntry.Forgotten.FontSize = (double)newVal;
                matEntry.EntryPlaceholder.FontSize = (double)newVal;
            });

        public static readonly BindableProperty InnerPaddingProperty = BindableProperty.Create(nameof(InnerPadding),
            typeof(Thickness), typeof(MaterialEntry), defaultValue: new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.EntryElementsContainer.Padding = (Thickness)newVal;
            });

        public static readonly BindableProperty ExternalPaddingProperty = BindableProperty.Create(
            nameof(ExternalPadding), typeof(Thickness), typeof(MaterialEntry), defaultValue: new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.Padding = (Thickness)newVal;
            });

        public static readonly BindableProperty ExternalMarginProperty = BindableProperty.Create(nameof(ExternalMargin),
            typeof(Thickness), typeof(MaterialEntry), defaultValue: new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.Margin = (Thickness)newVal;
            });

        public static readonly BindableProperty TextIndentProperty = BindableProperty.Create(nameof(TextIndent),
            typeof(double), typeof(MaterialEntry), defaultValue: 0d, propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                var indent = (double)newVal;
                matEntry.EntryField.Margin = new Thickness(indent, 0, 0, 0);
                matEntry.EntryPlaceholder.Margin = new Thickness(indent, 0, 0, 0);
            });

        public static BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius),
            typeof(CornerRadius), typeof(MaterialEntry), defaultValue: new CornerRadius(0),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;

                var newRadius = matEntry.UseWrappingBorder ? (CornerRadius)newVal : new CornerRadius(0);

                bool isBorderInitialized = matEntry.EntryWrapper.StrokeShape is not null &&
                                           matEntry.EntryWrapper.StrokeShape is RoundRectangle;
                var border = isBorderInitialized
                    ? (RoundRectangle)matEntry.EntryWrapper.StrokeShape
                    : new RoundRectangle();
                border.CornerRadius = newRadius;
                matEntry.EntryWrapper.StrokeShape = border;
            });

        public static BindableProperty UseWrappingBorderProperty = BindableProperty.Create(nameof(UseWrappingBorder),
            typeof(bool), typeof(MaterialEntry), defaultValue: false, propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                bool useBorder = (bool)newVal;
                if (useBorder)
                {
                    matEntry.EntryField.VerticalOptions = LayoutOptions.Center;
                    matEntry.EntryPlaceholder.VerticalOptions = LayoutOptions.Center;
                    matEntry.Line.IsVisible = false;
                    matEntry.AccentLine.IsVisible = false;
                    matEntry.ErrorLine.IsVisible = false;
                    Grid.SetRowSpan(matEntry.Eye, 2);
                    Grid.SetRowSpan(matEntry.EntryField, 2);
                    Grid.SetRowSpan(matEntry.EntryPlaceholder, 2);

                    var border = new RoundRectangle();
                    border.CornerRadius = matEntry.CornerRadius;
                    matEntry.EntryWrapper.StrokeShape = border;

                    matEntry.EntryWrapper.Stroke = matEntry.BorderColor;
                    matEntry.EntryWrapper.StrokeThickness = matEntry.BorderThickness;

                }
                else
                {
                    Grid.SetRowSpan(matEntry.Eye, 0);
                    Grid.SetRowSpan(matEntry.EntryField, 0);
                    Grid.SetRowSpan(matEntry.EntryPlaceholder, 0);
                    matEntry.EntryField.VerticalOptions = LayoutOptions.End;
                    matEntry.EntryPlaceholder.VerticalOptions = LayoutOptions.End;

                    matEntry.EntryWrapper.StrokeShape = null;
                    matEntry.EntryWrapper.StrokeThickness = 0;
                    // matEntry.EntryWrapper.CornerRadius = 0;
                    // matEntry.EntryWrapper.Border = null;

                    matEntry.AdaptElements(matEntry.EntryField.IsFocused);
                }
            });

        public static BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness),
            typeof(int), typeof(MaterialEntry), defaultValue: 0, propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                if (matEntry.UseWrappingBorder)
                {
                    matEntry.EntryWrapper.StrokeThickness = (int)newVal;
                }
            });

        public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color),
            typeof(MaterialEntry), defaultValue: Color.FromArgb("#FF000000"),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                if (matEntry.UseWrappingBorder)
                {
                    matEntry.EntryWrapper.Stroke = (Color)newVal;
                }
            });
        
        public static BindableProperty MustMantainAccentOnValueSetProperty =
            BindableProperty.Create(nameof(MustMantainAccentOnValueSet), typeof(bool), typeof(MaterialEntry),
                defaultValue: false, propertyChanged: (bindable, oldVal, newVal) => { });

        public static BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool),
            typeof(MaterialEntry), defaultValue: false, propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.IsPassword = (bool)newVal;
            });

        public static BindableProperty InputTranslationYOnValueSetProperty =
            BindableProperty.Create(nameof(InputTranslationYOnValueSet), typeof(double), typeof(MaterialEntry),
                defaultValue: 9d, propertyChanged: (bindable, oldVal, newVal) => { });

        public static BindableProperty MustMantainPlaceholderOnValueSetProperty = BindableProperty.Create(
            nameof(MustMantainPlaceholderOnValueSet), typeof(bool), typeof(MaterialEntry), defaultValue: false,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                bool boolVal = (bool)newVal;

                if (boolVal) matEntry.EntryField.Placeholder = string.Empty;
                else matEntry.EntryField.Placeholder = matEntry.Placeholder;
                matEntry.EntryPlaceholder.IsVisible = boolVal;
            });

        public static BindableProperty PlaceholderReductionOnValueSetProperty =
            BindableProperty.Create(nameof(PlaceholderReductionOnValueSet), typeof(int), typeof(MaterialEntry),
                defaultValue: 2, propertyChanged: (bindable, oldVal, newVal) => { });

        public static BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor), typeof(Color),
            typeof(MaterialEntry), defaultValue: Color.FromArgb("#FF8C8C8C"),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.Line.BackgroundColor = matEntry.Line.Color = (Color)newVal;
            });

        public static BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color),
            typeof(MaterialEntry), defaultValue: Color.FromArgb("#1E90FF"),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.AccentLine.BackgroundColor = matEntry.AccentLine.Color = (Color)newVal;
            });

        public static BindableProperty ErrorColorProperty = BindableProperty.Create(nameof(ErrorColor), typeof(Color),
            typeof(MaterialEntry), defaultValue: Color.FromArgb("#FF0000"),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (MaterialEntry)bindable;
                matEntry.ErrorLine.BackgroundColor = matEntry.ErrorLine.Color = (Color)newVal;
            });


        #endregion
        
        #region auto-properties

        public bool IsPassword { get; set; }
        private bool HasEye { get; set; }
        private bool HasForgottenHint { get; set; }
        private Action ActOnForgotten { get; set; }

        private bool IsPasswordReadable { get; set; }
        private bool IsSelfInteraction { get; set; }
        public bool HasError { get; private set; }

        #endregion
        
        #region properties

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public string PlaceholderFontFamily
        {
            get => (string)GetValue(PlaceholderFontFamilyProperty);
            set => SetValue(PlaceholderFontFamilyProperty, value);
        }

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public Thickness InnerPadding
        {
            get => (Thickness)GetValue(InnerPaddingProperty);
            set => SetValue(InnerPaddingProperty, value);
        }

        public Thickness ExternalPadding
        {
            get => (Thickness)GetValue(ExternalPaddingProperty);
            set => SetValue(ExternalPaddingProperty, value);
        }

        public Thickness ExternalMargin
        {
            get => (Thickness)GetValue(ExternalMarginProperty);
            set => SetValue(ExternalMarginProperty, value);
        }

        public double TextIndent
        {
            get => (double)GetValue(TextIndentProperty);
            set => SetValue(TextIndentProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public bool UseWrappingBorder
        {
            get => (bool)GetValue(UseWrappingBorderProperty);
            set => SetValue(UseWrappingBorderProperty, value);
        }

        public int BorderThickness
        {
            get => (int)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
        
        public bool MustMantainAccentOnValueSet
        {
            get => (bool)GetValue(MustMantainAccentOnValueSetProperty);
            set => SetValue(MustMantainAccentOnValueSetProperty, value);
        }

        public double InputTranslationYOnValueSet
        {
            get => (double)GetValue(InputTranslationYOnValueSetProperty);
            set => SetValue(InputTranslationYOnValueSetProperty, value);
        }

        public bool MustMantainPlaceholderOnValueSet
        {
            get => (bool)GetValue(MustMantainPlaceholderOnValueSetProperty);
            set => SetValue(MustMantainPlaceholderOnValueSetProperty, value);
        }

        public int PlaceholderReductionOnValueSet
        {
            get => (int)GetValue(PlaceholderReductionOnValueSetProperty);
            set => SetValue(PlaceholderReductionOnValueSetProperty, value);
        }

        public Color LineColor
        {
            get => (Color)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        public Color AccentColor
        {
            get => (Color)GetValue(AccentColorProperty);
            set => SetValue(AccentColorProperty, value);
        }

        public Color ErrorColor
        {
            get => (Color)GetValue(ErrorColorProperty);
            set => SetValue(ErrorColorProperty, value);
        }

        #endregion

        #region fields

        private double _latestHeightAllocated = -1;
        private double _latestEntryFieldHeightAllocated = -1;

        #endregion

        #region properties

        public new bool IsFocused => EntryField.IsFocused;
        public double EntryHeight => EntryField.Height;

        #endregion

        #region events

        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler<FocusEventArgs> EntryFocused;

        #endregion


        #region ctor(s)

        public MaterialEntry()
        {
            InitializeComponent();

            AdaptElements(false);
            UpdateColumns();

            EntryField.BindingContext = this;
            EntryField.OnSizeChanged += HandleEntryFieldSizeChanged;
        }

        #endregion
        
        #region access methods

        public void Configure()
        {
        }

        public void Initialize(bool isPassword, bool hasForgottenHint,
            Action actOnForgotten, bool hasEye = true)
        {
            IsPassword = isPassword;
            HasForgottenHint = hasForgottenHint;
            ActOnForgotten = actOnForgotten;
            HasEye = hasEye;

            AdaptElements(EntryField.IsFocused);

            UpdateColumns();
        }

        public void SetError(bool hasError)
        {
            HasError = hasError;
            AdaptElements(EntryField.IsFocused);
        }

        public new void Focus()
        {
            EntryField.Focus();
        }

        public new void Unfocus()
        {
            EntryField.Unfocus();
        }

        #endregion


        #region helper methods

        private void UpdateColumns()
        {
            bool canShowForgotten = IsPassword && HasForgottenHint;
            int entryColumnSpan = !canShowForgotten ? 2 : 1;

            Grid.SetColumnSpan(EntryField, entryColumnSpan);
            Grid.SetColumnSpan(EntryPlaceholder, entryColumnSpan);
            Grid.SetColumnSpan(EntryElementsContainer, entryColumnSpan);
            Forgotten.IsVisible = canShowForgotten;
        }

        private void AdaptElements(bool isEntryFocused)
        {
            EntryField.IsPassword = IsPassword && !IsPasswordReadable;
            Eye.Source = null;

            if (IsPassword && HasEye)
            {
                if (IsPasswordReadable)
                {
                    Eye.Source = ImageSource.FromFile("eye_no");
                }
                else
                {
                    Eye.Source = ImageSource.FromFile("eye_yes");
                }
            }

            if (MustMantainPlaceholderOnValueSet)
            {
                if (!string.IsNullOrEmpty(EntryField.Text))
                {
                    if (EntryField.Height != -1)
                    {
                        var entryFieldTranslationY = UseWrappingBorder ? InputTranslationYOnValueSet : 0;
                        var scale = 1 - (PlaceholderReductionOnValueSet / FontSize);

                        if (UseWrappingBorder) EntryField.TranslateTo(0, entryFieldTranslationY);
                        EntryPlaceholder.ScaleTo(scale);
                        EntryPlaceholder.TranslateTo(0, -(EntryField.Height - entryFieldTranslationY));
                    }
                }
                else
                {
                    if (UseWrappingBorder) EntryField.TranslateTo(0, 0);
                    EntryPlaceholder.ScaleTo(1);
                    EntryPlaceholder.TranslateTo(0, 0);
                }
            }

            if (!isEntryFocused)
            {
                if (HasError)
                {
                    if (!UseWrappingBorder)
                    {
                        ErrorLine.IsVisible = true;
                        Line.IsVisible = false;
                    }
                    else
                    {
                        EntryWrapper.Stroke = ErrorColor;
                    }
                }
                else
                {
                    if (!UseWrappingBorder)
                    {
                        ErrorLine.IsVisible = false;
                        Line.IsVisible = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(EntryField.Text) && MustMantainAccentOnValueSet)
                        {
                            EntryWrapper.Stroke = AccentColor;
                        }
                        else
                        {
                            EntryWrapper.Stroke = BorderColor;
                        }
                    }
                }

                AccentLine.IsVisible = false;
                EyeFrame.IsVisible = Eye.IsVisible = false;

                Forgotten.IsVisible = IsPassword && HasForgottenHint;

                if (String.IsNullOrEmpty(EntryField.Text))
                {
                    Forgotten.TextColor = PlaceholderColor;
                }
                else
                {
                    Forgotten.TextColor = AccentColor;
                }
            }
            else
            {
                if (!UseWrappingBorder)
                {
                    Line.IsVisible = false;
                    AccentLine.IsVisible = true;
                    ErrorLine.IsVisible = false;
                }
                else
                {
                    EntryWrapper.Stroke = AccentColor;
                }

                EyeFrame.IsVisible = Eye.IsVisible = IsPassword && HasEye;
                Forgotten.IsVisible = false;

            }
        }

        void RaiseFocused(bool isFocused)
        {
            EventHandler<FocusEventArgs> handler = EntryFocused;
            if (handler != null)
            {
                var e = new FocusEventArgs(this, isFocused);
                handler(this, e);
            }
        }

        #endregion



        #region event handlers

        private async void HandleEntryFieldFocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
        {
            // await Task.Delay(250);

            if (IsSelfInteraction)
            {
                if (!e.IsFocused)
                {
                    EntryField.Focus();
                }
                else
                {
                    IsSelfInteraction = false;
                }
            }
            else
            {
                await MainThread.InvokeOnMainThreadAsync(() => AdaptElements(e.IsFocused));
                RaiseFocused(e.IsFocused);
            }

            if (DeviceInfo.Platform == DevicePlatform.Android)
                EntryField.RaiseFocusChanged(true);
        }

        private async void HandleEntryFieldTextChanged(System.Object sender,
            Microsoft.Maui.Controls.TextChangedEventArgs e)
        {
            await MainThread.InvokeOnMainThreadAsync(() => AdaptElements(EntryField.IsFocused));

            EventHandler<TextChangedEventArgs> handler = TextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        private void HandleEyeTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
        {
            // aggiunto il controllo SE_NON_ANDROID perchè su Android se ti tappa sull'occhio non viene consecutivamente chiamato il FocusOut
            // anzi genera un nuovo "Focus()" quando l'utente effettivamente tappa fuori dalla Entry
            if (DeviceInfo.Platform != DevicePlatform.Android)
                IsSelfInteraction = true;

            IsPasswordReadable = !IsPasswordReadable;
            if (IsPassword && HasEye)
                Eye.Source = IsPasswordReadable ? ImageSource.FromFile("eye_no") : ImageSource.FromFile("eye_yes");
            EntryField.IsPassword = IsPassword && !IsPasswordReadable;

            EntryField.Focus();

            if (DeviceInfo.Platform == DevicePlatform.Android)
                EntryField.RaiseFocusChanged(true);
        }

        private void HandleForgottenTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
        {
            if (!(ActOnForgotten is null))
            {
                ActOnForgotten();
            }
        }

        private void HandleInactiveAreaTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
        {
            EntryField.Focus();
        }
        
        private void HandleEntryFieldSizeChanged(object? sender, BorderlessEntry.SizeChangedEventArgs e)
        {
            if (e.Height != -1 && e.Height != _latestEntryFieldHeightAllocated)
            {
                AdaptElements(IsFocused);
                _latestEntryFieldHeightAllocated = e.Height;
            }
        }

        #endregion

        #region overrides

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (height != -1 && height != _latestHeightAllocated)
            {
                AdaptElements(IsFocused);
                _latestHeightAllocated = height;
            }
        }

        #endregion
    }
}