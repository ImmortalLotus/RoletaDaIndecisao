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

            var pen = new Pen(Brushes.Transparent,20 , lineCap: PenLineCap.Square);
            ConicGradientBrush brush = new ConicGradientBrush();
            brush.Center = new RelativePoint(p1, RelativeUnit.Absolute);

            brush.GradientStops = new GradientStops();

            for (int i = 0; i < 10; i++)
            {
                double offset = i / (10 - 1.0);
                byte r, g, b;
                if (offset < 0.2) // Verde ciano para azul
                {
                    r = 0;
                    g = (byte)((255 - 0) * (5 * offset) + 0);
                    b = (byte)((255 - 255) * (5 * offset) + 255);
                }
                else if (offset < 0.4) // Azul para roxo
                {
                    r = (byte)((128 - 0) * ((offset - 0.2) * 5) + 0);
                    g = 0;
                    b = (byte)((128 - 255) * ((offset - 0.2) * 5) + 255);
                }
                else if (offset < 0.6) // Roxo para vermelho
                {
                    r = (byte)((255 - 128) * ((offset - 0.4) * 5) + 128);
                    g = 0;
                    b = (byte)((0 - 128) * ((offset - 0.4) * 5) + 128);
                }
                else if (offset < 0.8) // Vermelho para laranja
                {
                    r = 255;
                    g = (byte)((165 - 0) * ((offset - 0.6) * 5) + 0);
                    b = 0;
                }
                else // Laranja para amarelo
                {
                    r = 255;
                    g = (byte)((255 - 165) * ((offset - 0.8) * 5) + 165);
                    b = 0;
                }


                byte alpha = 70;
                Color cor= new Color(alpha,r, g, b);

                brush.GradientStops.Add(new GradientStop(cor, offset));
            }

            drawingContext.DrawEllipse(brush, pen, p1, 400, 400);
            //p1                    // Coordenadas do centro do círculo
            Brush brush1= new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
            pen = new Pen(brush1,5,lineCap: PenLineCap.Square);
            // Coordenadas do primeiro vértice
            float angleIncrement = 360 / 10;
            float startingAngle = 270;
            for (int i = 0; i < 10; i++)
            {
                float angle = startingAngle;
                if(i != 0)
                {
                    angle = startingAngle + (i) * angleIncrement;
                }

                Point pointA= new Point(400*Math.Cos((angle* Math.PI)/180),400*Math.Sin((angle * Math.PI)/180));

                drawingContext.DrawLine(pen, new Point(0,0), pointA);
            }

        }
    }
}
