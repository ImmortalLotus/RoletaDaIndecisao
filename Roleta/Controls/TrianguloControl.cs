using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Roleta.Models;
using Roleta.Static_Properties;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using Brush = Avalonia.Media.Brush;
using Brushes = Avalonia.Media.Brushes;
using Color = Avalonia.Media.Color;
using Pen = Avalonia.Media.Pen;
using Point = Avalonia.Point;
namespace Roleta.Controls
{
    public class TrianguloControl : Control, IStyleable
    {
        Type IStyleable.StyleKey => typeof(Button);
        static TrianguloControl(){
                AffectsRender<TrianguloControl>(FilmesContagemProperty);

        }
        public static readonly StyledProperty<ObservableCollection<Filme>> FilmesProperty =
            AvaloniaProperty.Register<TrianguloControl, ObservableCollection<Filme>>(nameof(Filmes));

        public ObservableCollection<Filme> Filmes
        {
            get => GetValue(FilmesProperty);
            set => SetValue(FilmesProperty, value);
        }

        public static readonly StyledProperty<int> FilmesContagemProperty =
            AvaloniaProperty.Register<TrianguloControl, int>(nameof(FilmesContagem));

        public int FilmesContagem
        {
            get => GetValue(FilmesContagemProperty);
            set => SetValue(FilmesContagemProperty, value);
        }

        public override void Render(DrawingContext drawingContext)
        {
            PolylineGeometry triangulo = new PolylineGeometry();
            triangulo.Points.Add(new Point(980, 0));
            triangulo.Points.Add(new Point(1015, 50));
            triangulo.Points.Add(new Point(1015, -50));
            triangulo.IsFilled = true;
            Brush brush1 = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            var pen = new Pen(brush1, 5, lineCap: PenLineCap.Square);
            drawingContext.DrawGeometry(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), pen, triangulo);
            for(int i = 0; i<Filmes.Count; i++)
            {
                var size = 20;
                FormattedText texto = new FormattedText
                {
                    Text = Filmes[i].Name[0] + Filmes[i].Name[(Filmes[i].Name.Length)-1].ToString(),
                    Typeface = Typeface.Default,
                    FontSize = size,
                    TextAlignment = TextAlignment.Left,
                    TextWrapping = TextWrapping.Wrap,
                    Constraint = new Avalonia.Size(230,30),                    
                };
                Brush brush = new SolidColorBrush(Cores.coresPastel[i]);
                drawingContext.DrawText(brush, new Point(1030, -100 + (20 * i)), texto);
            }
        }

    }
}
