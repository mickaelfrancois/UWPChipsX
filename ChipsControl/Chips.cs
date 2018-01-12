using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace ChipsControl
{
    [TemplatePart(Name = ItemsControlName, Type = typeof(ItemsControl))]
    [TemplatePart(Name = SuggestControlName, Type = typeof(AutoSuggestBox))]
    [TemplatePart(Name = SelectionsControlName, Type = typeof(Selector))]
    public sealed class Chips : Control
    {
        private const string ItemsControlName = "PART_ItemsControl";
        private const string SuggestControlName = "PART_SuggestBox";
        private const string SelectionsControlName = "PART_SelectionList";

        private ItemsControl _itemsControl;
        private Selector _selectionsList;
        private AutoSuggestBox _suggestBox;

        public Chips()
        {
            DefaultStyleKey = typeof(Chips);
        }

        public static DependencyProperty SelectorStyleProperty { get; } =
            DependencyProperty.Register("SelectorStyle", typeof(ChipsSelectorStyle), typeof(Chips),
                new PropertyMetadata(ChipsSelectorStyle.AutoSuggest, OnSelectorStylePropertyChanged));

        public static DependencyProperty SelectedChipsProperty { get; } =
            DependencyProperty.Register("SelectedChips", typeof(IEnumerable<string>), typeof(Chips),
                new PropertyMetadata(Enumerable.Empty<string>(), OnSelectedChipsPropertyChanged));


        public static DependencyProperty AvailableChipsProperty { get; } =
            DependencyProperty.Register("AvailableChips", typeof(IEnumerable<string>), typeof(Chips),
                new PropertyMetadata(Enumerable.Empty<string>(), OnAvailableChipsPropertyChanged));


        public ChipsSelectorStyle SelectorStyle
        {
            get => (ChipsSelectorStyle) GetValue(SelectorStyleProperty);
            set => SetValue(SelectorStyleProperty, value);
        }

        public IEnumerable<string> SelectedChips
        {
            get => (IEnumerable<string>)GetValue(SelectedChipsProperty);
            set => SetValue(SelectedChipsProperty, value);
        }

        public IEnumerable<string> AvailableChips
        {
            get => (IEnumerable<string>)GetValue(AvailableChipsProperty);
            set => SetValue(AvailableChipsProperty, value);
        }

        private static void OnSelectorStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Chips chips))
                return;
            if (chips._selectionsList == null || chips._suggestBox == null)
                return;
            if (!(e.NewValue is ChipsSelectorStyle))
                return;
            var style = (ChipsSelectorStyle) e.NewValue;
            chips._suggestBox.Visibility = style == ChipsSelectorStyle.AutoSuggest ? Visibility.Visible : Visibility.Collapsed;
            chips._selectionsList.Visibility = style == ChipsSelectorStyle.Selector ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsControl = GetTemplateChild(ItemsControlName) as ItemsControl;
            _suggestBox = GetTemplateChild(SuggestControlName) as AutoSuggestBox;
            _selectionsList = GetTemplateChild(SelectionsControlName) as Selector;
            if (_itemsControl == null || _suggestBox == null || _selectionsList == null)
                return;
            _suggestBox.Visibility = SelectorStyle == ChipsSelectorStyle.AutoSuggest
                ? Visibility.Visible
                : Visibility.Collapsed;
            _suggestBox.TextChanged += OnSuggestBoxTextChanged;
            _suggestBox.QuerySubmitted += OnSuggestBoxQuerySubmitted;
            _suggestBox.GotFocus += OnGotFocus;
            _selectionsList.SelectionChanged += OnSelectedItemChanged;
            _selectionsList.Visibility = SelectorStyle == ChipsSelectorStyle.Selector
                ? Visibility.Visible
                : Visibility.Collapsed;
            RecreateGrid();
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            _suggestBox.Text = string.Empty;
            _suggestBox.IsSuggestionListOpen = true;
        }

        private void OnSelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            var newItem = e.AddedItems?.FirstOrDefault() as string;
            if (string.IsNullOrWhiteSpace(newItem) || SelectedChips.Contains(newItem))
                return;
            var chip = new Chip
            {
                Content = newItem
            };
            chip.ChipDelete += OnChipDelete;
            _itemsControl.Items?.Insert(2, chip);
            chip.ChipDelete += OnChipDelete;
            SelectedChips = new[] {newItem}.Concat(SelectedChips);
            _selectionsList.ItemsSource = AvailableChips.Where(c => !SelectedChips.Contains(c));
        }

        private void OnSuggestBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.QueryText))
                return;
            if (SelectedChips.Contains(args.QueryText))
                return;
            var chip = new Chip
            {
                Content = args.QueryText
            };
            chip.ChipDelete += OnChipDelete;
            _itemsControl.Items?.Insert(2, chip);
            chip.ChipDelete += OnChipDelete;
            SelectedChips = new[] {args.QueryText}.Concat(SelectedChips);
        }

        private void OnChipDelete(object sender, Chip e)
        {
            _itemsControl.Items?.Remove(e);
            e.ChipDelete -= OnChipDelete;
            SelectedChips = SelectedChips.Where(c => c != e.Content as string);
            RecreateGrid();
        }

        private void OnSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var matchingChips = AvailableChips.Where(c =>
                c.ToLower().Contains(sender.Text.ToLower()) && !SelectedChips.Contains(c));
            sender.ItemsSource = matchingChips.ToList();
        }

        private void RecreateGrid()
        {
            if (_itemsControl == null || SelectedChips == null || _suggestBox == null || _selectionsList == null)
                return;
            var chips = _itemsControl.Items?.OfType<Chip>().ToList() ?? new List<Chip>();
            foreach (var chip in chips)
                _itemsControl.Items?.Remove(chip);
            foreach (var text in SelectedChips)
            {
                var chip = new Chip
                {
                    Content = text
                };
                _itemsControl.Items?.Add(chip);
                chip.ChipDelete += OnChipDelete;
            }

            _suggestBox.ItemsSource = AvailableChips.Where(c => !SelectedChips.Contains(c));
            _selectionsList.ItemsSource = AvailableChips.Where(c => !SelectedChips.Contains(c));
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

    public enum ChipsSelectorStyle
    {
        AutoSuggest,
        Selector
    }
}