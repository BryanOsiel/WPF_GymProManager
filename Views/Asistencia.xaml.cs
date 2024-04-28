using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace WPF_GymProManager.Views
{
    /// <summary>
    /// Lógica de interacción para Asistencia.xaml
    /// </summary>
    public partial class Asistencia : UserControl
    {
        private string connectionString;

        public Asistencia()
        {
            InitializeComponent();
            CargarDatos();

            // Obtener la cadena de conexión desde el archivo de configuración
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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

                    // Definir la consulta SQL para obtener los datos de asistencia con la información adicional de clientes y membresías
                    string query = "SELECT ta.ID AS ID_Asistencia, COALESCE(td.Nombre, 'N/A') AS Nombre, COALESCE(td.ApellidoPaterno, 'N/A') AS Apellido, tm.TipoMembresia, ta.FechaHoraAsistencia " +
                                   "FROM tasistencia ta " +
                                   "INNER JOIN tclientes tc ON ta.TClienteID = tc.ID " +
                                   "INNER JOIN tdatosclientes td ON tc.TDatosClientesID = td.ID " +
                                   "INNER JOIN tmembresias tm ON tc.MembresiaID = tm.ID";

                    // Crear un comando MySQL para ejecutar la consulta
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Crear un adaptador de datos MySQL para ejecutar el comando y llenar un DataSet
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataSet dataSet = new System.Data.DataSet();
                    adapter.Fill(dataSet, "Asistencia");

                    // Asignar el DataSet como origen de datos del control (por ejemplo, un DataGrid)
                    GridDatos.ItemsSource = dataSet.Tables["Asistencia"].DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos de asistencia: " + ex.Message);
                }
            }
        }





        private void Registrar(object sender, RoutedEventArgs e)
        {
            // Obtener el código de acceso ingresado por el usuario
            string codigoAcceso = CodigoAccesoTextBox.Text.Trim();

            // Verificar si el código de acceso es un número
            if (!EsNumero(codigoAcceso))
            {
                MessageBox.Show("El código de acceso debe ser numérico.");
                return;
            }

            // Verificar si el código de acceso existe en la base de datos
            if (!CodigoAccesoExiste(codigoAcceso))
            {
                MessageBox.Show("El código de acceso no existe.");
                return;
            }

            MySqlConnection connection = new MySqlConnection(connectionString);

            // Obtener los datos del cliente asociados al código de acceso
            string query = "SELECT tc.ID, td.Nombre, td.ApellidoPaterno, td.ApellidoMaterno, tm.TipoMembresia " +
                           "FROM tclientes tc " +
                           "INNER JOIN tdatosclientes td ON tc.TDatosClientesID = td.ID " +
                           "INNER JOIN tmembresias tm ON tc.MembresiaID = tm.ID " +
                           "WHERE tc.Acceso = @codigoAcceso";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@codigoAcceso", codigoAcceso);

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Obtener los datos del cliente
                    int clienteID = reader.GetInt32("ID");
                    string nombre = reader.GetString("Nombre");
                    string apellidoPaterno = reader.GetString("ApellidoPaterno");
                    string apellidoMaterno = reader.GetString("ApellidoMaterno");
                    string tipoMembresia = reader.GetString("TipoMembresia");

                    // Registrar la asistencia del cliente
                    RegistrarAsistencia(clienteID);
                }
                else
                {
                    MessageBox.Show("No se encontraron datos para el código de acceso.");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener datos del cliente: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private bool EsNumero(string texto)
        {
            foreach (char c in texto)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CodigoAccesoExiste(string codigoAcceso)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "SELECT COUNT(*) FROM tclientes WHERE Acceso = @codigoAcceso";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@codigoAcceso", codigoAcceso);

            try
            {
                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar el código de acceso: " + ex.Message);
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void RegistrarAsistencia(int clienteID)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            // Obtener la fecha y hora actual
            DateTime fechaHoraActual = DateTime.Now;

            // Insertar la asistencia del cliente en la base de datos
            string query = "INSERT INTO tasistencia (TClienteID, FechaHoraAsistencia) " +
                           "VALUES (@clienteID, @fechaHoraActual)";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@clienteID", clienteID);
            command.Parameters.AddWithValue("@fechaHoraActual", fechaHoraActual);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                MessageBox.Show("Asistencia registrada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la asistencia: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

    }
}
