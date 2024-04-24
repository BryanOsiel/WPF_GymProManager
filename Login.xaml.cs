using System;
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

            // Realizar la autenticación del empleado
            if (AutenticarEmpleado(nombreUsuario, contrasena))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                // Cerrar la ventana de inicio de sesión
                
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
