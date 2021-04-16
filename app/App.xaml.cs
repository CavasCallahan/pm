using app.Store;
using app.View;
using app.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigation = new NavigationStore();

            navigation.CurrentView = new HomeView(navigation);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigation)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
