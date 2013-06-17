using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ScriptCsPad.Behaviors
{
    public class GridSplitterExpanderBehavior : Behavior<Expander>
    {
        private bool _sizeInitialized;

        private bool _sizeManuallyChanged;

        private GridLength? _newDimensionSize;

        private GridLength _dimensionSize;

        private GridSplitter _gridSplitter;

        private FrameworkElement _expanderContent;

        private IExpanderSize _expanderSize;

        private Grid _parentGrid;

        protected override void OnAttached()
        {
            var parentGrid = AssociatedObject.Parent as Grid;
            if (parentGrid == null) return;

            _expanderSize = GetExpanderSize(parentGrid);

            _parentGrid = parentGrid;

            _parentGrid.Loaded += OnParentGridLoaded;

            AssociatedObject.Initialized += OnExpanderInitialized;
            AssociatedObject.Expanded += OnExpanderExpanded;
            AssociatedObject.Collapsed += OnExpanderCollapsed;
        }

        private IExpanderSize GetExpanderSize(Grid parentGrid)
        {
            if (IsColumn)
            {
                return new ColumnExpanderSize(AssociatedObject, parentGrid);
            }

            return new RowExpanderSize(AssociatedObject, parentGrid);
        }

        private bool IsColumn
        {
            get
            {
                return AssociatedObject.ExpandDirection == ExpandDirection.Left
                    || AssociatedObject.ExpandDirection == ExpandDirection.Right;
            }
        }

        protected override void OnDetaching()
        {
            _parentGrid.Loaded -= OnParentGridLoaded;

            AssociatedObject.Initialized -= OnExpanderInitialized;
            AssociatedObject.Expanded -= OnExpanderExpanded;
            AssociatedObject.Collapsed -= OnExpanderCollapsed;

            _expanderContent.SizeChanged -= OnExpanderContentSizeChanged;
        }

        private void OnExpanderInitialized(object sender, EventArgs e)
        {
            _expanderContent = (FrameworkElement) AssociatedObject.Content;
            _expanderContent.SizeChanged += OnExpanderContentSizeChanged;
        }

        private void OnParentGridLoaded(object sender, RoutedEventArgs e)
        {
            _gridSplitter = _parentGrid.Children.Cast<UIElement>().OfType<GridSplitter>().FirstOrDefault();
        }

        private void OnExpanderContentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!_sizeInitialized)
            {
                _sizeInitialized = true;
                _dimensionSize = _expanderSize.DimensionSize;
                return;
            }

            if (!_sizeInitialized) return;

            if (_sizeManuallyChanged)
                _sizeManuallyChanged = false;
            else
                _newDimensionSize = _expanderSize.DimensionSize;
        }

        private void OnExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            _expanderSize.DimensionSize = GridLength.Auto;

            if (_gridSplitter != null)
                _gridSplitter.IsEnabled = false;

            _sizeManuallyChanged = true;
        }

        private void OnExpanderExpanded(object sender, RoutedEventArgs e)
        {
            if (_gridSplitter != null)
                _gridSplitter.IsEnabled = true;

            _expanderSize.DimensionSize = _newDimensionSize.HasValue ? _newDimensionSize.Value : _dimensionSize;
        }
    }
}