using jtipanT3.Views;
using Microsoft.Extensions.DependencyInjection;

namespace jtipanT3
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new vContacto()));
        }
    }
}