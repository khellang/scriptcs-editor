using System.Windows;
using System.Windows.Controls;

namespace ScriptCsPad.Behaviors
{
    public sealed class RowExpanderSize : IExpanderSize
    {
        private readonly RowDefinition _rowDefinition;

        public RowExpanderSize(UIElement expander, Grid parentGrid)
        {
            var row = Grid.GetRow(expander);
            _rowDefinition = parentGrid.RowDefinitions[row];
        }

        public GridLength DimensionSize
        {
            get { return _rowDefinition.Height; }
            set { _rowDefinition.Height = value; }
        }
    }
}