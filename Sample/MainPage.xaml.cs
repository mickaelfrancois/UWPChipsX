using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DeanChalk.UWP.ChipsControl;

namespace Sample
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private IEnumerable<string> _availableOptions;
        private IEnumerable<string> _selectedOptions;
        private ChipsSelectorStyle _selectorStyle;

        public MainPage()
        {
            InitializeComponent();
            AvailableOptions = new[]
            {
                "England","France","Spain","Germany",
                "Italy","Greece","Denmark","Netherlands",
                "Ireland","Sweden","Switzerland"
            };
            SelectedOptions = new[] {"England","Spain","Sweden"};
        }

        public IEnumerable<string> AvailableOptions
        {
            get => _availableOptions;
            set
            {
                _availableOptions = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> SelectedOptions
        {
            get => _selectedOptions;
            set
            {
                _selectedOptions = value;
                OnPropertyChanged();
            }
        }

        public ChipsSelectorStyle SelectorStyle
        {
            get => _selectorStyle;
            set
            {
                _selectorStyle = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnAutoSuggestChecked(object sender, RoutedEventArgs e)
        {
            SelectorStyle = ChipsSelectorStyle.AutoSuggest;
        }

        private void OnSelectorChecked(object sender, RoutedEventArgs e)
        {
            SelectorStyle = ChipsSelectorStyle.Selector;
        }
    }
}