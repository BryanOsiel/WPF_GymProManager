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

                    // Insertar dirección del cliente
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
                    long direccionClienteId = insertDireccionCmd.LastInsertedId;

                    // Insertar datos del cliente
                    string insertClienteQuery = "INSERT INTO tdatosclientes (TDireccionClientesID, Nombre, ApellidoPaterno, ApellidoMaterno, Genero, FechaNacimiento, CorreoElectronico, Telefono) " +
                                                "VALUES (@DireccionClienteID, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Genero, @FechaNacimiento, @CorreoElectronico, @Telefono);";
                    MySqlCommand insertClienteCmd = new MySqlCommand(insertClienteQuery, connection);
                    insertClienteCmd.Parameters.AddWithValue("@DireccionClienteID", direccionClienteId);
                    insertClienteCmd.Parameters.AddWithValue("@Nombre", tbNombre.Text);
                    insertClienteCmd.Parameters.AddWithValue("@ApellidoPaterno", tbApellidoPaterno.Text);
                    insertClienteCmd.Parameters.AddWithValue("@ApellidoMaterno", tbApellidoMaterno.Text);
                    insertClienteCmd.Parameters.AddWithValue("@Genero", tbGenero.Text);
                    insertClienteCmd.Parameters.AddWithValue("@FechaNacimiento", dpFechaNacimiento.SelectedDate);
                    insertClienteCmd.Parameters.AddWithValue("@CorreoElectronico", tbEmail.Text);
                    insertClienteCmd.Parameters.AddWithValue("@Telefono", tbTelefono.Text);
                    insertClienteCmd.ExecuteNonQuery();

                    // Consultar el ID de membresía correspondiente al nombre ingresado por el usuario
                    string nombreMembresia = tbMembresia.Text;
                    string membresiaQuery = "SELECT ID FROM tmembresias WHERE TipoMembresia = @NombreMembresia";
                    MySqlCommand membresiaCmd = new MySqlCommand(membresiaQuery, connection);
                    membresiaCmd.Parameters.AddWithValue("@NombreMembresia", nombreMembresia);
                    int idMembresia = Convert.ToInt32(membresiaCmd.ExecuteScalar());

                    // Insertar información del cliente
                    string insertInfoClienteQuery = "INSERT INTO tclientes (TSucursalID, TDatosClientesID, MembresiaID, FechaRegistro, Acceso) " +
                                                     "VALUES (@SucursalID, @DatosClienteID, @MembresiaID, @FechaRegistro, @Acceso);";
                    MySqlCommand insertInfoClienteCmd = new MySqlCommand(insertInfoClienteQuery, connection);
                    insertInfoClienteCmd.Parameters.AddWithValue("@SucursalID", sucursalID); // Ajustar el ID de sucursal según corresponda
                    insertInfoClienteCmd.Parameters.AddWithValue("@DatosClienteID", insertClienteCmd.LastInsertedId);
                    insertInfoClienteCmd.Parameters.AddWithValue("@MembresiaID", idMembresia);
                    insertInfoClienteCmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now); // Fecha actual
                    insertInfoClienteCmd.Parameters.AddWithValue("@Acceso", tbCodigoAcceso.Text);
                    insertInfoClienteCmd.ExecuteNonQuery();


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
        #region Leer
        public void Consultar(int clienteID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = @"
                                        SELECT 
                                            m.ID AS ID_Cliente,
                                            m.TSucursalID,
                                            m.TDatosClientesID,
                                            m.MembresiaID,
                                            m.FechaRegistro,
                                            m.Acceso,
                                            dm.Nombre AS Nombre,
                                            dm.ApellidoPaterno AS Apellido,
                                            dm.ApellidoMaterno AS ApellidoMaterno,
                                            dm.Genero AS Genero,
                                            dm.FechaNacimiento AS FechaNacimiento,
                                            dm.CorreoElectronico AS Email,
                                            dm.Telefono AS Telefono,
                                            dc.ID AS DireccionID,
                                            dc.Numero AS DireccionNumero,
                                            dc.Calle AS DireccionCalle,
                                            dc.Colonia AS DireccionColonia,
                                            dc.CodigoPostal AS DireccionCodigoPostal,
                                            dc.Municipio AS DireccionMunicipio,
                                            dc.Estado AS DireccionEstado,
                                            mb.ID AS MembresiaID,
                                            mb.TipoMembresia AS Tipo_Membresia
                                        FROM 
                                            tclientes m
                                        JOIN 
                                            tdatosclientes dm ON m.TDatosClientesID = dm.ID
                                        JOIN 
                                            tdireccionclientes dc ON dm.TDireccionClientesID = dc.ID
                                        JOIN 
                                            tmembresias mb ON m.MembresiaID = mb.ID
                                        WHERE 
                                            m.ID = @ClienteID;";



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
                            this.tbNombre.Text = reader["Nombre"].ToString();
                            this.tbApellidoPaterno.Text = reader["Apellido"].ToString();
                            this.tbApellidoMaterno.Text = reader["ApellidoMaterno"].ToString();
                            this.tbGenero.Text = reader["Genero"].ToString();
                            this.dpFechaNacimiento.SelectedDate = Convert.ToDateTime(reader["FechaNacimiento"]);
                            this.tbEmail.Text = reader["Email"].ToString();
                            this.tbTelefono.Text = reader["Telefono"].ToString();
                            this.tbMembresia.Text = reader["Tipo_Membresia"].ToString();
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

        public void Editar(int clienteID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = @"
                SELECT 
                    tc.ID AS ClienteID,
                    tc.FechaRegistro,
                    tc.Acceso,
                    tdc.Nombre AS NombreCliente,
                    tdc.ApellidoPaterno,
                    tdc.ApellidoMaterno,
                    tdc.Genero,
                    tdc.FechaNacimiento,
                    tdc.CorreoElectronico,
                    tdc.Telefono,
                    tmb.TipoMembresia AS Membresia,
                    td.Numero AS DireccionNumero,
                    td.Calle AS DireccionCalle,
                    td.Colonia AS DireccionColonia,
                    td.CodigoPostal AS DireccionCodigoPostal,
                    td.Municipio AS DireccionMunicipio,
                    td.Estado AS DireccionEstado
                FROM 
                    tclientes tc
                JOIN 
                    tdatosclientes tdc ON tc.TDatosClientesID = tdc.ID
                JOIN 
                    tmembresias tmb ON tc.MembresiaID = tmb.ID
                JOIN 
                    tdireccionclientes td ON tdc.TDireccionClientesID = td.ID
                WHERE 
                    tc.ID = @ClienteID;";

                    MySqlCommand com = new MySqlCommand(sqlQuery, connection);
                    com.Parameters.AddWithValue("@ClienteID", clienteID);

                    using (MySqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Mostrar datos en los controles de la interfaz de usuario
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

            #endregion
        }
        #endregion

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
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

                // Validar que el campo de Codigo Postal solo contenga números y 5 digitos
                if (!tbCodigoPostal.Text.All(char.IsDigit) || tbCodigoPostal.Text.Length != 5)
                {
                    MessageBox.Show("El campo de Codigo Postal solo debe contener exactamente 5 números.");
                    return;
                }

                // Validar que el campo Acceso tenga exactamente 5 caracteres
                if (tbCodigoAcceso.Text.Length != 5)
                {
                    MessageBox.Show("El campo Acceso debe contener exactamente 5 caracteres.");
                    return;
                }

                // Validar que el correo electrónico tenga un formato válido
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

                    // Consultar el ID de membresía correspondiente al nombre ingresado por el usuario
                    string nombreMembresia = tbMembresia.Text;
                    string membresiaQuery = "SELECT ID FROM tmembresias WHERE TipoMembresia = @NombreMembresia";
                    MySqlCommand membresiaCmd = new MySqlCommand(membresiaQuery, connection);
                    membresiaCmd.Parameters.AddWithValue("@NombreMembresia", nombreMembresia);
                    int idMembresia = Convert.ToInt32(membresiaCmd.ExecuteScalar());

                    // Obtener el ID del cliente que deseas actualizar
                    int clienteID = ObtenerIDCliente(tbNombre.Text); // Reemplaza esto con la lógica real para obtener el ID del cliente

                    // Actualizar información del cliente
                    string updateQuery = @"
                                        UPDATE tclientes AS c
                                        JOIN tdatosclientes AS dc ON c.TDatosClientesID = dc.ID
                                        JOIN tdireccionclientes AS dirc ON dc.TDireccionClientesID = dirc.ID
                                        SET 
                                            dirc.Numero = @Numero,
                                            dirc.Calle = @Calle,
                                            dirc.Colonia = @Colonia,
                                            dirc.CodigoPostal = @CodigoPostal,
                                            dirc.Municipio = @Municipio,
                                            dirc.Estado = @Estado,
                                            dc.Nombre = @Nombre,
                                            dc.ApellidoPaterno = @ApellidoPaterno,
                                            dc.ApellidoMaterno = @ApellidoMaterno,
                                            dc.Genero = @Genero,
                                            dc.FechaNacimiento = @FechaNacimiento,
                                            dc.CorreoElectronico = @CorreoElectronico,
                                            dc.Telefono = @Telefono,
                                            c.MembresiaID = @MembresiaID,
                                            c.FechaRegistro = @FechaRegistro,
                                            c.Acceso = @Acceso
                                        WHERE
                                            c.ID = @ClienteID;";

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@Numero", tbNumero.Text);
                    updateCmd.Parameters.AddWithValue("@Calle", tbCalle.Text);
                    updateCmd.Parameters.AddWithValue("@Colonia", tbColonia.Text);
                    updateCmd.Parameters.AddWithValue("@CodigoPostal", tbCodigoPostal.Text);
                    updateCmd.Parameters.AddWithValue("@Municipio", tbMunicipio.Text);
                    updateCmd.Parameters.AddWithValue("@Estado", tbEstado.Text);
                    updateCmd.Parameters.AddWithValue("@Nombre", tbNombre.Text);
                    updateCmd.Parameters.AddWithValue("@ApellidoPaterno", tbApellidoPaterno.Text);
                    updateCmd.Parameters.AddWithValue("@ApellidoMaterno", tbApellidoMaterno.Text);
                    updateCmd.Parameters.AddWithValue("@Genero", tbGenero.Text);
                    updateCmd.Parameters.AddWithValue("@FechaNacimiento", dpFechaNacimiento.SelectedDate);
                    updateCmd.Parameters.AddWithValue("@CorreoElectronico", tbEmail.Text);
                    updateCmd.Parameters.AddWithValue("@Telefono", tbTelefono.Text);
                    updateCmd.Parameters.AddWithValue("@MembresiaID", idMembresia);
                    updateCmd.Parameters.AddWithValue("@FechaRegistro", dpFechaRegistro.SelectedDate);
                    updateCmd.Parameters.AddWithValue("@Acceso", tbCodigoAcceso.Text);
                    updateCmd.Parameters.AddWithValue("@ClienteID", clienteID);
                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("Cliente actualizado correctamente.");
                    Content = new Miembros();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el cliente: " + ex.Message);
            }
        }


        private int ObtenerIDCliente(string nombreCliente)
        {
            // Obtener la cadena de conexión desde App.config
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // Realiza una consulta a la base de datos para obtener el ID del cliente
            string query = @"
        SELECT tc.ID 
        FROM tclientes AS tc
        JOIN tdatosclientes AS td ON tc.TDatosClientesID = td.ID
        WHERE td.Nombre = @Nombre";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", nombreCliente);

                // Ejecuta la consulta y devuelve el resultado
                object result = command.ExecuteScalar();

                // Verifica si se obtuvo algún resultado y lo convierte a entero
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Si no se encontró ningún cliente con ese nombre, retorna -1
                    return -1;
                }
            }
        }

        public void Eliminar(int clienteID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = @"
                                        SELECT 
                                            m.ID AS ID_Cliente,
                                            m.TSucursalID,
                                            m.TDatosClientesID,
                                            m.MembresiaID,
                                            m.FechaRegistro,
                                            m.Acceso,
                                            dm.Nombre AS Nombre,
                                            dm.ApellidoPaterno AS Apellido,
                                            dm.ApellidoMaterno AS ApellidoMaterno,
                                            dm.Genero AS Genero,
                                            dm.FechaNacimiento AS FechaNacimiento,
                                            dm.CorreoElectronico AS Email,
                                            dm.Telefono AS Telefono,
                                            dc.ID AS DireccionID,
                                            dc.Numero AS DireccionNumero,
                                            dc.Calle AS DireccionCalle,
                                            dc.Colonia AS DireccionColonia,
                                            dc.CodigoPostal AS DireccionCodigoPostal,
                                            dc.Municipio AS DireccionMunicipio,
                                            dc.Estado AS DireccionEstado,
                                            mb.ID AS MembresiaID,
                                            mb.TipoMembresia AS Tipo_Membresia
                                        FROM 
                                            tclientes m
                                        JOIN 
                                            tdatosclientes dm ON m.TDatosClientesID = dm.ID
                                        JOIN 
                                            tdireccionclientes dc ON dm.TDireccionClientesID = dc.ID
                                        JOIN 
                                            tmembresias mb ON m.MembresiaID = mb.ID
                                        WHERE 
                                            m.ID = @ClienteID;";



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
                            this.tbNombre.Text = reader["Nombre"].ToString();
                            this.tbApellidoPaterno.Text = reader["Apellido"].ToString();
                            this.tbApellidoMaterno.Text = reader["ApellidoMaterno"].ToString();
                            this.tbGenero.Text = reader["Genero"].ToString();
                            this.dpFechaNacimiento.SelectedDate = Convert.ToDateTime(reader["FechaNacimiento"]);
                            this.tbEmail.Text = reader["Email"].ToString();
                            this.tbTelefono.Text = reader["Telefono"].ToString();
                            this.tbMembresia.Text = reader["Tipo_Membresia"].ToString();
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

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener el ID del cliente que se desea eliminar
                int clienteID = ObtenerIDCliente(tbNombre.Text); // Reemplaza esto con la lógica real para obtener el ID del cliente

                // Si el ID del cliente es válido (-1 indica que no se encontró el cliente)
                if (clienteID != -1)
                {
                    // Confirmar si realmente se desea eliminar al cliente
                    MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres eliminar este cliente?", "Confirmar Eliminación", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Obtener la cadena de conexión desde App.config
                        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                        // Establecer conexión con la base de datos
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            // Eliminar al cliente de las tablas relacionadas
                            string deleteQuery = @"
                        DELETE tclientes, tdatosclientes, tdireccionclientes 
                        FROM tclientes
                        LEFT JOIN tdatosclientes ON tclientes.TDatosClientesID = tdatosclientes.ID
                        LEFT JOIN tdireccionclientes ON tdatosclientes.TDireccionClientesID = tdireccionclientes.ID
                        WHERE tclientes.ID = @ClienteID";

                            MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                            deleteCmd.Parameters.AddWithValue("@ClienteID", clienteID);
                            deleteCmd.ExecuteNonQuery();

                            MessageBox.Show("Cliente eliminado correctamente.");
                            //Regresa a la ventana de miembros
                            Content = new Miembros();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se encontró ningún cliente con ese nombre.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el cliente: " + ex.Message);
            }
        }

    }
}

