using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Windows.Forms;
using Button = System.Windows.Controls.Button;

namespace WPF_GymProManager.Views
{
    /// <summary>
    /// Lógica de interacción para Usuarios.xaml
    /// </summary>
    public partial class Usuarios : System.Windows.Controls.UserControl
    {
        public Usuarios()
        {
            InitializeComponent();
            CargarDatos();
            
        }

        void CargarDatos()
        {
            // Obtener la cadena de conexión desde el archivo de configuración
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // Crear una conexión a la base de datos
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    // Abrir la conexión
                    connection.Open();

                    // Definir la consulta SQL
                    string query = "SELECT e.ID AS ID_Empleado, d.Nombre AS Nombre, d.ApellidoPaterno AS Apellido, d.CorreoElectronico AS Email, d.Telefono AS Telefono, e.Puesto AS Puesto FROM templeados e JOIN tdatosempleados d ON e.TDatosEmpleadosID = d.ID JOIN tdireccionempleados de ON d.TDireccionEmpleadosID = de.ID;";

                    // Crear un comando MySQL para ejecutar la consulta
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Crear un adaptador de datos MySQL para ejecutar el comando y llenar un DataSet
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataSet dataSet = new System.Data.DataSet();
                    adapter.Fill(dataSet, "Usuarios");

                    // Asignar el DataSet como origen de datos del control (por ejemplo, un DataGrid)
                    GridDatos.ItemsSource = dataSet.Tables["Usuarios"].DefaultView;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error al cargar datos: " + ex.Message);
                }
            }
        }


        private void Agregar(object sender, RoutedEventArgs e)
        {
            CRUDUsuarios ventana = new CRUDUsuarios();
            FrameUsuarios.Content = ventana;
            ventana.btnCrear.Visibility = Visibility.Visible;
        }

        private void Consultar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del usuario que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDUsuarios
            CRUDUsuarios ventana = new CRUDUsuarios();

            // Configurar la ventana para mostrar la información del usuario consultado
            ventana.IdUsuario = id;
            ventana.Consultar(id);
            ventana.Titulo.Text = "Consultar Usuario";

            // Deshabilitar todos los controles de edición en la ventana
            DisableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameUsuarios.Content = ventana;
        }

        // Método para deshabilitar los controles de edición en la ventana
        private void DisableEditControls(CRUDUsuarios ventana)
        {
            // Deshabilitar todos los controles de edición en la ventana
            ventana.tbCalle.IsEnabled = false;
            ventana.tbNumero.IsEnabled = false;
            ventana.tbColonia.IsEnabled = false;
            ventana.tbCodigoPostal.IsEnabled = false;
            ventana.tbMunicipio.IsEnabled = false;
            ventana.tbEstado.IsEnabled = false;
            ventana.tbNombre.IsEnabled = false;
            ventana.tbApellidoPaterno.IsEnabled = false;
            ventana.tbApellidoMaterno.IsEnabled = false;
            ventana.tbGenero.IsEnabled = false;
            ventana.dpFechaNacimiento.IsEnabled = false;
            ventana.tbEmail.IsEnabled = false;
            ventana.tbTelefono.IsEnabled = false;
            ventana.tbTurno.IsEnabled = false;
            ventana.dpFechaContratacion.IsEnabled = false;
            ventana.tbSalario.IsEnabled = false;
            ventana.tbCodigoAcceso.IsEnabled = false;
            ventana.tbPuesto.IsEnabled = false;
            ventana.tbContraseña.IsEnabled = false;
            
        }

        private void EnableEditControls(CRUDUsuarios ventana)
        {
            // Deshabilitar todos los controles de edición en la ventana
            ventana.tbCalle.IsEnabled = true;
            ventana.tbNumero.IsEnabled = true;
            ventana.tbColonia.IsEnabled = true;
            ventana.tbCodigoPostal.IsEnabled = true;
            ventana.tbMunicipio.IsEnabled = true;
            ventana.tbEstado.IsEnabled = true;
            ventana.tbNombre.IsEnabled = true;
            ventana.tbApellidoPaterno.IsEnabled = true;
            ventana.tbApellidoMaterno.IsEnabled = true;
            ventana.tbGenero.IsEnabled = true;
            ventana.dpFechaNacimiento.IsEnabled = true;
            ventana.tbEmail.IsEnabled = true;
            ventana.tbTelefono.IsEnabled = true;
            ventana.tbTurno.IsEnabled = true;
            ventana.dpFechaContratacion.IsEnabled = true;
            ventana.tbSalario.IsEnabled = true;
            ventana.tbCodigoAcceso.IsEnabled = true;
            ventana.tbPuesto.IsEnabled = true;
            ventana.tbContraseña.IsEnabled = true;

        }
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Actualizar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del usuario que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDUsuarios
            CRUDUsuarios ventana = new CRUDUsuarios();

            // Configurar la ventana para mostrar la información del usuario consultado
            ventana.IdUsuario = id;
            ventana.Editar(id);
            ventana.Titulo.Text = "Editar Usuario";
            ventana.btnActualizar.Visibility = Visibility.Visible;


            // Deshabilitar todos los controles de edición en la ventana
            EnableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameUsuarios.Content = ventana;
        }

        private void Eliminar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del usuario que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDUsuarios
            CRUDUsuarios ventana = new CRUDUsuarios();

            // Configurar la ventana para mostrar la información del usuario consultado
            ventana.IdUsuario = id;
            ventana.Eliminar(id);
            ventana.Titulo.Text = "Eliminar Usuario";
            ventana.btnElinimar.Visibility = Visibility.Visible;


            // Deshabilitar todos los controles de edición en la ventana
            DisableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameUsuarios.Content = ventana;
        }
    }

}
