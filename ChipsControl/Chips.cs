using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ChipsControl
{
    [TemplatePart(Name = ItemsControlName, Type = typeof(ItemsControl))]
    [TemplatePart(Name = SuggestControlName, Type = typeof(AutoSuggestBox))]
    public sealed class Chips : Control
    {
        private const string ItemsControlName = "PART_ItemsControl";
        private const string SuggestControlName = "PART_SuggestBox";

        private ItemsControl _itemsControl;
        private AutoSuggestBox _suggestBox;

        public Chips()
        {
            DefaultStyleKey = typeof(Chips);
        }

        public bool CanCreateChips
        {
            get => (bool) GetValue(CanCreateChipsProperty);
            set => SetValue(CanCreateChipsProperty, value);
        }


        public static DependencyProperty CanCreateChipsProperty { get; } =
            DependencyProperty.Register("CanCreateChips", typeof(bool), typeof(Chips), new PropertyMetadata(true));


        public IEnumerable<string> SelectedChips
        {
            get => (IEnumerable<string>) GetValue(SelectedChipsProperty);
            set => SetValue(SelectedChipsProperty, value);
        }

        public static DependencyProperty SelectedChipsProperty { get; } =
            DependencyProperty.Register("SelectedChips", typeof(IEnumerable<string>), typeof(Chips),
                new PropertyMetadata(Enumerable.Empty<string>(), OnSelectedChipsPropertyChanged));


        public static DependencyProperty AvailableChipsProperty { get; } =
            DependencyProperty.Register("AvailableChips", typeof(IEnumerable<string>), typeof(Chips),
                new PropertyMetadata(Enumerable.Empty<string>(), OnAvailableChipsPropertyChanged));


        public IEnumerable<string> AvailableChips
        {
            get => (IEnumerable<string>) GetValue(AvailableChipsProperty);
            set => SetValue(AvailableChipsProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsControl = GetTemplateChild(ItemsControlName) as ItemsControl;
            _suggestBox = GetTemplateChild(SuggestControlName) as AutoSuggestBox;
            if (_suggestBox != null)
            {
                _suggestBox.TextChanged += OnSuggestBoxTextChanged;
                _suggestBox.QuerySubmitted += OnSuggestBoxQuerySubmitted;
            }
            RecreateGrid();
        }

        private void OnSuggestBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var chip = new Chip
            {
                Content = args.QueryText
            };
            _itemsControl.Items?.Add(chip);
            sender.Text = string.Empty;
        }

        private void OnSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var matchingChips = AvailableChips.Where(c => c.ToLower().Contains(sender.Text.ToLower()) && !SelectedChips.Contains(c));
                sender.ItemsSource = matchingChips.ToList();
            }
        }

        private void RecreateGrid()
        {
            if (_itemsControl == null || SelectedChips == null || _suggestBox == null)
                return;
            var chips = _itemsControl.Items?.OfType<Chip>().ToList() ?? new List<Chip>();
            foreach (var chip in chips)
            {
                _itemsControl.Items?.Remove(chip);
            }
            foreach (var text in SelectedChips)
            {
                var chip = new Chip
                {
                    Content = text
                };
                _itemsControl.Items?.Add(chip);
            }
            _suggestBox.ItemsSource = AvailableChips;
        }

        private static void OnSelectedChipsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chips = d as Chips;
            chips?.RecreateGrid();
        }

        private static void OnAvailableChipsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chips = d as Chips;
            chips?.RecreateGrid();
        }
    }
}