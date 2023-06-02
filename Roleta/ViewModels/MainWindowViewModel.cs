using DynamicData.Binding;
using ReactiveUI;
using Roleta.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Roleta.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        private string _Angle;


        private ObservableCollection<Filme> _Filmes;
        public ObservableCollection<Filme> Filmes { get=>_Filmes; set => this.RaiseAndSetIfChanged(ref _Filmes, value); }
        public int PontoInvisivel { get; set; }
        public int FilmeInvisivel { get; set; }
        public string Angle
        {
            get => _Angle;
            set => this.RaiseAndSetIfChanged(ref _Angle, value);
        }
        public MainWindowViewModel()
        {
            Angle = "rotate(10deg)";
            Filmes = new ObservableCollection<Filme>();
            _Filmes = Filmes;
            _Angle=Angle;
        }
    }
}