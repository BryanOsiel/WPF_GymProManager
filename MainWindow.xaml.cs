using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_GymProManager.Views;

namespace WPF_GymProManager
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Dashboard();
        }

        private void TBShow(object sender, RoutedEventArgs e)
        {
            GridContent.Opacity = 0.5;
        }

        private void TBHide(object sender, RoutedEventArgs e)
        {
            GridContent.Opacity = 1;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PreviewMouseLeftButtonDownBG(object sender, MouseButtonEventArgs e)
        {
            btnShowHide.IsChecked = false;
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximizar_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void Usuarios_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Usuarios();
        }

        private void Miembros_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Miembros();
        }

        private void btnCuenta_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Programa desarrollado para la materia de Ingeniería de Software por Juan Amaral, Osiel Casas, José Flores y Mildred Ruiz.", "Acerca de nosotros");
        }

        private void Equipo_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Equipo();
        }

        private void Inicio_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Dashboard();
        }

        private void btnAsistencia_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Asistencia();
        }
    }
}
