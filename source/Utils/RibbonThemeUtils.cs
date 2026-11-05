using System;

namespace BIMPlugins.UI.Utils
{
    /// <summary>
    /// Provides data for the <see cref="RibbonThemeUtils.ThemeChanged"/> event.
    /// </summary>
    public sealed class ThemeChangedEventArgs : EventArgs
    {
        internal ThemeChangedEventArgs(bool isLight)
        {
            IsLight = isLight;
        }

        /// <summary>
        /// Gets whether the new theme is light.
        /// </summary>
        public bool IsLight { get; private set; }

        /// <summary>
        /// Gets whether the new theme is dark.
        /// </summary>
        public bool IsDark => !IsLight;
    }

    /// <summary>
    /// Represents the method that handles the <see cref="RibbonThemeUtils.ThemeChanged"/> event.
    /// </summary>
    /// <param name="sender">The current <c>UIFramework.ApplicationTheme</c>.</param>
    /// <param name="e">The event data.</param>
    public delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs e);
    /// <summary>
    /// Tracks the Revit UI theme (light or dark) and notifies when it changes.
    /// The theme is detected by the brightness of the active tab background color.
    /// </summary>
    public class RibbonThemeUtils
    {
#pragma warning disable CS0067
        /// <summary>
        /// Occurs when the Revit UI theme changes between light and dark.
        /// </summary>
        public static event ThemeChangedEventHandler ThemeChanged;
#pragma warning restore CS0067
        /// <summary>
        /// Gets whether the current Revit UI theme is light.
        /// </summary>
        public static bool IsLight { get; private set; } = true;
        /// <summary>
        /// Gets whether the current Revit UI theme is dark.
        /// </summary>
        public static bool IsDark => !IsLight;

        static RibbonThemeUtils()
        {
            ExecuteCurrentTheme();
            UIFramework.ApplicationTheme.CurrentTheme.PropertyChanged += CurrentTheme_PropertyChanged;
        }

        /// <summary>
        /// Unsubscribes from theme changes and removes all <see cref="ThemeChanged"/> handlers.
        /// </summary>
        internal static void Dispose()
        {
            UIFramework.ApplicationTheme.CurrentTheme.PropertyChanged -= CurrentTheme_PropertyChanged;
            ThemeChanged = null;
        }

        /// <summary>
        /// Test helper that switches the theme to light, then to dark, and then restores the original theme.
        /// </summary>
        /// <param name="actionLight">Action invoked while the light theme is active.</param>
        /// <param name="actionDark">Action invoked while the dark theme is active.</param>
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

        /// <summary>
        /// Raises <see cref="ThemeChanged"/> if the current theme differs from <see cref="IsLight"/>.
        /// </summary>
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
        /// <summary>
        /// Raises <see cref="ThemeChanged"/>, ignoring exceptions thrown by handlers.
        /// </summary>
        private static void ExecuteThemeChanged(UIFramework.ApplicationTheme theme, bool isLight)
        {
            try
            {
                ThemeChanged?.Invoke(theme, new ThemeChangedEventArgs(IsLight));
            }
            catch { }
        }
        /// <summary>
        /// Checks whether the current theme differs from <see cref="IsLight"/> and updates <see cref="IsLight"/>.
        /// </summary>
        /// <param name="theme">The current theme.</param>
        /// <param name="isLight"><c>true</c> if the current theme is light.</param>
        /// <returns><c>true</c> if the theme changed.</returns>
        private static bool IsThemeChanged(out UIFramework.ApplicationTheme theme, out bool isLight)
        {
            theme = UIFramework.ApplicationTheme.CurrentTheme;
            isLight = IsThemeLight(theme);

            if (isLight == IsLight)
                return false;

            IsLight = isLight;
            return true;
        }
        /// <summary>
        /// Checks whether <paramref name="applicationTheme"/> is light, based on the brightness of the active tab background color.
        /// </summary>
        /// <param name="applicationTheme">The theme.</param>
        /// <returns><c>true</c> if the average of the color components is greater than 128, or if <paramref name="applicationTheme"/> is <c>null</c>.</returns>
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