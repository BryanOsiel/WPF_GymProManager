using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para CRUDMiembros.xaml
    /// </summary>
    public partial class CRUDMiembros : Page
    {
        public CRUDMiembros()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            Content = new Miembros();
        }

        #region CRUD
        #region Crear

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar que ningún campo esté vacío
                if (string.IsNullOrEmpty(tbNumero.Text) ||
                    string.IsNullOrEmpty(tbCalle.Text) ||
                    string.IsNullOrEmpty(tbColonia.Text) ||
                    string.IsNullOrEmpty(tbCodigoPostal.Text) ||
                    string.IsNullOrEmpty(tbMunicipio.Text) ||
                    string.IsNullOrEmpty(tbEstado.Text) ||
                    string.IsNullOrEmpty(tbNombre.Text) ||
                    string.IsNullOrEmpty(tbApellidoPaterno.Text) ||
                    string.IsNullOrEmpty(tbApellidoMaterno.Text) ||
                    string.IsNullOrEmpty(tbGenero.Text) ||
                    dpFechaNacimiento.SelectedDate == null ||
                    string.IsNullOrEmpty(tbEmail.Text) ||
                    string.IsNullOrEmpty(tbTelefono.Text) ||
                    string.IsNullOrEmpty(tbMembresia.Text) ||
                    dpFechaRegistro.SelectedDate == null ||
                    string.IsNullOrEmpty(tbCodigoAcceso.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                // Validar que el campo de teléfono solo contenga números y tenga 10 dígitos
                if (!tbTelefono.Text.All(char.IsDigit) || tbTelefono.Text.Length != 10)
                {
                    MessageBox.Show("El campo de teléfono debe contener exactamente 10 números.");
                    return;
                }

                // Validar que el campo de Codigo Postal solo contenga números y 5 dígitos
                if (!tbCodigoPostal.Text.All(char.IsDigit) || tbCodigoPostal.Text.Length != 5)
                {
                    MessageBox.Show("El campo de Codigo Postal solo debe contener exactamente 5 números.");
                    return;
                }

                // Validar que el campo de Codigo Acceso solo contenga números y tenga 5 dígitos
                if (!tbCodigoAcceso.Text.All(char.IsDigit) || tbCodigoAcceso.Text.Length != 5)
                {
                    MessageBox.Show("El campo Codigo de Acceso debe contener exactamente 5 números.");
                    return;
                }

                // Validar el formato del correo electrónico
                string correo = tbEmail.Text;
                if (!Regex.IsMatch(correo, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    MessageBox.Show("Por favor, introduzca un correo electrónico válido.");
                    return;
                }

                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para obtener el ID de la sucursal
                    string query = "SELECT ID FROM tsucursales WHERE NombreSucursal = @NombreSucursal";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NombreSucursal", "Arnold Gym");

                    // Ejecutar la consulta y obtener el ID de la sucursal
                    int sucursalID = Convert.ToInt32(cmd.ExecuteScalar());

                    // Insertar dirección del miembro
                    string insertDireccionQuery = "INSERT INTO tdireccionclientes (Numero, Calle, Colonia, CodigoPostal, Municipio, Estado) " +
                                                  "VALUES (@Numero, @Calle, @Colonia, @CodigoPostal, @Municipio, @Estado);";
                    MySqlCommand insertDireccionCmd = new MySqlCommand(insertDireccionQuery, connection);
                    insertDireccionCmd.Parameters.AddWithValue("@Numero", tbNumero.Text);
                    insertDireccionCmd.Parameters.AddWithValue("@Calle", tbCalle.Text);
                    insertDireccionCmd.Parameters.AddWithValue("@Colonia", tbColonia.Text);
                    insertDireccionCmd.Parameters.AddWithValue("@CodigoPostal", tbCodigoPostal.Text);
                    insertDireccionCmd.Parameters.AddWithValue("@Municipio", tbMunicipio.Text);
                    insertDireccionCmd.Parameters.AddWithValue("@Estado", tbEstado.Text);
                    insertDireccionCmd.ExecuteNonQuery();

                    // Obtener el ID de la dirección insertada
                    long direccionMiembroId = insertDireccionCmd.LastInsertedId;

                    // Insertar datos del miembro
                    string insertMiembroQuery = "INSERT INTO tdatosclientes (TDireccionClientesID, Nombre, ApellidoPaterno, ApellidoMaterno, Genero, FechaNacimiento, CorreoElectronico, Telefono) " +
                                                "VALUES (@DireccionClienteID, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Genero, @FechaNacimiento, @CorreoElectronico, @Telefono);";
                    MySqlCommand insertMiembroCmd = new MySqlCommand(insertMiembroQuery, connection);
                    insertMiembroCmd.Parameters.AddWithValue("@DireccionClienteID", direccionMiembroId);
                    insertMiembroCmd.Parameters.AddWithValue("@Nombre", tbNombre.Text);
                    insertMiembroCmd.Parameters.AddWithValue("@ApellidoPaterno", tbApellidoPaterno.Text);
                    insertMiembroCmd.Parameters.AddWithValue("@ApellidoMaterno", tbApellidoMaterno.Text);
                    insertMiembroCmd.Parameters.AddWithValue("@Genero", tbGenero.Text);
                    insertMiembroCmd.Parameters.AddWithValue("@FechaNacimiento", dpFechaNacimiento.SelectedDate);
                    insertMiembroCmd.Parameters.AddWithValue("@CorreoElectronico", tbEmail.Text);
                    insertMiembroCmd.Parameters.AddWithValue("@Telefono", tbTelefono.Text);
                    insertMiembroCmd.ExecuteNonQuery();

                    // Consulta para obtener el nombre de la membresía
                    string membresiaQuery = "SELECT TipoMembresia FROM tmembresias WHERE ID = @MembresiaID";
                    MySqlCommand membresiaCmd = new MySqlCommand(membresiaQuery, connection);
                    membresiaCmd.Parameters.AddWithValue("@MembresiaID", Convert.ToInt32(tbMembresia.Text)); // Ajustar según cómo se obtenga la membresía
                    string nombreMembresia = membresiaCmd.ExecuteScalar().ToString();

                    // Insertar información del miembro
                    string insertInfoMiembroQuery = "INSERT INTO tclientes (TSucursalID, TDatosClientesID, MembresiaID, NombreMembresia, FechaRegistro, Acceso) " +
                                                     "VALUES (@SucursalID, @DatosMiembroID, @MembresiaID, @NombreMembresia, @FechaRegistro, @Acceso);";
                    MySqlCommand insertInfoMiembroCmd = new MySqlCommand(insertInfoMiembroQuery, connection);
                    insertInfoMiembroCmd.Parameters.AddWithValue("@SucursalID", sucursalID); // Añadir el ID de la sucursal
                    insertInfoMiembroCmd.Parameters.AddWithValue("@DatosMiembroID", insertMiembroCmd.LastInsertedId);
                    insertInfoMiembroCmd.Parameters.AddWithValue("@MembresiaID", Convert.ToInt32(tbMembresia.Text)); // Ajustar según cómo se obtenga la membresía
                    insertInfoMiembroCmd.Parameters.AddWithValue("@NombreMembresia", nombreMembresia);
                    insertInfoMiembroCmd.Parameters.AddWithValue("@FechaRegistro", dpFechaRegistro.SelectedDate);
                    insertInfoMiembroCmd.Parameters.AddWithValue("@Acceso", tbCodigoAcceso.Text);
                    insertInfoMiembroCmd.ExecuteNonQuery();

                    MessageBox.Show("Miembro creado correctamente.");
                    Content = new Miembros();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el miembro: " + ex.Message);
            }
        }

        #endregion
        public int IdCliente;
        private Button sender;
        #region Leer
        public void Consultar(int clienteID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = @"SELECT 
                                        tc.ID AS ClienteID,
                                        tc.Nombre AS NombreCliente,
                                        tc.ApellidoPaterno,
                                        tc.ApellidoMaterno,
                                        tc.Genero,
                                        tc.FechaNacimiento,
                                        tc.CorreoElectronico,
                                        tc.Telefono,
                                        tm.TipoMembresia AS Membresia,
                                        tc.FechaRegistro,
                                        tc.Acceso,
                                        td.ID AS DireccionID,
                                        td.Numero AS DireccionNumero,
                                        td.Calle AS DireccionCalle,
                                        td.Colonia AS DireccionColonia,
                                        td.CodigoPostal AS DireccionCodigoPostal,
                                        td.Municipio AS DireccionMunicipio,
                                        td.Estado AS DireccionEstado
                                    FROM 
                                        tclientes tc
                                    JOIN 
                                        tdireccionclientes td ON tc.TDatosClientesID = td.ID
                                    JOIN 
                                        tmembresias tm ON tc.MembresiaID = tm.ID
                                    WHERE 
                                        tc.ID = @ClienteID;
                                    ";

                    MySqlCommand com = new MySqlCommand(sqlQuery, connection);
                    com.Parameters.AddWithValue("@ClienteID", clienteID);

                    using (MySqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Mostrar datos en los cuadros de texto
                            this.tbCalle.Text = reader["DireccionCalle"].ToString();
                            this.tbNumero.Text = reader["DireccionNumero"].ToString();
                            this.tbCodigoPostal.Text = reader["DireccionCodigoPostal"].ToString();
                            this.tbColonia.Text = reader["DireccionColonia"].ToString();
                            this.tbMunicipio.Text = reader["DireccionMunicipio"].ToString();
                            this.tbEstado.Text = reader["DireccionEstado"].ToString();
                            this.tbNombre.Text = reader["NombreCliente"].ToString();
                            this.tbApellidoPaterno.Text = reader["ApellidoPaterno"].ToString();
                            this.tbApellidoMaterno.Text = reader["ApellidoMaterno"].ToString();
                            this.tbGenero.Text = reader["Genero"].ToString();
                            this.dpFechaNacimiento.SelectedDate = Convert.ToDateTime(reader["FechaNacimiento"]);
                            this.tbEmail.Text = reader["CorreoElectronico"].ToString();
                            this.tbTelefono.Text = reader["Telefono"].ToString();
                            this.tbMembresia.Text = reader["Membresia"].ToString();
                            this.dpFechaRegistro.SelectedDate = Convert.ToDateTime(reader["FechaRegistro"]);
                            this.tbCodigoAcceso.Text = reader["Acceso"].ToString();
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Manejar excepciones de MySQL
                Console.WriteLine("Error de MySQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        #endregion
    }
    #endregion
}

