using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace FMTool
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Catch unhandled UI-thread exceptions so the app never silently crashes
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += (s, ev) =>
            {
                if (ev.ExceptionObject is Exception ex)
                    TryShow(ex);
            };
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            TryShow(e.Exception);
            e.Handled = true; // keep app alive
        }

        private static void TryShow(Exception ex)
        {
            try
            {
                MessageBox.Show("Произошла ошибка / An error occurred:\n\n" + ex.Message,
                    "FMTool", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch { }
        }

        public static void SwitchTheme(string themeName)
        {
            string file = themeName switch
            {
                "Arctic" => "Themes/ArcticTheme.xaml",
                "Emerald" => "Themes/EmeraldTheme.xaml",
                "Rose" => "Themes/RoseTheme.xaml",
                "Cherry" => "Themes/CherryTheme.xaml",
                "Gold" => "Themes/GoldTheme.xaml",
                "Violet" => "Themes/VioletTheme.xaml",
                _ => "Themes/DarkTheme.xaml"
            };
            try
            {
                var dict = new ResourceDictionary { Source = new Uri(file, UriKind.Relative) };
                var merged = Current.Resources.MergedDictionaries;
                if (merged.Count > 0) merged.RemoveAt(0);
                merged.Insert(0, dict);
            }
            catch { }
        }

    }

    public class BoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c)
            => (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;
        public object ConvertBack(object value, Type t, object p, CultureInfo c) => throw new NotImplementedException();
    }

    public class InverseBoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c)
            => (value is bool b && b) ? Visibility.Collapsed : Visibility.Visible;
        public object ConvertBack(object value, Type t, object p, CultureInfo c) => throw new NotImplementedException();
    }
}
