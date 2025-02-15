using Avalonia.Markup.Xaml;
using UVtools.Core.Suggestions;

namespace UVtools.WPF.Controls.Suggestions
{
    public partial class SuggestionTransitionLayerCountControl : SuggestionControl
    {
        public SuggestionTransitionLayerCountControl() : this(new SuggestionTransitionLayerCount())
        { }

        public SuggestionTransitionLayerCountControl(Suggestion suggestion)
        {
            Suggestion = suggestion;
            DataContext = this;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
