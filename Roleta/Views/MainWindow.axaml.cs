using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Remote.Protocol;
using Roleta.Controls;
using Roleta.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Roleta.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InputFilme.AddHandler(TextInputEvent, OnDigitacao, RoutingStrategies.Tunnel);
            this.AddHandler
            (
                Control.PointerPressedEvent,
                HandleClickEvent,
                RoutingStrategies.Bubble
            //,true // uncomment if you want to test that the event still propagates event 
            // after being handled
            );
            this.AddHandler(
                CirculoControl.ClickEvent,
                OnClick,
                RoutingStrategies.Bubble
                );
        }
        private async void HandleClickEvent(object? sender, RoutedEventArgs e)
        {
            Control senderControl = (Control)sender!;

            string eventType = e.Route switch
            {
                RoutingStrategies.Bubble => "Bubbling",
                RoutingStrategies.Tunnel => "Tunneling",
                _ => "Direct"
            };
            Debug.WriteLine($"{eventType} Routed Event {e.RoutedEvent!.Name} raised on { senderControl.Name}; Event Source is { (e.Source as Control)!.Name }");
            if(e.Source is not null && (e.Source as Control)!.Name is not null && (e.Source as Control)!.Name.Equals("Circulo"))
            {
                var context = DataContext as MainWindowViewModel;
                RaiseEvent(new RoutedEventArgs(CirculoControl.ClickEvent));
                await Task.Delay(10000);
                context!.Filmes[context!.FilmeInvisivel].Pontuacao = context!.PontoInvisivel;
            }
            // uncomment if you want to test handling the event
            
        }

        public void OnDigitacao(object? sender, RoutedEventArgs args)
        {
            var textInpt = (TextBox)sender!;
            if (textInpt.Text is not null && textInpt.Text.Contains('@'))
            {
                var context = DataContext as MainWindowViewModel;
                if (context.Filmes.Count >= 10)
                {
                    context.Filmes.RemoveAt(0);
                }
                context!.Filmes.Add(new Models.Filme(textInpt.Text.Replace("@","").Trim()));
                textInpt.Text = "";
            }
        }

        public void OnClick(object? sender, RoutedEventArgs args)
        {
            if(Circulo.Classes.Any(x => x.Equals("clicarao")))
            {
                Circulo.Classes.Set("clicarao", false);
                return;
            }
            var context = DataContext as MainWindowViewModel;
                Random rand = new Random(Guid.NewGuid().GetHashCode());
            var tamanho = 360 / context!.Filmes.Count;
            int graus = rand.Next(0, context!.Filmes.Count);
            int giro = 120- graus*(360/ context!.Filmes.Count)  + 360 * rand.Next(5, 20);

            context!.FilmeInvisivel = graus;

            context!.PontoInvisivel=  context!.Filmes[context!.FilmeInvisivel].Pontuacao+ 1;
            context!.Angle = $"rotate({giro}deg)";

            Circulo.Classes.Set("clicarao", true);
            Debug.WriteLine("passou por aqui");
            args.Handled = true;
        }

    }
}