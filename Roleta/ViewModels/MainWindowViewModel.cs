namespace Roleta.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public string Spin{ get; set; }
        public MainWindowViewModel()
        {
            Spin = "rotate(10deg)";
        }
    }
}