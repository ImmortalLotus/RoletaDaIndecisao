using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.ConstrainedExecution;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Styling;
using Avalonia.Threading;
using HarfBuzzSharp;
using ReactiveUI;
using Roleta.Models;
using Roleta.Static_Properties;
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
            AffectsRender<CirculoControl>(DivisoesProperty);

        }

        public CirculoControl()
        {
            List<string> decisoes = new List<string>();
            int n = Cores.coresPastel.Length;
            Random rand = new Random();
            while (n > 1)
            {
                int k = rand.Next(n--);
                var temp = Cores.coresPastel[n];
                Cores.coresPastel[n] = Cores.coresPastel[k];
                Cores.coresPastel[k] = temp;
            }
        }

        public static readonly StyledProperty<int> DivisoesProperty =
            AvaloniaProperty.Register<CirculoControl, int>(nameof(Divisoes));

        public int Divisoes
        {
            get => GetValue(DivisoesProperty);
            set => SetValue(DivisoesProperty, value);
        }
        

        public Geometry geraArco(int StartAngle, int SweepAngle)
        {
            var angle1 = DegreesToRad(StartAngle);
            var angle2 = angle1 + DegreesToRad(SweepAngle);

            var startAngle = Math.Min(angle1, angle2);
            var sweepAngle = Math.Max(angle1, angle2);

            var normStart = RadToNormRad(startAngle);
            var normEnd = RadToNormRad(sweepAngle);

            var rect = new Rect(0,0,600, 600);

            if ((normStart == normEnd) && (startAngle != sweepAngle)) // Complete ring.
            {
                return new EllipseGeometry(rect.Deflate(1 / 2));
            }
            else if (SweepAngle == 0)
            {
                return new StreamGeometry();
            }
            else // Partial arc.
            {
                var deflatedRect = rect;

                var centerX = 0;
                var centerY = 0;

                var radiusX = 300;
                var radiusY = 300;

                var angleGap = RadToNormRad(sweepAngle - startAngle);

                var startPoint = GetRingPoint(radiusX, radiusY, centerX, centerY, startAngle);
                var endPoint = GetRingPoint(radiusX, radiusY, centerX, centerY, sweepAngle);

                var arcGeometry = new StreamGeometry();

                using (var ctx = arcGeometry.Open())
                {
                    
                    ctx.BeginFigure(startPoint, false);


                    ctx.ArcTo(endPoint, new Avalonia.Size(radiusX, radiusY), 0, angleGap >= Math.PI,
                        SweepDirection.Clockwise);
                    ctx.LineTo(new Point(0, 0));
                    ctx.EndFigure(false);
                }

                return arcGeometry;
            }
        }

        static double DegreesToRad(double inAngle) =>
        inAngle * Math.PI / 180;

        static double RadToNormRad(double inAngle) => ((inAngle % (Math.PI * 2)) + (Math.PI * 2)) % (Math.PI * 2);

        static Point GetRingPoint(double radiusX, double radiusY, double centerX, double centerY, double angle) =>
            new Point((radiusX * Math.Cos(angle)) + centerX, (radiusY * Math.Sin(angle)) + centerY);
        public override void Render(DrawingContext drawingContext)
        {
            var p1 = new Point(0, 0);
            Brush brush1 = new SolidColorBrush(Cores.coresPastel[0]);
            var pen = new Pen(new SolidColorBrush( Color.FromArgb(255,255,255,255)), 5, lineCap: PenLineCap.Square);
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            if (Divisoes <2)
            {

                drawingContext.DrawEllipse(brush1, null, p1, 300, 300);
            }
            else
            {
                

                float angleIncrement = 360 / Divisoes;
                float startingAngle = 270;

                for (int i = 0; i < Divisoes; i++)
                {
                    int corRandom = rand.Next(1, 2);

                    float angle = startingAngle;
                    float angle2 = 360 / Divisoes;
                    float previousAngle = startingAngle - 360 / Divisoes;
                    if (i != 0)
                    {
                        angle = startingAngle + (i) * angleIncrement;
                        angle2 = 360 / Divisoes;
                        previousAngle = startingAngle + (i - 1) * angleIncrement;
                    }
                    brush1 = new SolidColorBrush(Cores.coresPastel[i]);
                    // Restaure o estado anterior da transformação
                    Point pointA = new Point(300 * Math.Cos((angle * Math.PI) / 180), 300 * Math.Sin((angle * Math.PI) / 180));
                    drawingContext.DrawGeometry(brush1, pen, this.geraArco((int)previousAngle, (int)angle2));
                    drawingContext.DrawLine(pen, new Point(0, 0), pointA);
                }
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
