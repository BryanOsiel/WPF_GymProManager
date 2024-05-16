using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPF_GymProManager.Views
{
    public partial class Asistencia : UserControl
    {
        private string connectionString;

        public Asistencia()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            CargarDatos();
        }

        void CargarDatos()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ta.ID AS ID_Asistencia, COALESCE(td.Nombre, 'N/A') AS Nombre, COALESCE(td.ApellidoPaterno, 'N/A') AS Apellido, tm.TipoMembresia, ta.FechaHoraAsistencia " +
                                   "FROM tasistencia ta " +
                                   "INNER JOIN tclientes tc ON ta.TClienteID = tc.ID " +
                                   "INNER JOIN tdatosclientes td ON tc.TDatosClientesID = td.ID " +
                                   "INNER JOIN tmembresias tm ON tc.MembresiaID = tm.ID " +
                                   "WHERE DATE(ta.FechaHoraAsistencia) = @fechaActual";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@fechaActual", DateTime.Now.Date);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "Asistencia");

                    // Ordenar el DataView por FechaHoraAsistencia en orden ascendente
                    DataView dv = dataSet.Tables["Asistencia"].DefaultView;
                    dv.Sort = "FechaHoraAsistencia ASC";
                    GridDatos.ItemsSource = dv;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos de asistencia: " + ex.Message);
                }
            }
        }

        private void Registrar(object sender, RoutedEventArgs e)
        {
            string codigoAcceso = CodigoAccesoTextBox.Text.Trim();
            if (!EsNumero(codigoAcceso))
            {
                MessageBox.Show("El código de acceso debe ser numérico.");
                return;
            }

            if (!CodigoAccesoExiste(codigoAcceso))
            {
                MessageBox.Show("El código de acceso no existe.");
                return;
            }

            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = "SELECT tc.ID, td.Nombre, td.ApellidoPaterno, td.ApellidoMaterno, tm.TipoMembresia, tm.DuracionMeses, tc.FechaRegistro " +
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
                    int clienteID = reader.GetInt32("ID");
                    string nombre = reader.GetString("Nombre");
                    string apellidoPaterno = reader.GetString("ApellidoPaterno");
                    string apellidoMaterno = reader.GetString("ApellidoMaterno");
                    string tipoMembresia = reader.GetString("TipoMembresia");
                    int duracionMeses = reader.GetInt32("DuracionMeses");
                    DateTime fechaRegistro = reader.GetDateTime("FechaRegistro");

                    Cliente cliente = new Cliente
                    {
                        FechaRegistro = fechaRegistro,
                        TipoMembresia = tipoMembresia,
                        DuracionMeses = duracionMeses
                    };
                    int diasRestantes = cliente.CalcularDiasRestantes();

                    if (diasRestantes > 0)
                    {
                        RegistrarAsistencia(clienteID);
                        MessageBox.Show($"Asistencia registrada para: {nombre} {apellidoPaterno} {apellidoMaterno}. " +
                                        $"Días restantes de membresía ({tipoMembresia}): {diasRestantes}");

                        // Limpiar el TextBox de código de acceso
                        CodigoAccesoTextBox.Clear();
                        Content = new Asistencia();
                    }
                    else
                    {
                        MessageBox.Show($"La membresía de {nombre} {apellidoPaterno} {apellidoMaterno} está vencida.");
                    }
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
            DateTime fechaHoraActual = DateTime.Now;
            string query = "INSERT INTO tasistencia (TClienteID, FechaHoraAsistencia) " +
                           "VALUES (@clienteID, @fechaHoraActual)";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@clienteID", clienteID);
            command.Parameters.AddWithValue("@fechaHoraActual", fechaHoraActual);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                //MessageBox.Show("Asistencia registrada correctamente.");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error al registrar la asistencia: " + ex.Message);
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
