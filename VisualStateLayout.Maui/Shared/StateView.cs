﻿using Microsoft.Maui.Controls;


namespace VisualStateLayout
{
    public class StateView : ContentView
    {
        public static readonly BindableProperty StateKeyProperty = BindableProperty.Create(nameof(StateKey), typeof(State), typeof(StateView), default(State));
        public static readonly BindableProperty CustomStateKeyProperty = BindableProperty.Create(nameof(CustomStateKey), typeof(string), typeof(StateView), default(string));
        public static readonly BindableProperty RepeatCountProperty = BindableProperty.Create(nameof(RepeatCount), typeof(int), typeof(StateView), 1);
        public static readonly BindableProperty RepeatTemplateProperty = BindableProperty.Create(nameof(RepeatTemplate), typeof(DataTemplate), typeof(StateView), null);

        public State StateKey
        {
            get { return (State)GetValue(StateKeyProperty); }
            set { SetValue(StateKeyProperty, value); }
        }

        public string CustomStateKey
        {
            get { return (string)GetValue(CustomStateKeyProperty); }
            set { SetValue(CustomStateKeyProperty, value); }
        }

        public int RepeatCount
        {
            get { return (int)GetValue(RepeatCountProperty); }
            set { SetValue(RepeatCountProperty, value); }
        }

        public DataTemplate RepeatTemplate
        {
            get { return (DataTemplate)GetValue(RepeatTemplateProperty); }
            set { SetValue(RepeatTemplateProperty, value); }
        }
    }
}
