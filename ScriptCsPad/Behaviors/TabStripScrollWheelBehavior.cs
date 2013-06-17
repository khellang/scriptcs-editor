using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

using ScriptCsPad.Extensions;

namespace ScriptCsPad.Behaviors
{
    public class TabStripScrollWheelBehavior : LoadDependentBehavior<TabControl>
    {
        private TabPanel _tabPanel;

        protected override void OnLoaded()
        {
            _tabPanel = AssociatedObject.GetVisualChildren()
                .SelectMany(child => child.GetVisualChildren())
                .OfType<TabPanel>()
                .SingleOrDefault();

            if (_tabPanel == null)
                AssociatedObject.ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;
            else
                _tabPanel.MouseWheel += OnTabPanelMouseWheel;
        }

        public bool InvertScrollDirection
        {
            get { return (bool)GetValue(InvertScrollDirectionProperty); }
            set { SetValue(InvertScrollDirectionProperty, value); }
        }

        public static readonly DependencyProperty InvertScrollDirectionProperty =
            DependencyProperty.Register(
                "InvertScrollDirection",
                typeof(bool),
                typeof(TabStripScrollWheelBehavior),
                new PropertyMetadata(false));

        public bool ScrollWrapsAround
        {
            get { return (bool)GetValue(ScrollWrapsAroundProperty); }
            set { SetValue(ScrollWrapsAroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollWrapsAroundProperty =
            DependencyProperty.Register(
                "ScrollWrapsAround",
                typeof(bool),
                typeof(TabStripScrollWheelBehavior),
                new PropertyMetadata(false));

        protected override void OnDetaching()
        {
            _tabPanel.MouseWheel -= OnTabPanelMouseWheel;
        }

        private void OnItemContainerGeneratorStatusChanged(object sender, EventArgs e)
        {
            if (AssociatedObject.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) return;
            
            AssociatedObject.ItemContainerGenerator.StatusChanged -= OnItemContainerGeneratorStatusChanged;
            OnLoaded();
        }

        private void OnTabPanelMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var nextTab = InvertScrollDirection ? e.Delta < 0 : e.Delta > 0;
            if (nextTab)
            {
                if (AssociatedObject.SelectedIndex < (AssociatedObject.Items.Count - 1))
                {
                    AssociatedObject.SelectedIndex++;
                }
                else if (ScrollWrapsAround)
                {
                    AssociatedObject.SelectedIndex = 0;
                }

                return;
            }

            if (AssociatedObject.SelectedIndex > 0)
            {
                AssociatedObject.SelectedIndex--;
            }
            else if (ScrollWrapsAround)
            {
                AssociatedObject.SelectedIndex = AssociatedObject.Items.Count - 1;
            }
        }
    }
}