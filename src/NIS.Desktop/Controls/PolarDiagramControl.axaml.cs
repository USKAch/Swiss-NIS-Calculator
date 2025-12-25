using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace NIS.Desktop.Controls;

public partial class PolarDiagramControl : UserControl
{
    private const int PointCount = 36; // 10-degree increments
    private double _scale = 1.0;
    private int _hoveredIndex = -1;
    private Point _tooltipPosition;
    private bool _showTooltip;

    public static readonly StyledProperty<double[]?> PatternDataProperty =
        AvaloniaProperty.Register<PolarDiagramControl, double[]?>(nameof(PatternData));

    public static readonly StyledProperty<double> MaxAttenuationProperty =
        AvaloniaProperty.Register<PolarDiagramControl, double>(nameof(MaxAttenuation), 30.0);

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<PolarDiagramControl, string>(nameof(Title), "Radiation Pattern");

    public double[]? PatternData
    {
        get => GetValue(PatternDataProperty);
        set => SetValue(PatternDataProperty, value);
    }

    public double MaxAttenuation
    {
        get => GetValue(MaxAttenuationProperty);
        set => SetValue(MaxAttenuationProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public PolarDiagramControl()
    {
        InitializeComponent();

        // Subscribe to property changes
        PatternDataProperty.Changed.AddClassHandler<PolarDiagramControl>((s, _) => s.InvalidateVisual());
        MaxAttenuationProperty.Changed.AddClassHandler<PolarDiagramControl>((s, _) => s.InvalidateVisual());

        // Mouse events for interactivity
        PointerMoved += OnPointerMoved;
        PointerExited += OnPointerExited;
        PointerWheelChanged += OnPointerWheelChanged;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        double width = Bounds.Width;
        double height = Bounds.Height;

        // Draw border to confirm rendering
        var borderPen = new Pen(new SolidColorBrush(Color.Parse("#E0E0E0")), 1);
        context.DrawRectangle(Brushes.White, borderPen, new Rect(0, 0, width, height));

        if (width <= 0 || height <= 0) return;

        double margin = 50;
        double centerX = width / 2;
        double centerY = height / 2;
        double radius = (Math.Min(width, height) / 2 - margin) * _scale;

        if (radius <= 10) radius = 10; // Minimum radius to show something

        DrawGrid(context, centerX, centerY, radius);
        DrawRadialLines(context, centerX, centerY, radius);
        DrawPattern(context, centerX, centerY, radius);
        DrawTitle(context);
        DrawTooltip(context);
    }

    private void DrawGrid(DrawingContext context, double centerX, double centerY, double radius)
    {
        double maxDb = MaxAttenuation > 0 ? MaxAttenuation : 30;
        int circleCount = (int)(maxDb / 10) + 1;
        if (circleCount < 3) circleCount = 3;
        double dbStep = maxDb / circleCount;

        var gridPen = new Pen(new SolidColorBrush(Color.Parse("#CCCCCC")), 1);
        var majorGridPen = new Pen(new SolidColorBrush(Color.Parse("#999999")), 2);
        var textBrush = new SolidColorBrush(Color.Parse("#666666"));
        var typeface = new Typeface("Segoe UI");

        for (int i = 1; i <= circleCount; i++)
        {
            double r = radius * i / circleCount;
            var pen = i == circleCount ? majorGridPen : gridPen;

            context.DrawEllipse(null, pen, new Point(centerX, centerY), r, r);

            // Add dB labels
            double db = dbStep * i;
            var formattedText = new FormattedText(
                $"{db:F0} dB",
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                10,
                textBrush);
            context.DrawText(formattedText, new Point(centerX + 3, centerY - r - 7));
        }
    }

    private void DrawRadialLines(DrawingContext context, double centerX, double centerY, double radius)
    {
        var linePen = new Pen(new SolidColorBrush(Color.Parse("#CCCCCC")), 1);
        var textBrush = new SolidColorBrush(Color.Parse("#555555"));
        var typeface = new Typeface("Segoe UI");

        for (int angle = 0; angle < 360; angle += 30)
        {
            double rad = angle * Math.PI / 180;
            double x2 = centerX + radius * Math.Sin(rad);
            double y2 = centerY - radius * Math.Cos(rad);

            context.DrawLine(linePen, new Point(centerX, centerY), new Point(x2, y2));

            // Add angle labels
            double labelRadius = radius + 15;
            double lx = centerX + labelRadius * Math.Sin(rad) - 12;
            double ly = centerY - labelRadius * Math.Cos(rad) - 8;

            var formattedText = new FormattedText(
                $"{angle}°",
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                11,
                textBrush);
            context.DrawText(formattedText, new Point(lx, ly));
        }
    }

    private void DrawPattern(DrawingContext context, double centerX, double centerY, double radius)
    {
        var data = PatternData;
        if (data == null || data.Length < PointCount) return;

        double maxDb = MaxAttenuation > 0 ? MaxAttenuation : 30;

        // Calculate points
        var points = new Point[PointCount];
        for (int i = 0; i < PointCount; i++)
        {
            double angle = i * 10 * Math.PI / 180;
            double attenuation = Math.Min(data[i], maxDb);
            double r = radius * (1 - attenuation / maxDb);
            if (r < 0) r = 0;

            double x = centerX + r * Math.Sin(angle);
            double y = centerY - r * Math.Cos(angle);
            points[i] = new Point(x, y);
        }

        // Draw filled polygon
        var fillBrush = new SolidColorBrush(Color.Parse("#402196F3"));
        var strokePen = new Pen(new SolidColorBrush(Color.Parse("#2196F3")), 2);

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(points[0], true);
            for (int i = 1; i < PointCount; i++)
            {
                ctx.LineTo(points[i]);
            }
            ctx.EndFigure(true);
        }
        context.DrawGeometry(fillBrush, strokePen, geometry);

        // Draw data points
        var pointBrush = new SolidColorBrush(Color.Parse("#1976D2"));
        var hoveredBrush = new SolidColorBrush(Color.Parse("#F44336"));
        var pointPen = new Pen(Brushes.White, 1.5);

        for (int i = 0; i < PointCount; i++)
        {
            var brush = i == _hoveredIndex ? hoveredBrush : pointBrush;
            context.DrawEllipse(brush, pointPen, points[i], 4, 4);
        }
    }

    private void DrawTitle(DrawingContext context)
    {
        var textBrush = new SolidColorBrush(Color.Parse("#333333"));
        var typeface = new Typeface("Segoe UI", FontStyle.Normal, FontWeight.SemiBold);

        var formattedText = new FormattedText(
            Title ?? "Radiation Pattern",
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            typeface,
            14,
            textBrush);
        context.DrawText(formattedText, new Point(10, 10));
    }

    private void DrawTooltip(DrawingContext context)
    {
        if (!_showTooltip || _hoveredIndex < 0) return;

        var data = PatternData;
        if (data == null || _hoveredIndex >= data.Length) return;

        int angleDeg = _hoveredIndex * 10;
        double db = data[_hoveredIndex];
        string text = $"{angleDeg}°: {db:F1} dB";

        var bgBrush = new SolidColorBrush(Color.Parse("#333333"));
        var textBrush = Brushes.White;
        var typeface = new Typeface("Segoe UI");

        var formattedText = new FormattedText(
            text,
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            typeface,
            12,
            textBrush);

        double padding = 6;
        var rect = new Rect(
            _tooltipPosition.X + 15,
            _tooltipPosition.Y - 10,
            formattedText.Width + padding * 2,
            formattedText.Height + padding * 2);

        context.DrawRectangle(bgBrush, null, rect, 4, 4);
        context.DrawText(formattedText, new Point(rect.X + padding, rect.Y + padding));
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        var data = PatternData;
        if (data == null || data.Length < PointCount) return;

        double width = Bounds.Width;
        double height = Bounds.Height;
        double margin = 40;
        double centerX = width / 2;
        double centerY = height / 2;
        double radius = (Math.Min(width, height) / 2 - margin) * _scale;
        double maxDb = MaxAttenuation > 0 ? MaxAttenuation : 30;

        // Find closest point
        int closestIndex = -1;
        double closestDist = double.MaxValue;

        for (int i = 0; i < PointCount; i++)
        {
            double angle = i * 10 * Math.PI / 180;
            double attenuation = Math.Min(data[i], maxDb);
            double r = radius * (1 - attenuation / maxDb);
            if (r < 0) r = 0;

            double x = centerX + r * Math.Sin(angle);
            double y = centerY - r * Math.Cos(angle);

            double dist = Math.Sqrt(Math.Pow(pos.X - x, 2) + Math.Pow(pos.Y - y, 2));
            if (dist < closestDist && dist < 20)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        if (closestIndex != _hoveredIndex || closestIndex >= 0)
        {
            _hoveredIndex = closestIndex;
            _tooltipPosition = pos;
            _showTooltip = closestIndex >= 0;
            InvalidateVisual();
        }
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        _hoveredIndex = -1;
        _showTooltip = false;
        InvalidateVisual();
    }

    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        double delta = e.Delta.Y > 0 ? 1.1 : 0.9;
        _scale = Math.Clamp(_scale * delta, 0.5, 2.0);
        InvalidateVisual();
        e.Handled = true;
    }
}
