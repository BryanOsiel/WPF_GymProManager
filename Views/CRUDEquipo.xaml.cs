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

namespace WPF_GymProManager.Views
{
    /// <summary>
    /// Lógica de interacción para CRUDEquipo.xaml
    /// </summary>
    public partial class CRUDEquipo : Page
    {
        public CRUDEquipo()
        {
            InitializeComponent();
        }
        public int IdEquipo;

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
             Content = new Equipo();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar que ningún campo esté vacío
                if (string.IsNullOrEmpty(tbNombreEquipo.Text) ||
                    string.IsNullOrEmpty(tbCantidad.Text) ||
                    string.IsNullOrEmpty(tbDescripcion.Text) ||
                    string.IsNullOrEmpty(tbEstado.Text) ||
                    string.IsNullOrEmpty(tbMarca.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                // Validar que el campo de cantidad solo contenga números.
                if (!tbCantidad.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El campo de cantidad debe contener solo números.");
                    return;
                }

                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para obtener el ID de la sucursal
                    string querySucursal = "SELECT ID FROM tsucursales WHERE NombreSucursal = @NombreSucursal";
                    MySqlCommand cmdSucursal = new MySqlCommand(querySucursal, connection);
                    cmdSucursal.Parameters.AddWithValue("@NombreSucursal", "Arnold Gym");

                    // Ejecutar la consulta y obtener el ID de la sucursal
                    int sucursalID = Convert.ToInt32(cmdSucursal.ExecuteScalar());

                    // Consulta para insertar un nuevo equipo
                    string queryInsert = @"INSERT INTO tequipos (TSucursalID, NombreEquipo, Cantidad, Descripcion, Estado, Marca) 
                                   VALUES (@TSucursalID, @NombreEquipo, @Cantidad, @Descripcion, @Estado, @Marca)";

                    // Crear el comando SQL
                    MySqlCommand commandInsert = new MySqlCommand(queryInsert, connection);

                    // Establecer los parámetros
                    commandInsert.Parameters.AddWithValue("@TSucursalID", sucursalID);
                    commandInsert.Parameters.AddWithValue("@NombreEquipo", tbNombreEquipo.Text);
                    commandInsert.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(tbCantidad.Text));
                    commandInsert.Parameters.AddWithValue("@Descripcion", tbDescripcion.Text);
                    commandInsert.Parameters.AddWithValue("@Estado", tbEstado.Text);
                    commandInsert.Parameters.AddWithValue("@Marca", tbMarca.Text);

                    // Ejecutar la consulta
                    int rowsAffected = commandInsert.ExecuteNonQuery();

                    // Verificar si se insertó correctamente
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Equipo creado exitosamente.");
                        Content = new Equipo();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo crear el equipo.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el equipo: " + ex.Message);
            }
        }

        public void Consultar(int equipoID)
        {
            try
            {
                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para obtener información detallada del equipo
                    string query = @"
                SELECT 
                    te.ID AS EquipoID,
                    te.NombreEquipo,
                    te.Cantidad,
                    te.Descripcion,
                    te.Estado,
                    te.Marca
                FROM 
                    tequipos te
                JOIN 
                    tsucursales ts ON te.TSucursalID = ts.ID
                WHERE 
                    te.ID = @EquipoID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EquipoID", equipoID);

                    // Ejecutar la consulta y obtener el resultado en un lector de datos
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Verificar si se encontraron datos
                        {
                            // Mostrar la información del equipo en los controles de la interfaz de usuario
                            this.tbNombreEquipo.Text = reader["NombreEquipo"].ToString();
                            this.tbCantidad.Text = reader["Cantidad"].ToString();
                            this.tbDescripcion.Text = reader["Descripcion"].ToString();
                            this.tbEstado.Text = reader["Estado"].ToString();
                            this.tbMarca.Text = reader["Marca"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún equipo con el ID proporcionado.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Manejar las excepciones específicas de MySQL
                MessageBox.Show("Error de MySQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        public void Editar(int equipoID)
        {
            try
            {
                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para obtener información detallada del equipo
                    string query = @"
                SELECT 
                    te.ID AS EquipoID,
                    te.NombreEquipo,
                    te.Cantidad,
                    te.Descripcion,
                    te.Estado,
                    te.Marca,
                    ts.NombreSucursal AS NombreSucursal
                FROM 
                    tequipos te
                JOIN 
                    tsucursales ts ON te.TSucursalID = ts.ID
                WHERE 
                    te.ID = @EquipoID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EquipoID", equipoID);

                    // Ejecutar la consulta y obtener el resultado en un lector de datos
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Verificar si se encontraron datos
                        {
                            // Mostrar la información del equipo en los controles de la interfaz de usuario
                            this.tbNombreEquipo.Text = reader["NombreEquipo"].ToString();
                            this.tbCantidad.Text = reader["Cantidad"].ToString();
                            this.tbDescripcion.Text = reader["Descripcion"].ToString();
                            this.tbEstado.Text = reader["Estado"].ToString();
                            this.tbMarca.Text = reader["Marca"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún equipo con el ID proporcionado.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Manejar las excepciones específicas de MySQL
                MessageBox.Show("Error de MySQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar que ningún campo esté vacío
                if (string.IsNullOrEmpty(tbNombreEquipo.Text) ||
                    string.IsNullOrEmpty(tbCantidad.Text) ||
                    string.IsNullOrEmpty(tbDescripcion.Text) ||
                    string.IsNullOrEmpty(tbEstado.Text) ||
                    string.IsNullOrEmpty(tbMarca.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Advertencia");
                    return;
                }

                // Validar que el campo de cantidad solo contenga números.
                if (!tbCantidad.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El campo de cantidad debe contener solo números.", "Advertencia");
                    return;
                }

                int equipoID = ObtenerIDEquipo(tbNombreEquipo.Text);

                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para actualizar los datos del equipo
                    string query = @"
                UPDATE tequipos 
                SET 
                    NombreEquipo = @NombreEquipo,
                    Cantidad = @Cantidad,
                    Descripcion = @Descripcion,
                    Estado = @Estado,
                    Marca = @Marca
                WHERE 
                    ID = @EquipoID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreEquipo", tbNombreEquipo.Text);
                    command.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(tbCantidad.Text));
                    command.Parameters.AddWithValue("@Descripcion", tbDescripcion.Text);
                    command.Parameters.AddWithValue("@Estado", tbEstado.Text);
                    command.Parameters.AddWithValue("@Marca", tbMarca.Text);
                    command.Parameters.AddWithValue("@EquipoID", equipoID);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Equipo actualizado correctamente.", "Actualizacion");
                        Content = new Equipo();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el equipo.", "Actualizacion");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Manejar las excepciones específicas de MySQL
                MessageBox.Show("Error de MySQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private int ObtenerIDEquipo(string nombreEquipo)
        {
            try
            {
                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Realizar una consulta a la base de datos para obtener el ID del equipo
                string query = @"
            SELECT te.ID 
            FROM tequipos AS te
            WHERE te.NombreEquipo = @NombreEquipo";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreEquipo", nombreEquipo);

                    // Ejecutar la consulta y obtener el resultado
                    object result = command.ExecuteScalar();

                    // Verificar si se obtuvo algún resultado y convertirlo a entero
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        // Si no se encontró ningún equipo con ese nombre, retorna -1
                        return -1;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Manejar las excepciones específicas de MySQL
                MessageBox.Show("Error de MySQL: " + ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                MessageBox.Show("Error: " + ex.Message);
                return -1;
            }
        }

        public void Eliminar(int equipoID)
        {
            try
            {
                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para obtener información detallada del equipo
                    string query = @"
                SELECT 
                    te.ID AS EquipoID,
                    te.NombreEquipo,
                    te.Cantidad,
                    te.Descripcion,
                    te.Estado,
                    te.Marca,
                    ts.NombreSucursal AS NombreSucursal
                FROM 
                    tequipos te
                JOIN 
                    tsucursales ts ON te.TSucursalID = ts.ID
                WHERE 
                    te.ID = @EquipoID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EquipoID", equipoID);

                    // Ejecutar la consulta y obtener el resultado en un lector de datos
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Verificar si se encontraron datos
                        {
                            // Mostrar la información del equipo en los controles de la interfaz de usuario
                            this.tbNombreEquipo.Text = reader["NombreEquipo"].ToString();
                            this.tbCantidad.Text = reader["Cantidad"].ToString();
                            this.tbDescripcion.Text = reader["Descripcion"].ToString();
                            this.tbEstado.Text = reader["Estado"].ToString();
                            this.tbMarca.Text = reader["Marca"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún equipo con el ID proporcionado.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Manejar las excepciones específicas de MySQL
                MessageBox.Show("Error de MySQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener el ID del equipo que se desea eliminar
                int equipoID = ObtenerIDEquipo(tbNombreEquipo.Text); // Implementa esto con la lógica real para obtener el ID del equipo

                // Validar que se haya encontrado un equipo con el nombre proporcionado
                if (equipoID != -1)
                {
                    // Confirmar si realmente se desea eliminar el equipo
                    MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres eliminar este equipo?", "Confirmar Eliminación", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            // Consulta para eliminar el equipo
                            string deleteQuery = "DELETE FROM tequipos WHERE ID = @EquipoID";

                            MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                            deleteCmd.Parameters.AddWithValue("@EquipoID", equipoID);
                            deleteCmd.ExecuteNonQuery();

                            MessageBox.Show("Equipo eliminado correctamente.", "Eliminacion");
                            Content = new Equipo();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se encontró ningún equipo con ese nombre.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el equipo: " + ex.Message);
            }
        }

    }
}
