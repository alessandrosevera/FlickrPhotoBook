using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;


namespace VisualStateLayout
{
    public class StateLayoutController
    {
        private readonly WeakReference<Layout> _layoutWeakReference;
        private bool _layoutIsGrid = false;
        private IList<View> _originalContent;
        private State _previousState = State.None;

        public IList<StateView> StateViews { get; set; }

        public StateLayoutController(Layout layout)
        {
            _layoutWeakReference = new WeakReference<Layout>(layout);
        }

        public async void SwitchToContent(bool animate)
        {
            Layout layout;

            if (!_layoutWeakReference.TryGetTarget(out layout))
            {
                return;
            }

            _previousState = State.None;
            await ChildrenFadeTo(layout, animate, true);

            // Put the original content back in.
            layout.Children.Clear();
            foreach (var item in _originalContent)
            {
                item.Opacity = animate ? 0 : 1;
                layout.Children.Add(item);
            }
            await ChildrenFadeTo(layout, animate, false);
        }

        public void SwitchToTemplate(string customState, bool animate)
        {
            SwitchToTemplate(State.Custom, customState, animate);
        }

        public async void SwitchToTemplate(State state, string customState, bool animate)
        {
            Layout layout;

            if (!_layoutWeakReference.TryGetTarget(out layout))
            {
                return;
            }

            // Put the original content somewhere where we can restore it.
            if (_previousState == State.None)
            {
                _originalContent = new List<View>();

                foreach (var item in layout.Children)
                {
                    if (item is View view) _originalContent.Add(view);
                }
                    
            }

            if (HasTemplateForState(state, customState))
            {
                _previousState = state;

                await ChildrenFadeTo(layout, animate, true);

                layout.Children.Clear();

                var repeatCount = GetRepeatCount(state, customState);

                if (repeatCount == 1)
                {
                    var s = new VerticalStackLayout() { Opacity = animate ? 0 : 1, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Start };

                    if (layout is Grid grid)
                    {
                        if (grid.RowDefinitions.Any())
                            Grid.SetRowSpan(s, grid.RowDefinitions.Count);

                        if (grid.ColumnDefinitions.Any())
                            Grid.SetColumnSpan(s, grid.ColumnDefinitions.Count);

                        layout.Children.Add(s);

                        _layoutIsGrid = true;
                    }

                    var view = CreateItemView(state, customState);

                    if (view != null)
                    {
                        if (_layoutIsGrid)
                            s.Children.Add(view);
                        else
                            layout.Children.Add(view);
                    }
                }
                else
                {
                    var template = GetRepeatTemplate(state, customState);
                    var items = new List<int>();

                    for (int i = 0; i < repeatCount; i++)
                        items.Add(i);

                    var s = new VerticalStackLayout { Opacity = animate ? 0 : 1 };

                    if (layout is Grid grid)
                    {
                        if (grid.RowDefinitions.Any())
                            Grid.SetRowSpan(s, grid.RowDefinitions.Count);

                        if (grid.ColumnDefinitions.Any())
                            Grid.SetColumnSpan(s, grid.ColumnDefinitions.Count);
                    }

                    BindableLayout.SetItemTemplate(s, template);
                    BindableLayout.SetItemsSource(s, items);

                    layout.Children.Add(s);
                }
                await ChildrenFadeTo(layout, animate, false);
            }
        }

        private bool HasTemplateForState(State state, string customState)
        {
            var template = StateViews.FirstOrDefault(x => (x.StateKey == state && state != State.Custom) ||
                            (state == State.Custom && x.CustomStateKey == customState));

            return template != null;
        }

        private int GetRepeatCount(State state, string customState)
        {
            var template = StateViews.FirstOrDefault(x => (x.StateKey == state && state != State.Custom) ||
                           (state == State.Custom && x.CustomStateKey == customState));

            if (template != null)
            {
                return template.RepeatCount;
            }

            return 1;
        }

        private DataTemplate GetRepeatTemplate(State state, string customState)
        {
            var template = StateViews.FirstOrDefault(x => (x.StateKey == state && state != State.Custom) ||
                           (state == State.Custom && x.CustomStateKey == customState));

            if (template != null)
            {
                return template.RepeatTemplate;
            }

            return null;
        }

        View CreateItemView(State state, string customState)
        {
            var template = StateViews.FirstOrDefault(x => (x.StateKey == state && state != State.Custom) ||
                            (state == State.Custom && x.CustomStateKey == customState));

            if (template != null)
            {
                // TODO: This only allows for a repeatcount of 1.
                // Internally in Xamarin.Forms we cannot add the same element to Children multiple times.
                return template;
            }

            return new Label() { Text = $"Template for {state.ToString()}{customState} not defined." };
        }

        private async Task ChildrenFadeTo(Layout layout, bool animate, bool isHide)
        {
            if (animate && layout?.Children?.Count > 0)
            {
                var tasks = new List<Task<bool>>();
                foreach (var a in layout.Children)
                    if (a is View view) tasks.Add(view.FadeTo(isHide ? 0 : 1, isHide ? 100u : 500u));

                await Task.WhenAll(tasks);
            }
        }
    }
}
