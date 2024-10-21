﻿using System.Collections.Generic;
using Microsoft.Maui.Controls;


namespace VisualStateLayout
{
    public static class StateLayout
    {
        internal static readonly BindablePropertyKey StateViewsPropertyKey = BindableProperty.CreateReadOnly("StateViews", typeof(IList<StateView>), typeof(Layout), default(IList<StateView>),
            defaultValueCreator: bindable =>
            {
                var collection = new List<StateView>();
                return collection;
            });

        public static readonly BindableProperty StateViewsProperty = StateViewsPropertyKey.BindableProperty;

        static readonly BindableProperty LayoutControllerProperty = BindableProperty.CreateAttached("LayoutController", typeof(StateLayoutController), typeof(Layout), default(StateLayoutController),
                 defaultValueCreator: (b) => new StateLayoutController((Layout)b) { StateViews = GetStateViews(b) });

        public static readonly BindableProperty CurrentStateProperty = BindableProperty.CreateAttached("CurrentState", typeof(State), typeof(Layout), default(State), propertyChanged: (b, o, n) => OnCurrentStateChanged(b, (State)o, (State)n));
        public static readonly BindableProperty CurrentCustomStateKeyProperty = BindableProperty.CreateAttached("CurrentCustomStateKey", typeof(string), typeof(Layout), default(string), propertyChanged: (b, o, n) => OnCurrentCustomStateKeyChanged(b, (string)o, (string)n));

        public static readonly BindableProperty AnimateStateChangesProperty = BindableProperty.CreateAttached("AnimateStateChanges", typeof(bool), typeof(Layout), true, propertyChanged: (b, o, n) => ((BindableObject)b).SetValue(AnimateStateChangesProperty, n));

        public static IList<StateView> GetStateViews(BindableObject b)
        {
            return (IList<StateView>)b.GetValue(StateViewsProperty);
        }

        public static void SetCurrentState(BindableObject b, State value)
        {
            b.SetValue(CurrentStateProperty, value);
        }

        public static State GetCurrentState(BindableObject b)
        {
            return (State)b.GetValue(CurrentStateProperty);
        }

        public static void SetCurrentCustomStateKey(BindableObject b, string value)
        {
            b.SetValue(CurrentCustomStateKeyProperty, value);
        }


        public static string GetCurrentCustomStateKey(BindableObject b)
        {
            return (string)b.GetValue(CurrentCustomStateKeyProperty);
        }

        public static void SetAnimateStateChanges(BindableObject b, bool value)
        {
            b.SetValue(AnimateStateChangesProperty, value);
        }

        public static bool GetAnimateStateChanges(BindableObject b)
        {
            return (bool?)b.GetValue(AnimateStateChangesProperty) ?? false;
        }

        static void OnCurrentStateChanged(BindableObject bindable, State oldValue, State newValue)
        {
            // Swap out the current children for the Loading Template.
            if (oldValue != newValue && newValue != State.None && newValue != State.Custom)
            {
                GetLayoutController(bindable).SwitchToTemplate(newValue, null, GetAnimateStateChanges(bindable));
            }
            else if (oldValue != newValue && newValue == State.None)
            {
                GetLayoutController(bindable).SwitchToContent(GetAnimateStateChanges(bindable));
            }
        }

        static void OnCurrentCustomStateKeyChanged(BindableObject bindable, string oldValue, string newValue)
        {
            var state = GetCurrentState(bindable);

            // Swap out the current children for the Loading Template.
            if (oldValue != newValue && state == State.Custom)
            {
                GetLayoutController(bindable).SwitchToTemplate(newValue, GetAnimateStateChanges(bindable));
            }
            else if (oldValue != newValue && state == State.None)
            {
                GetLayoutController(bindable).SwitchToContent(GetAnimateStateChanges(bindable));
            }
        }

        static StateLayoutController GetLayoutController(BindableObject b)
        {
            return (StateLayoutController)b.GetValue(LayoutControllerProperty);
        }
    }
}
