using Windows.UI.Xaml.Controls;

namespace Sample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            SampleChips.AvailableChips = new[] {"text1", "text2 blah blah", "text3", "test", "sadfadf", "sdfasdfsdfgsdfgsdfg", "sdfgfg", "sdfgsdfgfds", "dfgsdf", "fdgsfg"};
            SampleChips.SelectedChips = new[] {"text1", "text2 blah blah", "text3"};
        }
    }
}