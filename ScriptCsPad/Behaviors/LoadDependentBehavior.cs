using System.Windows;
using System.Windows.Interactivity;

namespace ScriptCsPad.Behaviors
{
    public abstract class LoadDependentBehavior<T> : Behavior<T> where T : FrameworkElement
    {
        protected override void OnAttached()
        {
            if (AssociatedObject.IsLoaded)
            {
                OnLoaded();
                return;
            }

            AssociatedObject.Loaded += OnAssociatedObjectLoaded;
        }

        protected abstract void OnLoaded();

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            OnLoaded();
            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
        }
    }
}