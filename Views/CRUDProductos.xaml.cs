using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPF_GymProManager.Views
{
    public partial class CRUDProductos : Page
    {
        public CRUDProductos()
        {
            InitializeComponent();
        }
        public int IdProducto;


        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            Content = new Productos();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones de campos vacíos y formato de datos
                if (string.IsNullOrEmpty(tbNombreProducto.Text) ||
                    string.IsNullOrEmpty(tbCantidad.Text) ||
                    string.IsNullOrEmpty(tbDescripcion.Text) ||
                    string.IsNullOrEmpty(tbPrecio.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                // Validar que el campo de Stock solo contenga números.
                if (!tbCantidad.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El campo de stock debe contener solo números.");
                    return;
                }

                // Validar que el campo de orecio solo contenga números.
                if (!tbPrecio.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El campo de precio debe contener solo números.");
                    return;
                }

                // Conexión a la base de datos
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta e inserción de datos en la base de datos
                    string queryInsert = @"INSERT INTO tproductos (TProveedorID, NombreProducto, Stock, Precio, Descripcion) 
                           VALUES (@TProveedorID, @NombreProducto, @Stock, @Precio, @Descripcion)";

                    MySqlCommand commandInsert = new MySqlCommand(queryInsert, connection);
                    commandInsert.Parameters.AddWithValue("@TProveedorID", 1); // Asignar el IdProveedor correspondiente
                    commandInsert.Parameters.AddWithValue("@NombreProducto", tbNombreProducto.Text);
                    commandInsert.Parameters.AddWithValue("@Stock", Convert.ToInt32(tbCantidad.Text));
                    commandInsert.Parameters.AddWithValue("@Precio", Convert.ToDecimal(tbPrecio.Text));
                    commandInsert.Parameters.AddWithValue("@Descripcion", tbDescripcion.Text);

                    int rowsAffected = commandInsert.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Producto creado exitosamente.");
                        Content = new Productos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo crear el producto.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el producto: " + ex.Message);
            }
        }


        public void Consultar(int productoID)
        {
            try
            {
                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para obtener información detallada del producto
                    string query = @"
            SELECT 
                p.ID AS ProductoID,
                p.NombreProducto,
                p.Stock,
                p.Precio,
                p.Descripcion
            FROM 
                tproductos p
            WHERE 
                p.ID = @ProductoID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductoID", productoID);

                    // Ejecutar la consulta y obtener el resultado en un lector de datos
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Verificar si se encontraron datos
                        {
                            // Mostrar la información del producto en los controles de la interfaz de usuario
                            this.tbNombreProducto.Text = reader["NombreProducto"].ToString();
                            this.tbCantidad.Text = reader["Stock"].ToString();
                            this.tbPrecio.Text = reader["Precio"].ToString();
                            this.tbDescripcion.Text = reader["Descripcion"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con el ID proporcionado.");
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

        public void Editar(int productoID)
        {
            try
            {
                // Obtener la cadena de conexión a la base de datos
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para obtener información detallada del producto
                    string query = @"
            SELECT 
                tp.ID AS ProductoID,
                tp.NombreProducto,
                tp.Stock,
                tp.Precio,
                tp.Descripcion,
                tp.TProveedorID
            FROM 
                tproductos tp
            WHERE 
                tp.ID = @ProductoID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductoID", productoID);

                    // Ejecutar la consulta y obtener el resultado en un lector de datos
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Verificar si se encontraron datos
                        {
                            // Mostrar la información del producto en los controles de la interfaz de usuario
                            this.tbNombreProducto.Text = reader["NombreProducto"].ToString();
                            this.tbCantidad.Text = reader["Stock"].ToString();
                            this.tbPrecio.Text = reader["Precio"].ToString();
                            this.tbDescripcion.Text = reader["Descripcion"].ToString();
                            // Suponiendo que el ID del proveedor sea 1 por defecto
                            int proveedorID = reader.GetInt32("TProveedorID");
                            // Asignar el proveedor por defecto al ComboBox u otro control correspondiente
                            // this.cbProveedor.SelectedValue = proveedorID;
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con el ID proporcionado.");
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
                if (string.IsNullOrEmpty(tbNombreProducto.Text) ||
                    string.IsNullOrEmpty(tbCantidad.Text) ||
                    string.IsNullOrEmpty(tbDescripcion.Text) ||
                    string.IsNullOrEmpty(tbPrecio.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                // Validar que el campo de Stock solo contenga números.
                if (!tbCantidad.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El campo de stock debe contener solo números.");
                    return;
                }

                // Validar que el campo de orecio solo contenga números.
                if (!tbPrecio.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El campo de precio debe contener solo números.");
                    return;
                }

                int productoID = ObtenerIDProducto(tbNombreProducto.Text);

                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para actualizar los datos del producto
                    string query = @"
            UPDATE tproductos 
            SET 
                NombreProducto = @NombreProducto,
                Stock = @Stock,
                Precio = @Precio,
                Descripcion = @Descripcion
            WHERE 
                ID = @ProductoID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreProducto", tbNombreProducto.Text);
                    command.Parameters.AddWithValue("@Stock", Convert.ToInt32(tbCantidad.Text));
                    command.Parameters.AddWithValue("@Precio", Convert.ToDecimal(tbPrecio.Text));
                    command.Parameters.AddWithValue("@Descripcion", tbDescripcion.Text);
                    command.Parameters.AddWithValue("@ProductoID", productoID);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Producto actualizado correctamente.", "Actualizacion");
                        Content = new Productos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el producto.", "Actualizacion");
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
                // Obtener el ID del producto que se desea eliminar
                int productoID = ObtenerIDProducto(tbNombreProducto.Text); // Implementa esto con la lógica real para obtener el ID del producto

                // Validar que se haya encontrado un producto con el nombre proporcionado
                if (productoID != -1)
                {
                    // Confirmar si realmente se desea eliminar el producto
                    MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres eliminar este producto?", "Confirmar Eliminación", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            // Consulta para eliminar el producto
                            string deleteQuery = "DELETE FROM tproductos WHERE ID = @ProductoID";

                            MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                            deleteCmd.Parameters.AddWithValue("@ProductoID", productoID);
                            deleteCmd.ExecuteNonQuery();

                            MessageBox.Show("Producto eliminado correctamente.", "Eliminación");
                            Content = new Productos();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se encontró ningún producto con ese nombre.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el producto: " + ex.Message);
            }
        }

        private int ObtenerIDProducto(string nombreProducto)
        {
            try
            {
                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Realizar una consulta a la base de datos para obtener el ID del producto
                string query = @"
        SELECT tp.ID 
        FROM tproductos AS tp
        WHERE tp.NombreProducto = @NombreProducto";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreProducto", nombreProducto);

                    // Ejecutar la consulta y obtener el resultado
                    object result = command.ExecuteScalar();

                    // Verificar si se obtuvo algún resultado y convertirlo a entero
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        // Si no se encontró ningún producto con ese nombre, retorna -1
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

        public void Eliminar(int productoID)
        {
            try
            {
                // Obtener la cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Establecer conexión con la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para obtener información detallada del producto
                    string query = @"
            SELECT 
                p.ID AS ProductoID,
                p.NombreProducto,
                p.Stock,
                p.Precio,
                p.Descripcion
            FROM 
                tproductos p
            WHERE 
                p.ID = @ProductoID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductoID", productoID);

                    // Ejecutar la consulta y obtener el resultado en un lector de datos
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Verificar si se encontraron datos
                        {
                            // Mostrar la información del producto en los controles de la interfaz de usuario
                            this.tbNombreProducto.Text = reader["NombreProducto"].ToString();
                            this.tbCantidad.Text = reader["Stock"].ToString();
                            this.tbPrecio.Text = reader["Precio"].ToString();
                            this.tbDescripcion.Text = reader["Descripcion"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con el ID proporcionado.");
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
    }
}
