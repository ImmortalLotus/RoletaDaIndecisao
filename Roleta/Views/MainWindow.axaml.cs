using Avalonia.Controls;
using Avalonia.Interactivity;
using Roleta.Controls;
using Roleta.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace Roleta.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

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
        private void HandleClickEvent(object? sender, RoutedEventArgs e)
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
                RaiseEvent(new RoutedEventArgs(CirculoControl.ClickEvent));
            // uncomment if you want to test handling the event
            
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
            int graus = rand.Next(3600, (10 / 2) * 7200) / 5;
            context!.Angle = $"rotate({graus}deg)";

            Circulo.Classes.Set("clicarao", true);
            Debug.WriteLine("passou por aqui");
            args.Handled = true;
        }

    }
}