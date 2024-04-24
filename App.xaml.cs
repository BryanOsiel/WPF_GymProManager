using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_GymProManager
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Abre la ventana de login
            var loginWindow = new Login();

            if (loginWindow.ShowDialog() == true)
            {
                // Si la autenticación es exitosa, abre la ventana principal
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                // Si la autenticación falla, cierra la aplicación
                Shutdown();
            }
        }
    }
}
