using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPChipsX
{
    [TemplatePart(Name = CanceButtonName, Type = typeof(Button))]
    public sealed class Chip : ContentControl
    {
        private const string CanceButtonName = "PART_CancelButton";
        private Button _button;

        public Chip() => DefaultStyleKey = typeof(Chip);

        public event EventHandler<Chip> ChipDelete;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _button = GetTemplateChild(CanceButtonName) as Button;
            if (_button == null)
                return;
            _button.PointerEntered += (o, e) =>
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            _button.PointerExited += (o, e) =>
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            _button.Click += (o, e) => ChipDelete?.Invoke(this, this);
            _button.Visibility = CloseButtonVisiblity;
        }
        public static DependencyProperty CloseButtonVisiblityProperty { get; } =
            DependencyProperty.Register("CloseButtonVisiblity", typeof(Visibility), typeof(Chip),
                new PropertyMetadata(defaultValue: Visibility.Visible,
                    propertyChangedCallback: OnCloseButtonVisiblityPropertyChanged));

        public Visibility CloseButtonVisiblity
        {
            get => (Visibility)GetValue(CloseButtonVisiblityProperty);
            set => SetValue(CloseButtonVisiblityProperty, value);
        }
        private static void OnCloseButtonVisiblityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Chip chip)) return;
            if (!(e.NewValue is Visibility)) return;
            if (chip._button == null) return;
            chip._button.Visibility = (Visibility)e.NewValue;
        }
    }
}