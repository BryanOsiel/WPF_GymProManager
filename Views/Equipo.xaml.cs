using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using Button = System.Windows.Controls.Button;

namespace WPF_GymProManager.Views
{
    /// <summary>
    /// Lógica de interacción para Equipo.xaml
    /// </summary>
    public partial class Equipo : System.Windows.Controls.UserControl
    {
        public Equipo()
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
                    string query = "SELECT ID AS ID_Equipo, NombreEquipo, Cantidad, Descripcion, Estado, Marca FROM tequipos;";

                    // Crear un comando MySQL para ejecutar la consulta
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Crear un adaptador de datos MySQL para ejecutar el comando y llenar un DataSet
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataSet dataSet = new System.Data.DataSet();
                    adapter.Fill(dataSet, "Equipo");

                    // Asignar el DataSet como origen de datos del control (por ejemplo, un DataGrid)
                    GridDatos.ItemsSource = dataSet.Tables["Equipo"].DefaultView;
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
            CRUDEquipo ventana = new CRUDEquipo();
            FrameUsuarios.Content = ventana;
            ventana.btnCrear.Visibility = Visibility.Visible;
        }

        private void Consultar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del usuario que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDUsuarios
            CRUDEquipo ventana = new CRUDEquipo();

            // Configurar la ventana para mostrar la información del usuario consultado
            ventana.IdEquipo = id;
            ventana.Consultar(id);
            ventana.Titulo.Text = "Consultar Equipo";

            // Deshabilitar todos los controles de edición en la ventana
            DisableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameUsuarios.Content = ventana;
        }

        private void DisableEditControls(CRUDEquipo ventana)
        {
            // Deshabilitar todos los controles de edición en la ventana
            ventana.tbNombreEquipo.IsEnabled = false;
            ventana.tbCantidad.IsEnabled = false;
            ventana.tbDescripcion.IsEnabled = false;
            ventana.tbEstado.IsEnabled = false;
            ventana.tbMarca.IsEnabled = false;

        }
        
        private void EnableEditControls(CRUDEquipo ventana)
        {
            // Deshabilitar todos los controles de edición en la ventana
            ventana.tbNombreEquipo.IsEnabled = true;
            ventana.tbCantidad.IsEnabled = true;
            ventana.tbDescripcion.IsEnabled = true;
            ventana.tbEstado.IsEnabled = true;
            ventana.tbMarca.IsEnabled = true;

        }

        private void Actualizar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del usuario que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDUsuarios
            CRUDEquipo ventana = new CRUDEquipo();

            // Configurar la ventana para mostrar la información del usuario consultado
            ventana.IdEquipo = id;
            ventana.Editar(id);
            ventana.Titulo.Text = "Editar Equipo";
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
            CRUDEquipo ventana = new CRUDEquipo();

            // Configurar la ventana para mostrar la información del usuario consultado
            ventana.IdEquipo = id;
            ventana.Eliminar(id);
            ventana.Titulo.Text = "Eliminar Equipo";
            ventana.btnEliminar.Visibility = Visibility.Visible;


            // Deshabilitar todos los controles de edición en la ventana
            DisableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameUsuarios.Content = ventana;
        }
    }
}
