using Avalonia.Markup.Xaml;
using UVtools.Core.Suggestions;

namespace UVtools.WPF.Controls.Suggestions
{
    public partial class SuggestionModelPositionControl : SuggestionControl
    {
        public SuggestionModelPositionControl() : this(new SuggestionModelPosition())
        { }

        public SuggestionModelPositionControl(Suggestion suggestion)
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
