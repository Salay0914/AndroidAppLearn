using SF_HandheldTerminal.Models.Cable;
using System.Globalization;

namespace SF_HandheldTerminal.Converters
{
    /// <summary>
    /// 将 <see cref="RiskLevel"/> 映射为前景色（用于 chip 文字、强调点）。
    /// </summary>
    public sealed class RiskLevelToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not RiskLevel level)
                return Color.FromArgb("#6B7280");

            return level switch
            {
                RiskLevel.High => Color.FromArgb("#E74C3C"),
                RiskLevel.Medium => Color.FromArgb("#F5A623"),
                RiskLevel.Light => Color.FromArgb("#F5A623"),
                RiskLevel.Normal => Color.FromArgb("#2BBF6E"),
                RiskLevel.Offline => Color.FromArgb("#9AA4B0"),
                _ => Color.FromArgb("#6B7280"),
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    }

    /// <summary>
    /// 将 <see cref="RiskLevel"/> 映射为浅色背景（用于 chip 底色）。
    /// </summary>
    public sealed class RiskLevelToSoftBackgroundConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not RiskLevel level)
                return Color.FromArgb("#EEF1F4");

            return level switch
            {
                RiskLevel.High => Color.FromArgb("#FDECEA"),
                RiskLevel.Medium => Color.FromArgb("#FFF3DE"),
                RiskLevel.Light => Color.FromArgb("#FFF3DE"),
                RiskLevel.Normal => Color.FromArgb("#E2F7EC"),
                RiskLevel.Offline => Color.FromArgb("#EEF1F4"),
                _ => Color.FromArgb("#EEF1F4"),
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    }

    /// <summary>
    /// 在线/离线状态对应颜色。
    /// </summary>
    public sealed class OnlineStatusToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool online && online)
                return Color.FromArgb("#2BBF6E");

            return Color.FromArgb("#9AA4B0");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    }

    /// <summary>
    /// 处理状态对应颜色：已处理 = 绿色，未处理 = 灰色。
    /// </summary>
    public sealed class HandledFlagToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool handled && handled)
                return Color.FromArgb("#2BBF6E");

            return Color.FromArgb("#6B7280");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    }

    /// <summary>
    /// 处理状态对应背景色：已处理 = 浅绿，未处理 = 浅灰。
    /// </summary>
    public sealed class HandledFlagToSoftBackgroundConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool handled && handled)
                return Color.FromArgb("#E2F7EC");

            return Color.FromArgb("#EEF1F4");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    }

    /// <summary>
    /// 当 chip "选中态"切换时给定主色（主蓝） / 反之灰白。
    /// </summary>
    public sealed class BoolToAccentColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool selected && selected)
                return Color.FromArgb("#1F3A6E");

            return Color.FromArgb("#F0F3F8");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    }

    /// <summary>
    /// 当 chip "选中态"切换时给定白色 / 反之深灰文字色。
    /// </summary>
    public sealed class BoolToOnAccentTextColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool selected && selected)
                return Colors.White;

            return Color.FromArgb("#1A2333");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    }

    /// <summary>
    /// 将 <see cref="Color"/> 包装成 <see cref="SolidColorBrush"/>，
    /// 用于 SfCartesianChart 系列的 Fill / Stroke 这类只接受 Brush 的属性。
    /// </summary>
    public sealed class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Color color)
                return new SolidColorBrush(color);

            if (value is Brush brush)
                return brush;

            return new SolidColorBrush(Colors.SteelBlue);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush scb)
                return scb.Color;

            return null;
        }
    }
}
