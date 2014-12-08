using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Kent.Boogaart.Windows.Controls.Automation.Peers;

namespace Kent.Boogaart.Windows.Controls
{
	/// <summary>
	/// Represents a control with a single, resizable piece of content.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <c>Resizer</c> is a customized <see cref="ContentControl"/> that allows itself to be resized. The default template places
	/// a <see cref="ResizeGrip"/> in the lower-right-hand corner of the content. The user can drag the size grip in order to
	/// resize the <c>Resizer</c> and its content.
	/// </para>
	/// <para>
	/// The <see cref="IsGripEnabled"/> and <see cref="IsGripVisible"/> properties facilitate control over the resize grip
	/// inside the <c>Resizer</c>.
	/// </para>
	/// </remarks>
	/// <example>
	/// The following example shows how a <c>Resizer</c> can be used to provide a resizable <c>TextBox</c>:
	/// <code>
	/// <![CDATA[
	/// <kb:Resizer>
	/// 	<TextBox/>
	/// </kb:Resizer>
	/// ]]>
	/// </code>
	/// </example>
	/// <example>
	/// The following example shows how a <c>Resizer</c> can be used to provide resizable content inside a <c>Popup</c>:
	/// <code>
	/// <![CDATA[
	/// <Popup>
	/// 	<kb:Resizer>
	/// 		<TextBlock>Here is the content</TextBlock>
	/// 	</kb:Resizer>
	/// </Popup>
	/// ]]>
	/// </code>
	/// </example>
	[TemplatePart(Name=Resizer._gripName, Type=typeof(FrameworkElement))]
	public class Resizer : ContentControl
	{
		private FrameworkElement _frameworkElement;
		private Point _resizeOrigin;
		private double _originalWidth;
		private double _originalHeight;

		private static RoutedCommand _startResizeCommand;
		private static RoutedCommand _updateSizeCommand;
		private static RoutedCommand _endResizeCommand;
		private static RoutedCommand _autoSizeCommand;

		/// <summary>
		/// Identifies the <see cref="IsGripEnabled"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsGripEnabledProperty = DependencyProperty.Register("IsGripEnabled",
			typeof(bool),
			typeof(Resizer),
			new FrameworkPropertyMetadata(true));

		/// <summary>
		/// Identifies the <see cref="IsGripVisible"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsGripVisibleProperty = DependencyProperty.Register("IsGripVisible",
			typeof(bool),
			typeof(Resizer),
			new FrameworkPropertyMetadata(true, IsGripVisible_Changed));

		/// <summary>
		/// Identifies the <see cref="IsAutoSizeEnabled"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsAutoSizeEnabledProperty = DependencyProperty.Register("IsAutoSizeEnabled",
			typeof(bool),
			typeof(Resizer),
			new FrameworkPropertyMetadata(true));

		/// <summary>
		/// Identifies the <see cref="ResizeDirection"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty ResizeDirectionProperty = DependencyProperty.Register("ResizeDirection",
			typeof(ResizeDirection),
			typeof(Resizer),
			new FrameworkPropertyMetadata(ResizeDirection.SouthEast));

		private const string _gripName = "PART_Grip";

		/// <summary>
		/// Gets or sets a value indicating whether the grip is enabled.
		/// </summary>
		public bool IsGripEnabled
		{
			get
			{
				return (bool) GetValue(IsGripEnabledProperty);
			}
			set
			{
				SetValue(IsGripEnabledProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the grip is visible.
		/// </summary>
		public bool IsGripVisible
		{
			get
			{
				return (bool) GetValue(IsGripVisibleProperty);
			}
			set
			{
				SetValue(IsGripVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <c>Resizer</c> can be auto-sized (by double-clicking on the grip in the default template).
		/// </summary>
		public bool IsAutoSizeEnabled
		{
			get
			{
				return (bool) GetValue(IsAutoSizeEnabledProperty);
			}
			set
			{
				SetValue(IsAutoSizeEnabledProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating the direction in which resizing takes place.
		/// </summary>
		public ResizeDirection ResizeDirection
		{
			get
			{
				return (ResizeDirection) GetValue(ResizeDirectionProperty);
			}
			set
			{
				SetValue(ResizeDirectionProperty, value);
			}
		}

		/// <summary>
		/// Gets the command used to start a resize operation.
		/// </summary>
		/// <remarks>
		/// The parameter passed to the command must be a <see cref="FrameworkElement"/>, which is used as a context for the resizing operation.
		/// </remarks>
		public static RoutedCommand StartResizeCommand
		{
			get
			{
				return _startResizeCommand;
			}
		}

		/// <summary>
		/// Gets the command used to update the size of the <c>Resizer</c>.
		/// </summary>
		public static RoutedCommand UpdateSizeCommand
		{
			get
			{
				return _updateSizeCommand;
			}
		}

		/// <summary>
		/// Gets the command used to end a resize operation.
		/// </summary>
		public static RoutedCommand EndResizeCommand
		{
			get
			{
				return _endResizeCommand;
			}
		}

		/// <summary>
		/// Gets the command used to automatically size the <c>Resizer</c> according to its content.
		/// </summary>
		public static RoutedCommand AutoSizeCommand
		{
			get
			{
				return _autoSizeCommand;
			}
		}

		static Resizer()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Resizer), new FrameworkPropertyMetadata(typeof(Resizer)));
			HorizontalContentAlignmentProperty.OverrideMetadata(typeof(Resizer), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
			VerticalContentAlignmentProperty.OverrideMetadata(typeof(Resizer), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));

			//hook up commands
			_startResizeCommand = new RoutedCommand("StartResize", typeof(Resizer));
			CommandManager.RegisterClassCommandBinding(typeof(Resizer), new CommandBinding(_startResizeCommand, OnStartResizeCommand));
			_updateSizeCommand = new RoutedCommand("UpdateSize", typeof(Resizer));
			CommandManager.RegisterClassCommandBinding(typeof(Resizer), new CommandBinding(_updateSizeCommand, OnUpdateSizeCommand));
			_endResizeCommand = new RoutedCommand("EndResize", typeof(Resizer));
			CommandManager.RegisterClassCommandBinding(typeof(Resizer), new CommandBinding(_endResizeCommand, OnEndResizeCommand));
			_autoSizeCommand = new RoutedCommand("AutoSize", typeof(Resizer));
			CommandManager.RegisterClassCommandBinding(typeof(Resizer), new CommandBinding(_autoSizeCommand, OnAutoSizeCommand));
		}

		protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
		{
			return new ResizerAutomationPeer(this);
		}

		private static void IsGripVisible_Changed(object sender, DependencyPropertyChangedEventArgs e)
		{
			Resizer resizer = sender as Resizer;
			Debug.Assert(resizer != null);
			FrameworkElement grip = resizer.Template.FindName(_gripName, resizer) as FrameworkElement;

			if (grip != null)
			{
				grip.Visibility = resizer.IsGripVisible ? Visibility.Visible : Visibility.Hidden;
			}
		}

		private static void OnStartResizeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Resizer resizer = sender as Resizer;
			Debug.Assert(resizer != null);
			resizer._frameworkElement = e.Parameter as FrameworkElement;

			if (resizer._frameworkElement == null)
			{
				throw new InvalidOperationException("Parameter to this command must be a FrameworkElement (normally the control being used to represent the Grip).");
			}

			resizer._resizeOrigin = resizer._frameworkElement.PointToScreen(Mouse.GetPosition(resizer._frameworkElement));
			resizer._originalWidth = resizer.ActualWidth;
			resizer._originalHeight = resizer.ActualHeight;
			e.Handled = true;
		}

		private static void OnUpdateSizeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Resizer resizer = sender as Resizer;
			Debug.Assert(resizer != null);

			if (resizer._frameworkElement != null)
			{
				Point point = resizer._frameworkElement.PointToScreen(Mouse.GetPosition(resizer._frameworkElement));
				double widthDelta = 0;
				double heightDelta = 0;

				switch (resizer.ResizeDirection)
				{
					case ResizeDirection.NorthEast:
						widthDelta = point.X - resizer._resizeOrigin.X;
						heightDelta = resizer._resizeOrigin.Y - point.Y;
						break;
					case ResizeDirection.NorthWest:
						widthDelta = resizer._resizeOrigin.X - point.X;
						heightDelta = resizer._resizeOrigin.Y - point.Y;
						break;
					case ResizeDirection.SouthEast:
						widthDelta = point.X - resizer._resizeOrigin.X;
						heightDelta = point.Y - resizer._resizeOrigin.Y;
						break;
					case ResizeDirection.SouthWest:
						widthDelta = resizer._resizeOrigin.X - point.X;
						heightDelta = point.Y - resizer._resizeOrigin.Y;
						break;
					default:
						Debug.Fail("Unexpected ResizeDirection: " + resizer.ResizeDirection);
						break;
				}

				//update the width and height, making sure we don't set to below zero
				resizer.Width = Math.Max(0, resizer._originalWidth + widthDelta);
				resizer.Height = Math.Max(0, resizer._originalHeight + heightDelta);
			}

			e.Handled = true;
		}

		private static void OnEndResizeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Resizer resizer = sender as Resizer;
			Debug.Assert(resizer != null);

			if (resizer._frameworkElement != null)
			{
				resizer._frameworkElement = null;
			}

			e.Handled = true;
		}

		private static void OnAutoSizeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Resizer resizer = sender as Resizer;

			if (resizer != null && resizer.IsAutoSizeEnabled)
			{
				resizer.Width = double.NaN;
				resizer.Height = double.NaN;
				e.Handled = true;
			}
		}
	}
}