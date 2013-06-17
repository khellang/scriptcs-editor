using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Caliburn.Micro;

using MahApps.Metro.Controls;

namespace ScriptCsPad
{
    public class MetroWindowManager : WindowManager
    {
        private static readonly Lazy<List<ResourceDictionary>> ResourceDictionaries;

        static MetroWindowManager()
        {
            ResourceDictionaries = new Lazy<List<ResourceDictionary>>(() => LoadResources().ToList());
        }

        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var metroWindow = view as MetroWindow;
            if (metroWindow != null)
            {
                var owner = InferOwnerOf(metroWindow);
                if (owner != null && isDialog)
                {
                    metroWindow.Owner = owner;
                }

                return metroWindow;
            }

            return CreateMetroWindow(view);
        }

        private MetroWindow CreateMetroWindow(object view)
        {
            var metroWindow = new MetroWindow
            {
                Content = view,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            metroWindow.SetValue(View.IsGeneratedProperty, true);

            AddMetroResources(metroWindow);

            var owner = InferOwnerOf(metroWindow);
            if (owner != null)
            {
                metroWindow.Owner = owner;
                metroWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            return metroWindow;
        }

        private static void AddMetroResources(FrameworkElement window)
        {
            foreach (var dictionary in ResourceDictionaries.Value)
            {
                window.Resources.MergedDictionaries.Add(dictionary);
            }
        }

        private static IEnumerable<ResourceDictionary> LoadResources()
        {
            const string Pack = "pack://application:,,,/MahApps.Metro;component/Styles/";

            var resources = new List<string>
            {
                "Colours.xaml",
                "Fonts.xaml",
                "Controls.xaml",
                "Controls.AnimatedSingleRowTabControl.xaml",
                "Accents/BaseDark.xaml",
                "Accents/Blue.xaml"
            };

            return resources.Select(resource => new ResourceDictionary { Source = new Uri(Pack + resource) });
        }
    }
}