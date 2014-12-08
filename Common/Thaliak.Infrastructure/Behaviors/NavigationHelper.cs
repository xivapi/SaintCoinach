using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Behaviors {
    public class NavigationHelper : FrameworkElement {
        public static readonly DependencyProperty DragEnabledProperty = DependencyProperty.RegisterAttached("DragEnabled", typeof(bool), typeof(NavigationHelper), new PropertyMetadata(false, OnDragEnabledChanged));
        public static readonly DependencyProperty NavigationEnabledProperty = DependencyProperty.RegisterAttached("NavigationEnabled", typeof(bool), typeof(NavigationHelper), new PropertyMetadata(false, OnNavigationEnabledChanged));
        public static readonly DependencyProperty NavigationObjectProperty = DependencyProperty.RegisterAttached("NavigationObject", typeof(object), typeof(NavigationHelper));

        public static void SetNavigationEnabled(DependencyObject o, bool v) { o.SetValue(NavigationEnabledProperty, v); }
        public static bool GetNavigationEnabled(DependencyObject o) { return (bool)o.GetValue(NavigationEnabledProperty); }

        public static void SetDragEnabled(DependencyObject o, bool v) { o.SetValue(DragEnabledProperty, v); }
        public static bool GetDragEnabled(DependencyObject o) { return (bool)o.GetValue(DragEnabledProperty); }

        public static void SetNavigationObject(DependencyObject o, object v) { o.SetValue(NavigationObjectProperty, v); }
        public static object GetNavigationObject(DependencyObject o) { return o.GetValue(NavigationObjectProperty); }

        #region Navigation
        private static void OnNavigationEnabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            var asControl = o as Control;
            if (asControl == null)
                throw new NotSupportedException();

            var oldVal = (bool)e.OldValue;
            var newVal = (bool)e.NewValue;
            if (oldVal == newVal)
                return;

            if (oldVal) {
                asControl.MouseDoubleClick -= OnControlMouseDoubleClick;
                asControl.TouchUp -= OnControlTouchUp;
            }

            if (newVal) {
                asControl.MouseDoubleClick += OnControlMouseDoubleClick;
                asControl.TouchUp += OnControlTouchUp;
            }
        }

        static void OnControlMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            TryNavigate(sender);
        }

        static void OnControlTouchUp(object sender, System.Windows.Input.TouchEventArgs e) {
            TryNavigate(sender);
        }
        static void TryNavigate(object sender) {
            var asDep = sender as DependencyObject;
            if (asDep != null) {
                var obj = GetNavigationObject(asDep);
                if (obj != null)
                    ServiceLocator.Current.GetInstance<IEventAggregator>().GetEvent<Events.NavigationRequestEvent>().Publish(obj, false);
            }
        }
        #endregion

        #region Drag
        private static void OnDragEnabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            var asFramework = o as FrameworkElement;
            if (asFramework == null)
                throw new NotSupportedException();

            var oldVal = (bool)e.OldValue;
            var newVal = (bool)e.NewValue;
            if (oldVal == newVal)
                return;

            if (oldVal)
                asFramework.MouseMove -= OnFrameworkElementMouseMove;

            if (newVal)
                asFramework.MouseMove += OnFrameworkElementMouseMove;
        }

        static void OnFrameworkElementMouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            var objectStore = ServiceLocator.Current.GetInstance<Services.IObjectStore>();

            var ele = sender as FrameworkElement;
            var data = GetNavigationObject(ele);
            if (ele != null && data != null && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
                var id = objectStore.Store(data);
                DragDrop.DoDragDrop(ele, id.ToString(), DragDropEffects.Link);
            }
        }
        #endregion
    }
}
