using System;

namespace BIMPlugins.UI.Utils
{
    public sealed class ThemeChangedEventArgs : EventArgs
    {
        internal ThemeChangedEventArgs(bool isLight)
        {
            IsLight = isLight;
        }
        
        public bool IsLight { get; private set; }

        public bool IsDark => !IsLight;
    }

    public delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs e);
    public class RibbonThemeUtils
    {
#pragma warning disable CS0067
        public static event ThemeChangedEventHandler ThemeChanged;
#pragma warning restore CS0067
        public static bool IsLight { get; private set; } = true;
        public static bool IsDark => !IsLight;

        static RibbonThemeUtils()
        {
            ExecuteCurrentTheme();
            UIFramework.ApplicationTheme.CurrentTheme.PropertyChanged += CurrentTheme_PropertyChanged;
        }

        internal static void Dispose()
        {
            UIFramework.ApplicationTheme.CurrentTheme.PropertyChanged -= CurrentTheme_PropertyChanged;
            ThemeChanged = null;
        }

        internal static void ThemeChangedTest(Action actionLight = null, Action actionDark = null)
        {
            UIFramework.RevitRibbonControl.RibbonControl.Dispatcher.Invoke(() =>
            {
                var color = UIFramework.ApplicationTheme.CurrentTheme.ActiveTabBackgroundColor;
                UIFramework.ApplicationTheme.CurrentTheme.ActiveTabBackgroundColor = System.Windows.Media.Colors.White;
                actionLight?.Invoke();
                UIFramework.ApplicationTheme.CurrentTheme.ActiveTabBackgroundColor = System.Windows.Media.Colors.Black;
                actionDark?.Invoke();
                UIFramework.ApplicationTheme.CurrentTheme.ActiveTabBackgroundColor = color;
            });
        }

        private static void ExecuteCurrentTheme()
        {
            if (IsThemeChanged(out UIFramework.ApplicationTheme theme, out bool isLight))
            {
                ExecuteThemeChanged(theme, isLight);
            }
        }

        const string ApplicationCurrentThemeChangedPropertyName = "ActiveTabBackgroundColor";
        private static void CurrentTheme_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ApplicationCurrentThemeChangedPropertyName)
            {
                ExecuteCurrentTheme();
            }
        }
        private static void ExecuteThemeChanged(UIFramework.ApplicationTheme theme, bool isLight)
        {
            try
            {
                ThemeChanged?.Invoke(theme, new ThemeChangedEventArgs(IsLight));
            }
            catch { }
        }
        private static bool IsThemeChanged(out UIFramework.ApplicationTheme theme, out bool isLight)
        {
            theme = UIFramework.ApplicationTheme.CurrentTheme;
            isLight = IsThemeLight(theme);

            if (isLight == IsLight)
                return false;

            IsLight = isLight;
            return true;
        }
        private static bool IsThemeLight(UIFramework.ApplicationTheme applicationTheme)
        {
            if (applicationTheme is null)
                return true;

            var color = applicationTheme.ActiveTabBackgroundColor;
            var brightness = (byte)((color.R + color.G + color.B) / 3);
            return brightness > 128;
        }
    }
}