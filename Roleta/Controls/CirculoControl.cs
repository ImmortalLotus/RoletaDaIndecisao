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
using Roleta.Helper;
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

        static CirculoControl()
        {
            AffectsRender<CirculoControl>(DivisoesProperty);

        }

        public static readonly RoutedEvent<RoutedEventArgs> ClickEvent =
            RoutedEvent.Register<CirculoControl, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }


        public static readonly StyledProperty<int> DivisoesProperty =
            AvaloniaProperty.Register<CirculoControl, int>(nameof(Divisoes));

        public int Divisoes
        {
            get => GetValue(DivisoesProperty);
            set => SetValue(DivisoesProperty, value);
        }

        Type IStyleable.StyleKey => typeof(Button);


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

        


        public override void Render(DrawingContext drawingContext)
        {
            Point p1 = new(0, 0);
            Brush brush1 = new SolidColorBrush(Cores.coresPastel[0]);
            Pen pen = new (new SolidColorBrush( Color.FromArgb(255,255,255,255)), 5, lineCap: PenLineCap.Square);
            Random rand = new (Guid.NewGuid().GetHashCode());
            if (Divisoes <2)
            {

                drawingContext.DrawEllipse(brush1, null, p1, 300, 300);
            }
            else
            {
                DrawRoulette(drawingContext, brush1, pen);
            }

        }

        private void DrawRoulette(DrawingContext drawingContext, Brush brush1, Pen pen)
        {
            float angleIncrement = 360 / Divisoes;
            float startingAngle = 270;

            for (int i = 0; i < Divisoes; i++)
            {

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
                drawingContext.DrawGeometry(brush1, pen, ArcHelper.geraArco((int)previousAngle, (int)angle2));
                drawingContext.DrawLine(pen, new Point(0, 0), pointA);
            }
        }
    }
}
