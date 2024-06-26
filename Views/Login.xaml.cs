﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cms;

namespace WPF_GymProManager
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string connectionString;
        private MainWindow mainWindow;
        private bool loggedIn = false; // Variable para rastrear si el usuario ha iniciado sesión correctamente

        public Login()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            // Obtener la cadena de conexión del archivo de configuración
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            string nombreUsuario = tbUsuario.Text;
            string contrasena = tbContrasena.Password;

            // Verificar si el usuario ya ha iniciado sesión
            if (loggedIn)
            {
                // Si ya está logueado, no permitir iniciar sesión nuevamente
                return;
            }

            // Realizar la autenticación del empleado
            if (AutenticarEmpleado(nombreUsuario, contrasena))
            {
                // Verificar si la ventana principal ya está abierta y es válida
                if (mainWindow == null || mainWindow.IsVisible == false)
                {
                    // Si la ventana principal no está abierta o está cerrada, crear una nueva instancia
                    mainWindow = new MainWindow();
                    // Asignar el evento Closed para manejar el cierre de la ventana principal
                    mainWindow.Closed += (s, args) =>
                    {
                        loggedIn = false; // Cuando se cierre la ventana principal, establecer loggedIn en false
                    };
                }

                // Intentar mostrar la ventana principal
                try
                {
                    mainWindow.Show();
                    loggedIn = true; // Establecer loggedIn en true para evitar abrir otra ventana de inicio de sesión
                    tbContrasena.Password = "";
                    tbUsuario.Text = "";
                }
                catch (InvalidOperationException ex)
                {
                    // Manejar el error si la ventana principal ya ha sido cerrada
                    Console.WriteLine($"Error al mostrar la ventana principal: {ex.Message}");
                }
            }
            else
            {
                // Mostrar el aviso de usuario o contraseña incorrectos
                lbAviso.Visibility = Visibility.Visible;

                // Esperar 3 segundos
                await Task.Delay(2000);

                // Ocultar el aviso
                lbAviso.Visibility = Visibility.Collapsed;
            }
        }

        private bool AutenticarEmpleado(string nombreUsuario, string contrasena)
        {
            bool autenticado = false;
            string query = "SELECT COUNT(*) FROM tdatosempleados AS de " +
                           "INNER JOIN templeados AS e ON de.ID = e.TDatosEmpleadosID " +
                           "WHERE de.Nombre = @nombreUsuario AND e.Contrasena = @contrasena";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("@contrasena", contrasena);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    autenticado = count > 0;
                }
            }

            return autenticado;
        }
    }
}
