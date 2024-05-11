using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

namespace WPF_GymProManager.Views
{
    /// <summary>
    /// Lógica de interacción para Productos.xaml
    /// </summary>
    public partial class Productos : UserControl
    {
        public Productos()
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
                    string query = "SELECT ID AS ID_Producto, NombreProducto, Stock, Precio, Descripcion FROM tproductos;";

                    // Crear un comando MySQL para ejecutar la consulta
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Crear un adaptador de datos MySQL para ejecutar el comando y llenar un DataSet
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataSet dataSet = new System.Data.DataSet();
                    adapter.Fill(dataSet, "Producto");

                    // Asignar el DataSet como origen de datos del control (por ejemplo, un DataGrid)
                    GridDatos.ItemsSource = dataSet.Tables["Producto"].DefaultView;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error al cargar datos: " + ex.Message);
                }
            }
        }

        private void Consultar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del producto que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDProducto
            CRUDProductos ventana = new CRUDProductos();

            // Configurar la ventana para mostrar la información del producto consultado
            ventana.IdProducto = id;
            ventana.Consultar(id);
            ventana.Titulo.Text = "Consultar Producto";

            // Deshabilitar todos los controles de edición en la ventana
            DisableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameProductos.Content = ventana;
        }

        private void DisableEditControls(CRUDProductos ventana)
        {
            // Deshabilitar todos los controles de edición en la ventana
            ventana.tbNombreProducto.IsEnabled = false;
            ventana.tbCantidad.IsEnabled = false;
            ventana.tbDescripcion.IsEnabled = false;
            ventana.tbPrecio.IsEnabled = false;

        }

        private void EnableEditControls(CRUDProductos ventana)
        {
            // Deshabilitar todos los controles de edición en la ventana
            ventana.tbNombreProducto.IsEnabled = true;
            ventana.tbCantidad.IsEnabled = true;
            ventana.tbDescripcion.IsEnabled = true;
            ventana.tbPrecio.IsEnabled = true;

        }

        private void Eliminar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del producto que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDProducto
            CRUDProductos ventana = new CRUDProductos();

            // Configurar la ventana para mostrar la información del producto consultado
            ventana.IdProducto = id;
            ventana.Eliminar(id);
            ventana.Titulo.Text = "Eliminar Producto";
            ventana.btnEliminar.Visibility = Visibility.Visible;

            // Deshabilitar todos los controles de edición en la ventana
            DisableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameProductos.Content = ventana;
        }

        private void Actualizar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del producto que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDProducto
            CRUDProductos ventana = new CRUDProductos();

            // Configurar la ventana para mostrar la información del producto consultado
            ventana.IdProducto = id;
            ventana.Editar(id);
            ventana.Titulo.Text = "Editar Producto";
            ventana.btnActualizar.Visibility = Visibility.Visible;

            // Habilitar todos los controles de edición en la ventana
            EnableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameProductos.Content = ventana;
        }

        private void Agregar(object sender, RoutedEventArgs e)
        {
            CRUDProductos ventana = new CRUDProductos();
            FrameProductos.Content = ventana;
            ventana.btnCrear.Visibility = Visibility.Visible;
        }

        private void Buscando(object sender, TextChangedEventArgs e)
        {
            // Obtener el texto de búsqueda
            string textoBusqueda = tbBuscar.Text;

            // Realizar la búsqueda en la base de datos
            DataTable dt = BuscarEnBaseDeDatos(textoBusqueda);

            // Mostrar los resultados en el DataGrid
            GridDatos.ItemsSource = dt.DefaultView;
        }

        private DataTable BuscarEnBaseDeDatos(string textoBusqueda)
        {
            DataTable dt = new DataTable();

            try
            {
                // Obtener la cadena de conexión desde el archivo de configuración
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Crear una conexión a la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();

                    // Definir la consulta SQL para la búsqueda
                    string sqlQuery = @"SELECT ID AS ID_Producto, NombreProducto, Stock, Precio, Descripcion 
                                        FROM tproductos
                                        WHERE NombreProducto LIKE @TextoBusqueda OR
                                              Descripcion LIKE @TextoBusqueda";

                    // Crear un comando MySQL para ejecutar la consulta
                    MySqlCommand command = new MySqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@TextoBusqueda", "%" + textoBusqueda + "%");

                    // Crear un adaptador de datos MySQL para ejecutar el comando y llenar un DataSet
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al buscar datos: " + ex.Message);
            }

            return dt;
        }

    }
}
