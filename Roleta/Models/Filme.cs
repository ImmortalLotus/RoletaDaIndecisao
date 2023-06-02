using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roleta.Models
{
    public class Filme : ReactiveObject
    {
        public int Pontuacao
        {
            get => _Pontuacao;
            set => this.RaiseAndSetIfChanged(ref _Pontuacao, value);
        }
        public string Name { get; set; }
        public int AngleCoverage { get; set; }
        private int _Pontuacao;
        public Filme(string Name)
        {
            this.Name = Name;
            this.Pontuacao = 0;
        }
    }
}
