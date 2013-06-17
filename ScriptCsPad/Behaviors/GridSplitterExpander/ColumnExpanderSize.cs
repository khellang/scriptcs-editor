using System.Windows;
using System.Windows.Controls;

namespace ScriptCsPad.Behaviors
{
    public sealed class ColumnExpanderSize : IExpanderSize
    {
        private readonly ColumnDefinition _columnDefinition;

        public ColumnExpanderSize(UIElement expander, Grid parentGrid)
        {
            var column = Grid.GetColumn(expander);
            _columnDefinition = parentGrid.ColumnDefinitions[column];
        }

        public GridLength DimensionSize
        {
            get { return _columnDefinition.Width; }
            set { _columnDefinition.Width = value; }
        }
    }
}