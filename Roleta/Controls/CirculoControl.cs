using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Styling;
using Avalonia.Threading;
using HarfBuzzSharp;
using ReactiveUI;
using Brush = Avalonia.Media.Brush;
using Brushes = Avalonia.Media.Brushes;
using Color = Avalonia.Media.Color;
using Pen = Avalonia.Media.Pen;
using Point = Avalonia.Point;

namespace Roleta.Controls
{
    public class CirculoControl : Control,IStyleable
    {
        public static readonly RoutedEvent<RoutedEventArgs> ClickEvent =
            RoutedEvent.Register<CirculoControl, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }



        Type IStyleable.StyleKey => typeof(Button);

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
            var p1 = new Point(0, 0);
            Brush brush1 = new SolidColorBrush(Color.FromArgb(90, 0, 0, 0));
            var pen = new Pen(brush1, 1, lineCap: PenLineCap.Square);
            ConicGradientBrush brush;

            var radialBrush = new RadialGradientBrush
            {
                GradientStops = new GradientStops
                {
                    new GradientStop(Color.FromArgb(255,255,0,0), 1),
                    new GradientStop(Color.FromArgb(90,255,0,0), 0.2),
                    new GradientStop(Color.FromArgb(0,0,0,0), 0)
                }
            };


                brush = CriaBrushParaOCirculo(p1, 100);
                drawingContext.DrawEllipse(brush, pen, p1, 300, 300);
                brush = CriaBrushParaOCirculo(p1, 50);
                drawingContext.DrawEllipse(brush, pen, p1, 305, 305);


            //p1                    // Coordenadas do centro do círculo


            // Coordenadas do primeiro vértice
            float angleIncrement = 360 / 10;
            float startingAngle = 270;
            for (int i = 0; i < 10; i++)
            {
                float angle = startingAngle;

                if (i != 0)
                {
                    angle = startingAngle + (i) * angleIncrement;
                }


                // Restaure o estado anterior da transformação
                Point pointA = new Point(300 * Math.Cos((angle * Math.PI) / 180), 300 * Math.Sin((angle * Math.PI) / 180));

                drawingContext.DrawLine(pen, new Point(0, 0), pointA);
            }

        }

        /// <summary>
        /// Aqui é onde é gerada a cor que irá preencher o círculo. para saber como customizar/dinamicamente criar cores,
        /// verifique o histórico de commits dessa classe.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="alphaInt"></param>
        /// <returns></returns>
        private static ConicGradientBrush CriaBrushParaOCirculo(Point p1, int alphaInt)
        {
            var alpha = (byte)alphaInt;
            ConicGradientBrush brush = new ConicGradientBrush();
            brush.Center = new RelativePoint(p1, RelativeUnit.Absolute);

            brush.GradientStops = new GradientStops();
            var colors = new[]
            {
                Color.FromArgb(alpha,255, 40, 40),
                Color.FromArgb(alpha,255, 20, 20),
                Color.FromArgb(alpha,255, 0, 0),
                Color.FromArgb(alpha, 167, 0, 0),
                Color.FromArgb(alpha, 140, 0, 0)
            };

            for (int i = 0; i < colors.Length; i++)
            {
                double offset = i / (double)(colors.Length - 1);
                var gradientStop = new GradientStop(colors[i], offset);
                brush.GradientStops.Add(gradientStop);
            }


            
         

            return brush;
        }
    }
}
