using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WPF_GymProManager.Views
{
    public partial class CRUDUsuarios : Page
    {
        public CRUDUsuarios()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            Content = new Usuarios();
        }

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
                    string.IsNullOrEmpty(tbPuesto.Text) ||
                    string.IsNullOrEmpty(tbTurno.Text) ||
                    dpFechaContratacion.SelectedDate == null ||
                    string.IsNullOrEmpty(tbSalario.Text) ||
                    string.IsNullOrEmpty(tbCodigoAcceso.Text) ||
                    string.IsNullOrEmpty(tbContraseña.Text))
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

                // Validar que el salario tenga como máximo 5 dígitos
                if (!int.TryParse(tbSalario.Text, out int salario) || salario <= 0 || salario > 99999)
                {
                    MessageBox.Show("El salario debe ser un número válido de hasta 5 dígitos.");
                    return;
                }

                // Validar que el campo de Codigo Acceso solo contenga números y tenga 5 dígitos
                if (!tbCodigoAcceso.Text.All(char.IsDigit) || tbCodigoAcceso.Text.Length != 5)
                {
                    MessageBox.Show("El campo Codigo de Acceso debe contener exactamente 5 números.");
                    return;
                }

                string correo = tbEmail.Text;
                if (!Regex.IsMatch(correo, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    MessageBox.Show("Por favor, introduzca un correo electrónico válido.");
                    return;
                }

                // Comprueba si el texto no coincide con las opciones válidas
                if (tbGenero.Text.ToLower() != "masculino" && tbGenero.Text.ToLower() != "femenino" && tbGenero.Text.ToLower() != "otro")
                {
                    // El texto no coincide con las opciones válidas
                    MessageBox.Show("El género debe ser 'Masculino', 'Femenino' u 'Otro'.");
                    return;
                }

                // Comprueba si el texto no coincide con las opciones válidas
                if (tbTurno.Text.ToLower() != "matutino" && tbTurno.Text.ToLower() != "vespertino")
                {
                    // El texto no coincide con las opciones válidas
                    MessageBox.Show("El turno debe ser 'Matutino' o 'Vespertino'.");
                    return;
                }

                // Comprueba si el texto no coincide con las opciones válidas
                if (tbPuesto.Text.ToLower() != "empleado" && tbPuesto.Text.ToLower() != "administrador")
                {
                    // El texto no coincide con las opciones válidas
                    MessageBox.Show("El puesto debe ser 'Empleado' o 'Administrador'.");
                    return;
                }

                // Obtener la fecha de nacimiento del DatePicker
                DateTime fechaNacimiento = dpFechaNacimiento.SelectedDate ?? DateTime.MinValue;

                // Obtener la fecha de contratación del DatePicker
                DateTime fechaContratacion = dpFechaContratacion.SelectedDate ?? DateTime.MinValue;

                // Calcular la edad actual
                int edad = DateTime.Today.Year - fechaNacimiento.Year;

                // Obtener la fecha actual
                DateTime fechaActual = DateTime.Today;

                // Restar un año si el cumpleaños aún no ha ocurrido este año
                if (DateTime.Today < fechaNacimiento.AddYears(edad))
                {
                    edad--;
                }

                // Comprobar si la edad es menor de 18 años
                if (edad < 18)
                {
                    // Si la edad es menor de 18 años, mostrar un mensaje de error
                    MessageBox.Show("Debe ser mayor de edad para continuar.");
                    return;
                }

                // Comprobar si la fecha de contratación es mayor que la fecha actual
                if (fechaContratacion > fechaActual)
                {
                    // Si la fecha de contratación es mayor que la fecha actual, mostrar un mensaje de error
                    MessageBox.Show("La fecha de contratación no puede ser mayor que la fecha actual.");
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

                    // Insertar dirección del empleado
                    string insertDireccionQuery = "INSERT INTO tdireccionempleados (Numero, Calle, Colonia, CodigoPostal, Municipio, Estado) " +
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
                    long direccionEmpleadoId = insertDireccionCmd.LastInsertedId;

                    // Insertar datos del empleado
                    string insertEmpleadoQuery = "INSERT INTO tdatosempleados (TDireccionEmpleadosID, Nombre, ApellidoPaterno, ApellidoMaterno, Genero, FechaNacimiento, CorreoElectronico, Telefono) " +
                                                 "VALUES (@DireccionEmpleadoID, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Genero, @FechaNacimiento, @CorreoElectronico, @Telefono);";
                    MySqlCommand insertEmpleadoCmd = new MySqlCommand(insertEmpleadoQuery, connection);
                    insertEmpleadoCmd.Parameters.AddWithValue("@DireccionEmpleadoID", direccionEmpleadoId);
                    insertEmpleadoCmd.Parameters.AddWithValue("@Nombre", tbNombre.Text);
                    insertEmpleadoCmd.Parameters.AddWithValue("@ApellidoPaterno", tbApellidoPaterno.Text);
                    insertEmpleadoCmd.Parameters.AddWithValue("@ApellidoMaterno", tbApellidoMaterno.Text);
                    insertEmpleadoCmd.Parameters.AddWithValue("@Genero", tbGenero.Text);
                    insertEmpleadoCmd.Parameters.AddWithValue("@FechaNacimiento", dpFechaNacimiento.SelectedDate);
                    insertEmpleadoCmd.Parameters.AddWithValue("@CorreoElectronico", tbEmail.Text);
                    insertEmpleadoCmd.Parameters.AddWithValue("@Telefono", tbTelefono.Text);
                    insertEmpleadoCmd.ExecuteNonQuery();

                    // Insertar información del empleado
                    string insertInfoEmpleadoQuery = "INSERT INTO templeados (TSucursalID, TDatosEmpleadosID, Puesto, Turno, FechaContratacion, Salario, Acceso, Contrasena) " +
                                                     "VALUES (@SucursalID, @DatosEmpleadoID, @Puesto, @Turno, @FechaContratacion, @Salario, @Acceso, @Contrasena);";
                    MySqlCommand insertInfoEmpleadoCmd = new MySqlCommand(insertInfoEmpleadoQuery, connection);
                    insertInfoEmpleadoCmd.Parameters.AddWithValue("@SucursalID", sucursalID); // Añadir el ID de la sucursal
                    insertInfoEmpleadoCmd.Parameters.AddWithValue("@DatosEmpleadoID", insertEmpleadoCmd.LastInsertedId);
                    insertInfoEmpleadoCmd.Parameters.AddWithValue("@Puesto", tbPuesto.Text);
                    insertInfoEmpleadoCmd.Parameters.AddWithValue("@Turno", tbTurno.Text);
                    insertInfoEmpleadoCmd.Parameters.AddWithValue("@FechaContratacion", dpFechaContratacion.SelectedDate);
                    insertInfoEmpleadoCmd.Parameters.AddWithValue("@Salario", Convert.ToDecimal(tbSalario.Text));
                    insertInfoEmpleadoCmd.Parameters.AddWithValue("@Acceso", tbCodigoAcceso.Text);
                    insertInfoEmpleadoCmd.Parameters.AddWithValue("@Contrasena", tbContraseña.Text);
                    insertInfoEmpleadoCmd.ExecuteNonQuery();


                    MessageBox.Show("Empleado creado correctamente.");
                    Content = new Usuarios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el empleado: " + ex.Message);
            }
        }

        public int IdUsuario;
        //private Button sender;
        public void Consultar(int empleadoID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = @"
                                SELECT 
                                    te.ID AS EmpleadoID,
                                    te.Nombre AS NombreEmpleado,
                                    te.ApellidoPaterno,
                                    te.ApellidoMaterno,
                                    te.Genero,
                                    te.FechaNacimiento,
                                    te.CorreoElectronico,
                                    te.Telefono,
                                    td.ID AS DireccionID,
                                    td.Numero AS DireccionNumero,
                                    td.Calle AS DireccionCalle,
                                    td.Colonia AS DireccionColonia,
                                    td.CodigoPostal AS DireccionCodigoPostal,
                                    td.Municipio AS DireccionMunicipio,
                                    td.Estado AS DireccionEstado,
                                    tem.ID AS EmpleadoID_Templeados,
                                    tem.TSucursalID,
                                    tem.TDatosEmpleadosID,
                                    tem.Puesto,
                                    tem.Turno,
                                    tem.FechaContratacion,
                                    tem.Salario,
                                    tem.Acceso
                                FROM 
                                    tdatosempleados te
                                JOIN 
                                    templeados tem ON te.ID = tem.TDatosEmpleadosID
                                JOIN 
                                    tdireccionempleados td ON te.TDireccionEmpleadosID = td.ID
                                WHERE 
                                    te.ID = @EmpleadoID";

                    MySqlCommand com = new MySqlCommand(sqlQuery, connection);
                    com.Parameters.AddWithValue("@EmpleadoID", empleadoID);

                    using (MySqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Display data in text boxes
                            this.tbCalle.Text = reader["DireccionCalle"].ToString();
                            this.tbNumero.Text = reader["DireccionNumero"].ToString();
                            this.tbCodigoPostal.Text = reader["DireccionCodigoPostal"].ToString();
                            this.tbColonia.Text = reader["DireccionColonia"].ToString();
                            this.tbMunicipio.Text = reader["DireccionMunicipio"].ToString();
                            this.tbEstado.Text = reader["DireccionEstado"].ToString();
                            this.tbNombre.Text = reader["NombreEmpleado"].ToString();
                            this.tbApellidoPaterno.Text = reader["ApellidoPaterno"].ToString();
                            this.tbApellidoMaterno.Text = reader["ApellidoMaterno"].ToString();
                            this.tbGenero.Text = reader["Genero"].ToString();
                            this.dpFechaNacimiento.SelectedDate = Convert.ToDateTime(reader["FechaNacimiento"]);
                            this.tbEmail.Text = reader["CorreoElectronico"].ToString();
                            this.tbTelefono.Text = reader["Telefono"].ToString();
                            this.tbTurno.Text = reader["Turno"].ToString();
                            this.dpFechaContratacion.SelectedDate = Convert.ToDateTime(reader["FechaContratacion"]);
                            this.tbSalario.Text = reader["Salario"].ToString();
                            this.tbCodigoAcceso.Text = reader["Acceso"].ToString();
                            this.tbPuesto.Text = reader["Puesto"].ToString();
                            this.tbContraseña.Text = reader["Contrasena"].ToString();

                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL exceptions
                Console.WriteLine("Error de MySQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void Editar(int empleadoID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = @"
                SELECT 
                    te.ID AS EmpleadoID,
                    te.Nombre AS NombreEmpleado,
                    te.ApellidoPaterno,
                    te.ApellidoMaterno,
                    te.Genero,
                    te.FechaNacimiento,
                    te.CorreoElectronico,
                    te.Telefono,
                    td.ID AS DireccionID,
                    td.Numero AS DireccionNumero,
                    td.Calle AS DireccionCalle,
                    td.Colonia AS DireccionColonia,
                    td.CodigoPostal AS DireccionCodigoPostal,
                    td.Municipio AS DireccionMunicipio,
                    td.Estado AS DireccionEstado,
                    tem.ID AS EmpleadoID_Templeados,
                    tem.TSucursalID,
                    tem.TDatosEmpleadosID,
                    tem.Puesto,
                    tem.Turno,
                    tem.FechaContratacion,
                    tem.Salario,
                    tem.Acceso
                FROM 
                    tdatosempleados te
                JOIN 
                    templeados tem ON te.ID = tem.TDatosEmpleadosID
                JOIN 
                    tdireccionempleados td ON te.TDireccionEmpleadosID = td.ID
                WHERE 
                    te.ID = @EmpleadoID";

                    MySqlCommand com = new MySqlCommand(sqlQuery, connection);
                    com.Parameters.AddWithValue("@EmpleadoID", empleadoID);

                    using (MySqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Populate UI controls with employee data for editing
                            this.tbCalle.Text = reader["DireccionCalle"].ToString();
                            this.tbNumero.Text = reader["DireccionNumero"].ToString();
                            this.tbCodigoPostal.Text = reader["DireccionCodigoPostal"].ToString();
                            this.tbColonia.Text = reader["DireccionColonia"].ToString();
                            this.tbMunicipio.Text = reader["DireccionMunicipio"].ToString();
                            this.tbEstado.Text = reader["DireccionEstado"].ToString();
                            this.tbNombre.Text = reader["NombreEmpleado"].ToString();
                            this.tbApellidoPaterno.Text = reader["ApellidoPaterno"].ToString();
                            this.tbApellidoMaterno.Text = reader["ApellidoMaterno"].ToString();
                            this.tbGenero.Text = reader["Genero"].ToString();
                            this.dpFechaNacimiento.SelectedDate = Convert.ToDateTime(reader["FechaNacimiento"]);
                            this.tbEmail.Text = reader["CorreoElectronico"].ToString();
                            this.tbTelefono.Text = reader["Telefono"].ToString();
                            this.tbTurno.Text = reader["Turno"].ToString();
                            this.dpFechaContratacion.SelectedDate = Convert.ToDateTime(reader["FechaContratacion"]);
                            this.tbSalario.Text = reader["Salario"].ToString();
                            this.tbCodigoAcceso.Text = reader["Acceso"].ToString();
                            this.tbPuesto.Text = reader["Puesto"].ToString();
                            this.tbContraseña.Text = reader["Contrasena"].ToString();
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL exceptions
                Console.WriteLine("Error de MySQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine("Error: " + ex.Message);
            }
        }

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
                    string.IsNullOrEmpty(tbPuesto.Text) ||
                    string.IsNullOrEmpty(tbTurno.Text) ||
                    dpFechaContratacion.SelectedDate == null ||
                    string.IsNullOrEmpty(tbSalario.Text) ||
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

                // Validar que el salario tenga como máximo 5 dígitos
                if (!decimal.TryParse(tbSalario.Text, out decimal salario) || salario <= 0 || salario > 99999)
                {
                    MessageBox.Show("El salario debe ser un número válido de hasta 5 dígitos.");
                    return;
                }

                // Validar que el campo de Codigo Acceso solo contenga números y tenga 5 dígitos
                if (!tbCodigoAcceso.Text.All(char.IsDigit) || tbCodigoAcceso.Text.Length != 5)
                {
                    MessageBox.Show("El campo Codigo de Acceso debe contener exactamente 5 números.");
                    return;
                }

                string correo = tbEmail.Text;
                if (!Regex.IsMatch(correo, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    MessageBox.Show("Por favor, introduzca un correo electrónico válido.");
                    return;
                }

                // Comprueba si el texto no coincide con las opciones válidas
                if (tbGenero.Text != "Masculino" && tbGenero.Text != "Femenino" && tbGenero.Text != "Otro")
                {
                    // El texto no coincide con las opciones válidas
                    MessageBox.Show("El género debe ser 'Masculino', 'Femenino' u 'Otro'.");
                    return;
                }

                // Comprueba si el texto no coincide con las opciones válidas
                if (tbGenero.Text.ToLower() != "masculino" && tbGenero.Text.ToLower() != "femenino" && tbGenero.Text.ToLower() != "otro")
                {
                    // El texto no coincide con las opciones válidas
                    MessageBox.Show("El género debe ser 'Masculino', 'Femenino' u 'Otro'.");
                    return;
                }

                // Comprueba si el texto no coincide con las opciones válidas
                if (tbTurno.Text.ToLower() != "matutino" && tbTurno.Text.ToLower() != "vespertino")
                {
                    // El texto no coincide con las opciones válidas
                    MessageBox.Show("El turno debe ser 'Matutino' o 'Vespertino'.");
                    return;
                }

                // Comprueba si el texto no coincide con las opciones válidas
                if (tbPuesto.Text.ToLower() != "empleado" && tbPuesto.Text.ToLower() != "administrador")
                {
                    // El texto no coincide con las opciones válidas
                    MessageBox.Show("El puesto debe ser 'Empleado' o 'Administrador'.");
                    return;
                }

                // Obtener la fecha de nacimiento del DatePicker
                DateTime fechaNacimiento = dpFechaNacimiento.SelectedDate ?? DateTime.MinValue;

                // Obtener la fecha de contratación del DatePicker
                DateTime fechaContratacion = dpFechaContratacion.SelectedDate ?? DateTime.MinValue;

                // Calcular la edad actual
                int edad = DateTime.Today.Year - fechaNacimiento.Year;

                // Restar un año si el cumpleaños aún no ha ocurrido este año
                if (DateTime.Today < fechaNacimiento.AddYears(edad))
                {
                    edad--;
                }

                // Comprobar si la edad es menor de 18 años
                if (edad < 18)
                {
                    // Si la edad es menor de 18 años, mostrar un mensaje de error
                    MessageBox.Show("Debe ser mayor de edad para continuar.");
                    return;
                }

                // Obtener la fecha actual
                DateTime fechaActual = DateTime.Today;

                // Comprobar si la fecha de contratación es mayor que la fecha actual
                if (fechaContratacion > fechaActual)
                {
                    // Si la fecha de contratación es mayor que la fecha actual, mostrar un mensaje de error
                    MessageBox.Show("La fecha de contratación no puede ser mayor que la fecha actual.");
                    return;
                }

                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Obtener el ID del empleado que deseas actualizar
                    int empleadoID = ObtenerIDEmpleado(tbNombre.Text); // Reemplaza esto con la lógica real para obtener el ID del empleado

                    // Actualizar información del empleado
                    string updateQuery = @"
                UPDATE templeados AS t
                JOIN tdatosempleados AS td ON t.TDatosEmpleadosID = td.ID
                JOIN tdireccionempleados AS tdirec ON td.TDireccionEmpleadosID = tdirec.ID
                SET 
                    tdirec.Calle = @Calle,
                    tdirec.Numero = @Numero,
                    tdirec.Colonia = @Colonia,
                    tdirec.CodigoPostal = @CodigoPostal,
                    tdirec.Municipio = @Municipio,
                    tdirec.Estado = @Estado,
                    td.Nombre = @Nombre,
                    td.ApellidoPaterno = @ApellidoPaterno,
                    td.ApellidoMaterno = @ApellidoMaterno,
                    td.Genero = @Genero,
                    td.FechaNacimiento = @FechaNacimiento,
                    td.CorreoElectronico = @CorreoElectronico,
                    td.Telefono = @Telefono,
                    t.Turno = @Turno,
                    t.FechaContratacion = @FechaContratacion,
                    t.Salario = @Salario,
                    t.Acceso = @Acceso,
                    t.Puesto = @Puesto";

                    if (!string.IsNullOrEmpty(tbContraseña.Text))
                    {
                        // Agregar la actualización de la contraseña si se proporciona
                        updateQuery += ", t.Contrasena = @Contrasena";
                    }

                    updateQuery += " WHERE td.ID = @EmpleadoID"; // Cambiado de NombreID a EmpleadoID

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@Puesto", tbPuesto.Text);
                    updateCmd.Parameters.AddWithValue("@Turno", tbTurno.Text);
                    updateCmd.Parameters.AddWithValue("@FechaContratacion", dpFechaContratacion.SelectedDate);
                    updateCmd.Parameters.AddWithValue("@Salario", salario);
                    updateCmd.Parameters.AddWithValue("@Acceso", tbCodigoAcceso.Text);
                    updateCmd.Parameters.AddWithValue("@Nombre", tbNombre.Text);
                    updateCmd.Parameters.AddWithValue("@ApellidoPaterno", tbApellidoPaterno.Text);
                    updateCmd.Parameters.AddWithValue("@ApellidoMaterno", tbApellidoMaterno.Text);
                    updateCmd.Parameters.AddWithValue("@Genero", tbGenero.Text);
                    updateCmd.Parameters.AddWithValue("@FechaNacimiento", dpFechaNacimiento.SelectedDate);
                    updateCmd.Parameters.AddWithValue("@CorreoElectronico", tbEmail.Text);
                    updateCmd.Parameters.AddWithValue("@Telefono", tbTelefono.Text);
                    updateCmd.Parameters.AddWithValue("@Numero", tbNumero.Text);
                    updateCmd.Parameters.AddWithValue("@Calle", tbCalle.Text);
                    updateCmd.Parameters.AddWithValue("@Colonia", tbColonia.Text);
                    updateCmd.Parameters.AddWithValue("@CodigoPostal", tbCodigoPostal.Text);
                    updateCmd.Parameters.AddWithValue("@Municipio", tbMunicipio.Text);
                    updateCmd.Parameters.AddWithValue("@Estado", tbEstado.Text);
                    updateCmd.Parameters.AddWithValue("@EmpleadoID", empleadoID); // Cambiado de NombreID a EmpleadoID

                    // Agregar la contraseña solo si se proporciona
                    if (!string.IsNullOrEmpty(tbContraseña.Text))
                    {
                        updateCmd.Parameters.AddWithValue("@Contrasena", tbContraseña.Text);
                    }

                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("Empleado actualizado correctamente.");
                    Content = new Usuarios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el empleado: " + ex.Message);
            }
        }


        private int ObtenerIDEmpleado(string nombreEmpleado)
        {

            // Obtener la cadena de conexión desde App.config
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // Realiza una consulta a la base de datos para obtener el ID del empleado
            string query = @"
                SELECT t.ID 
                FROM templeados AS t
                JOIN tdatosempleados AS td ON t.TDatosEmpleadosID = td.ID
                WHERE td.Nombre = @Nombre";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", nombreEmpleado);

                // Ejecuta la consulta y devuelve el resultado
                object result = command.ExecuteScalar();

                // Verifica si se obtuvo algún resultado y lo convierte a entero
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Si no se encontró ningún empleado con ese nombre, retorna -1
                    return -1;
                }
            }
        }

        public void Eliminar(int empleadoID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = @"
                SELECT 
                    te.ID AS EmpleadoID,
                    te.Nombre AS NombreEmpleado,
                    te.ApellidoPaterno,
                    te.ApellidoMaterno,
                    te.Genero,
                    te.FechaNacimiento,
                    te.CorreoElectronico,
                    te.Telefono,
                    td.ID AS DireccionID,
                    td.Numero AS DireccionNumero,
                    td.Calle AS DireccionCalle,
                    td.Colonia AS DireccionColonia,
                    td.CodigoPostal AS DireccionCodigoPostal,
                    td.Municipio AS DireccionMunicipio,
                    td.Estado AS DireccionEstado,
                    tem.ID AS EmpleadoID_Templeados,
                    tem.TSucursalID,
                    tem.TDatosEmpleadosID,
                    tem.Puesto,
                    tem.Turno,
                    tem.FechaContratacion,
                    tem.Salario,
                    tem.Acceso
                FROM 
                    tdatosempleados te
                JOIN 
                    templeados tem ON te.ID = tem.TDatosEmpleadosID
                JOIN 
                    tdireccionempleados td ON te.TDireccionEmpleadosID = td.ID
                WHERE 
                    te.ID = @EmpleadoID";

                    MySqlCommand com = new MySqlCommand(sqlQuery, connection);
                    com.Parameters.AddWithValue("@EmpleadoID", empleadoID);

                    using (MySqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Populate UI controls with employee data for editing
                            this.tbCalle.Text = reader["DireccionCalle"].ToString();
                            this.tbNumero.Text = reader["DireccionNumero"].ToString();
                            this.tbCodigoPostal.Text = reader["DireccionCodigoPostal"].ToString();
                            this.tbColonia.Text = reader["DireccionColonia"].ToString();
                            this.tbMunicipio.Text = reader["DireccionMunicipio"].ToString();
                            this.tbEstado.Text = reader["DireccionEstado"].ToString();
                            this.tbNombre.Text = reader["NombreEmpleado"].ToString();
                            this.tbApellidoPaterno.Text = reader["ApellidoPaterno"].ToString();
                            this.tbApellidoMaterno.Text = reader["ApellidoMaterno"].ToString();
                            this.tbGenero.Text = reader["Genero"].ToString();
                            this.dpFechaNacimiento.SelectedDate = Convert.ToDateTime(reader["FechaNacimiento"]);
                            this.tbEmail.Text = reader["CorreoElectronico"].ToString();
                            this.tbTelefono.Text = reader["Telefono"].ToString();
                            this.tbTurno.Text = reader["Turno"].ToString();
                            this.dpFechaContratacion.SelectedDate = Convert.ToDateTime(reader["FechaContratacion"]);
                            this.tbSalario.Text = reader["Salario"].ToString();
                            this.tbCodigoAcceso.Text = reader["Acceso"].ToString();
                            this.tbPuesto.Text = reader["Puesto"].ToString();
                            this.tbContraseña.Text = reader["Contrasena"].ToString();
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL exceptions
                Console.WriteLine("Error de MySQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Obtener el ID del empleado que deseas eliminar
                int empleadoID = ObtenerIDEmpleado(tbNombre.Text); // Reemplaza esto con la lógica real para obtener el ID del empleado

                // Validar que se haya encontrado un empleado con el nombre proporcionado
                if (empleadoID != -1)
                {
                    // Confirmar si realmente se desea eliminar al cliente
                    MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres eliminar este cliente?", "Confirmar Eliminación", MessageBoxButton.YesNo);


                    if (result == MessageBoxResult.Yes)
                    {

                        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            // Consulta para eliminar al empleado
                            string deleteQuery = @"
                                                    DELETE t, td, tdirec
                                                    FROM templeados AS t
                                                    JOIN tdatosempleados AS td ON t.TDatosEmpleadosID = td.ID
                                                    JOIN tdireccionempleados AS tdirec ON td.TDireccionEmpleadosID = tdirec.ID
                                                    WHERE td.ID = @EmpleadoID"; // Cambiado de NombreID a EmpleadoID

                            MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                            deleteCmd.Parameters.AddWithValue("@EmpleadoID", empleadoID); // Cambiado de NombreID a EmpleadoID
                            deleteCmd.ExecuteNonQuery();

                            MessageBox.Show("Empleado eliminado correctamente.");
                            Content = new Usuarios();
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

        private List<string> ObtenerCodigosAcceso()
        {
            List<string> codigosAcceso = new List<string>();

            try
            {
                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Consulta SQL para obtener todos los códigos de acceso
                string query = "SELECT Acceso FROM tclientes";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            codigosAcceso.Add(reader["Acceso"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los códigos de acceso: " + ex.Message);
            }

            return codigosAcceso;
        }

        private string GenerarCodigoAccesoAleatorio(List<string> codigosAcceso)
        {
            Random random = new Random();

            // Generar un código aleatorio de 5 cifras
            string codigoAcceso = random.Next(10000, 99999).ToString();

            // Verificar que el código generado no esté en la lista de códigos existentes
            while (codigosAcceso.Contains(codigoAcceso))
            {
                codigoAcceso = random.Next(10000, 99999).ToString();
            }

            return codigoAcceso;
        }

        private void btnGenerarCodigo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener todos los códigos de acceso existentes en la base de datos
                List<string> codigosAcceso = ObtenerCodigosAcceso();

                // Generar un nuevo código de acceso aleatorio que no exista en la base de datos
                string nuevoCodigoAcceso = GenerarCodigoAccesoAleatorio(codigosAcceso);

                // Mostrar el nuevo código de acceso generado en un MessageBox o en algún otro control de la interfaz
                tbCodigoAcceso.Text = nuevoCodigoAcceso;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el código de acceso: " + ex.Message);
            }
        }

        private void tbNumero_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}