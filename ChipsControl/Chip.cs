using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ChipsControl
{
    [TemplatePart(Name = CanceButtonName, Type = typeof(Button))]
    public sealed class Chip : ContentControl
    {
        private const string CanceButtonName = "PART_CancelButton";
        private Button _button;

        public Chip()
        {
            DefaultStyleKey = typeof(Chip);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _button = GetTemplateChild(CanceButtonName) as Button;
            if (_button == null)
                return;
            _button.PointerEntered += OnPointerEntered;
            _button.PointerExited += OnPointerExited;
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            
        }
    }
}