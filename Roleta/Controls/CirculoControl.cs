using System;
using System.Collections.Generic;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Threading;
using Brush = Avalonia.Media.Brush;
using Brushes = Avalonia.Media.Brushes;
using Color = Avalonia.Media.Color;
using Pen = Avalonia.Media.Pen;
using Point = Avalonia.Point;

namespace Roleta.Controls
{
    public class CirculoControl : Control
    {
        static CirculoControl()
        {
            AffectsRender<CirculoControl>(AngleProperty);
        }

        public CirculoControl()
        {
            List<string> decisoes = new List<string>();
        }

        public static readonly StyledProperty<double> AngleProperty =
            AvaloniaProperty.Register<CirculoControl, double>(nameof(Angle));

        public double Angle
        {
            get => GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public override void Render(DrawingContext drawingContext)
        {
            var p1 = new Point(0,0);

            var pen = new Pen(Brushes.Black, 20, lineCap: PenLineCap.Square);
            Brush brush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 2));

            drawingContext.DrawEllipse(brush, pen, p1, 400, 400);

        }
    }
}
