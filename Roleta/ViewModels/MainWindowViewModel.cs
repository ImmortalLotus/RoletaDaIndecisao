using ReactiveUI;

namespace Roleta.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        private string _Spin;
        public string Angle
        {
            get => _Spin;
            set => this.RaiseAndSetIfChanged(ref _Spin, value);
        }
        public MainWindowViewModel()
        {
            Angle = "rotate(10deg)";
        }
    }
}