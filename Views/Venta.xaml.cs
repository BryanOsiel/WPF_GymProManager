using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para Venta.xaml
    /// </summary>
    public partial class Venta : UserControl
    {
        public ObservableCollection<Producto> Productos { get; set; }

        public Venta()
        {
            InitializeComponent();
            Productos = new ObservableCollection<Producto>(); // Inicializa la colección en el constructor
            GridProductos.ItemsSource = Productos; // Establece la colección como ItemsSource del DataGrid
        }

        private void EliminarProducto(object sender, RoutedEventArgs e)
        {
            // Verificar si hay una fila seleccionada
            if (GridProductos.SelectedItem != null)
            {
                // Obtener el producto seleccionado
                Producto productoSeleccionado = (Producto)GridProductos.SelectedItem;

                // Eliminar el producto de la colección
                Productos.Remove(productoSeleccionado);

                // Recalcular el total después de eliminar el producto
                RecalcularTotal();
            }
            else
            {
                MessageBox.Show("Por favor seleccione un producto para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RecalcularTotal()
        {
            // Calcular el total sumando los montos totales de todos los productos
            decimal total = 0;
            foreach (Producto producto in Productos)
            {
                total += producto.MontoTotal;
            }

            // Actualizar el contenido del Label lblTotal con el nuevo total
            lblTotal.Content = "Total: " + total.ToString("C");

            // Obtener el total como decimal y luego convertirlo a negativo
            decimal totalDecimal = decimal.Parse(total.ToString());
            decimal cambio = totalDecimal * -1;

            // Actualizar el contenido del Label lblCambio con el nuevo cambio
            lblCambio.Content = "Cambio: " + cambio.ToString("C");
        }



        private void AgregarProducto(object sender, RoutedEventArgs e)
        {
            // Verificar que se haya ingresado la cantidad y el precio
            if (string.IsNullOrEmpty(tbCantidad.Text.Trim()) || string.IsNullOrEmpty(tbPrecio.Text.Trim()))
            {
                MessageBox.Show("Por favor ingresa la cantidad y el precio del producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar que la cantidad ingresada sea un número válido
            if (!int.TryParse(tbCantidad.Text.Trim(), out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("La cantidad ingresada no es válida. Por favor ingresa un número entero mayor que cero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Obtener el precio unitario del producto desde el TextBox tbPrecio
            if (!decimal.TryParse(tbPrecio.Text.Trim(), out decimal precioUnitario) || precioUnitario <= 0)
            {
                MessageBox.Show("El precio del producto no es válido. Por favor ingresa un número decimal mayor que cero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Calcular el precio total
            decimal precioTotal = precioUnitario * cantidad;

            // Crear un nuevo objeto para representar el producto
            Producto nuevoProducto = new Producto
            {
                Nombre = tbNombre.Text,
                Precio = precioUnitario,
                Cantidad = cantidad,
                MontoTotal = precioTotal
            };

            // Agregar el nuevo producto a la colección
            Productos.Add(nuevoProducto);

            // Actualizar el total
            ActualizarTotal();
        }


        private void CambiarCantidad(object sender, RoutedEventArgs e)
        {
            // Verificar si hay una fila seleccionada
            if (GridProductos.SelectedItem != null)
            {
                // Obtener el producto seleccionado
                Producto productoSeleccionado = (Producto)GridProductos.SelectedItem;

                // Mostrar el InputBoxWindow para ingresar la nueva cantidad
                InputBoxWindow inputBox = new InputBoxWindow();
                inputBox.Owner = Application.Current.MainWindow;
                bool? result = inputBox.ShowDialog();

                // Verificar si se presionó Aceptar en el InputBoxWindow
                if (result == true && int.TryParse(inputBox.InputValue, out int nuevaCantidad) && nuevaCantidad > 0)
                {
                    // Actualizar la cantidad del producto seleccionado
                    productoSeleccionado.Cantidad = nuevaCantidad;

                    // Recalcular el monto total
                    productoSeleccionado.MontoTotal = productoSeleccionado.Precio * productoSeleccionado.Cantidad;

                    // Actualizar la fila en el DataGrid
                    GridProductos.Items.Refresh();

                    // Actualizar el total
                    ActualizarTotal();
                }
                else
                {
                    MessageBox.Show("La cantidad ingresada no es válida. Por favor ingresa un número entero mayor que cero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor seleccione un producto para cambiar la cantidad.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizarTotal()
        {
            decimal total = 0;
            foreach (Producto producto in GridProductos.Items)
            {
                total += producto.MontoTotal;
            }
            lblTotal.Content = "Total: " + total.ToString("C");

            // Obtener el total como decimal y luego convertirlo a negativo
            decimal totalDecimal = decimal.Parse(total.ToString());
            decimal cambio = totalDecimal * -1;

            // Actualizar el contenido del Label lblCambio con el nuevo cambio
            lblCambio.Content = "Cambio: " + cambio.ToString("C");
        }



        private void BuscarProducto(object sender, RoutedEventArgs e)
        {
            string nombreProducto = tbBuscar.Text.Trim();



            if (string.IsNullOrEmpty(nombreProducto))
            {
                MessageBox.Show("Por favor ingresa el nombre del producto a buscar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {

                // Obtener la cadena de conexión desde el archivo de configuración
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // Crear una conexión a la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT NombreProducto, Precio FROM tproductos WHERE NombreProducto LIKE @nombreProducto";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombreProducto", "%" + nombreProducto + "%");

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Si se encontró un producto, actualiza los TextBox con los resultados
                            tbNombre.Text = reader["NombreProducto"].ToString();
                            tbPrecio.Text = reader["Precio"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron productos con el nombre especificado.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AnularOrden(object sender, RoutedEventArgs e)
        {
            // Limpiar la lista de productos
            Productos.Clear();

            // Actualizar el total
            ActualizarTotal();

            lblEfectivo.Content = "Efectivo: $0.00";

            tbBuscar.Text = "";
            tbCantidad.Text = "";
            tbNombre.Text = "";
            tbPrecio.Text = "";
        }


        private void Pagar(object sender, RoutedEventArgs e)
        {
            // Verificar si hay productos en la lista
            if (GridProductos.Items.Count == 0)
            {
                MessageBox.Show("No hay productos en la lista para pagar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            // Obtener la conexión a la base de datos desde el archivo de configuración
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // Abrir una nueva conexión para la transacción
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                // Consulta para obtener el ID de la sucursal
                string query = "SELECT ID FROM tsucursales WHERE NombreSucursal = @NombreSucursal";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@NombreSucursal", "Arnold Gym");

                // Ejecutar la consulta y obtener el ID de la sucursal
                int sucursalID = Convert.ToInt32(cmd.ExecuteScalar());

                try
                {
                    foreach (Producto producto in Productos)
                    {
                        // Obtener el ID del producto
                        query = "SELECT ID FROM tproductos WHERE NombreProducto = @NombreProducto";
                        cmd = new MySqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@NombreProducto", producto.Nombre);

                        // Ejecutar la consulta y obtener el ID del producto
                        int productoID = Convert.ToInt32(cmd.ExecuteScalar());

                        // Insertar la venta del producto en la tabla tventaproducto
                        query = "INSERT INTO tventaproducto (TSucursalID, TProductoID, Cantidad, MontoTotal, FechaVenta) VALUES (@SucursalID, @ProductoID, @Cantidad, @MontoTotal, NOW())";
                        cmd = new MySqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@SucursalID", sucursalID);
                        cmd.Parameters.AddWithValue("@ProductoID", productoID);
                        cmd.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                        cmd.Parameters.AddWithValue("@MontoTotal", producto.MontoTotal);
                        cmd.ExecuteNonQuery();
                    }

                    // Commit de la transacción si se ejecutó correctamente
                    transaction.Commit();

                    // Limpiar la lista de productos
                    Productos.Clear();
                    lblEfectivo.Content = "Efectivo: $0.00";
                    tbBuscar.Text = "";
                    tbCantidad.Text = "";
                    tbNombre.Text = "";
                    tbPrecio.Text = "";

                    // Actualizar el total
                    ActualizarTotal();

                    MessageBox.Show("La venta se ha registrado correctamente.", "Venta Registrada", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Rollback de la transacción en caso de error
                    transaction.Rollback();
                    MessageBox.Show("Error al registrar la venta: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Efectivo(object sender, RoutedEventArgs e)
        {
            // Mostrar el InputBoxWindow para ingresar el monto en efectivo
            InputBoxWindow inputBox = new InputBoxWindow();
            inputBox.Owner = Application.Current.MainWindow;
            inputBox.Texto.Text = "Ingrese la cantidad de efectivo";
            bool? result = inputBox.ShowDialog();

            // Verificar si se presionó Aceptar en el InputBoxWindow
            if (result == true && decimal.TryParse(inputBox.InputValue, out decimal montoEfectivo) && montoEfectivo >= 0)
            {
                // Obtener el total actual
                decimal total = 0;
                foreach (Producto producto in GridProductos.Items)
                {
                    total += producto.MontoTotal;
                }

                // Calcular el cambio
                decimal cambio = montoEfectivo - total;


                // Actualizar el contenido del Label lblEfectivo con el monto en efectivo ingresado
                lblEfectivo.Content = "Efectivo: " + montoEfectivo.ToString("C");

                // Actualizar el contenido del Label lblCambio con el cambio calculado
                lblCambio.Content = "Cambio: " + cambio.ToString("C");
            }
            else
            {
                MessageBox.Show("El monto ingresado no es válido. Por favor ingresa un número decimal mayor o igual que cero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
