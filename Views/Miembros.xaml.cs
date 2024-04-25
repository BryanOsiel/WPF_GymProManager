﻿using MySql.Data.MySqlClient;
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
    /// Lógica de interacción para Miembros.xaml
    /// </summary>
    public partial class Miembros : System.Windows.Controls.UserControl
    {
        public Miembros()
        {
            InitializeComponent();
            CargarDatos();
        }
        private void Agregar(object sender, RoutedEventArgs e)
        {
            CRUDMiembros ventana = new CRUDMiembros();
            FrameUsuarios.Content = ventana;
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
                    string sqlQuery = @"SELECT 
                                        m.ID AS ID_Cliente,
                                        dm.Nombre AS Nombre,
                                        dm.ApellidoPaterno AS Apellido,
                                        dm.CorreoElectronico AS Email,
                                        dm.Telefono AS Telefono,
                                        mb.TipoMembresia AS Tipo_Membresia
                                    FROM 
                                        tclientes m
                                    JOIN 
                                        tdatosclientes dm ON m.TDatosClientesID = dm.ID
                                    JOIN 
                                        tmembresias mb ON m.MembresiaID = mb.ID;";

                    // Crear un comando MySQL para ejecutar la consulta
                    MySqlCommand command = new MySqlCommand(sqlQuery, connection);

                    // Crear un adaptador de datos MySQL para ejecutar el comando y llenar un DataSet
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataSet dataSet = new System.Data.DataSet();
                    adapter.Fill(dataSet, "Clientes");

                    // Asignar el DataSet como origen de datos del control (por ejemplo, un DataGrid)
                    GridDatos.ItemsSource = dataSet.Tables["Clientes"].DefaultView;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error al cargar datos: " + ex.Message);
                }
            }
        }
        private void AgregarMiembro(object sender, RoutedEventArgs e)
        {
            CRUDMiembros ventana = new CRUDMiembros();
            FrameUsuarios.Content = ventana;
            ventana.btnCrear.Visibility = Visibility.Visible;
        }


        private void DisableEditControls(CRUDMiembros ventana)
        {
            // Deshabilitar todos los controles de edición en la ventana
            ventana.tbCalle.IsEnabled = false;
            ventana.tbNumero.IsEnabled = false;
            ventana.tbColonia.IsEnabled = false;
            ventana.tbCodigoPostal.IsEnabled = false;
            ventana.tbMunicipio.IsEnabled = false;
            ventana.tbEstado.IsEnabled = false;
            ventana.tbNombre.IsEnabled = false;
            ventana.tbApellidoPaterno.IsEnabled = false;
            ventana.tbApellidoMaterno.IsEnabled = false;
            ventana.tbGenero.IsEnabled = false;
            ventana.dpFechaNacimiento.IsEnabled = false;
            ventana.tbEmail.IsEnabled = false;
            ventana.tbTelefono.IsEnabled = false;
            ventana.dpFechaRegistro.IsEnabled = false;
            ventana.tbCodigoAcceso.IsEnabled = false;
            ventana.tbMembresia.IsEnabled = false;

        }

        private void EnableEditControls(CRUDMiembros ventana)
        {
            // Deshabilitar todos los controles de edición en la ventana
            ventana.tbCalle.IsEnabled = true;
            ventana.tbNumero.IsEnabled = true;
            ventana.tbColonia.IsEnabled = true;
            ventana.tbCodigoPostal.IsEnabled = true;
            ventana.tbMunicipio.IsEnabled = true;
            ventana.tbEstado.IsEnabled = true;
            ventana.tbNombre.IsEnabled = true;
            ventana.tbApellidoPaterno.IsEnabled = true;
            ventana.tbApellidoMaterno.IsEnabled = true;
            ventana.tbGenero.IsEnabled = true;
            ventana.dpFechaNacimiento.IsEnabled = true;
            ventana.tbEmail.IsEnabled = true;
            ventana.tbTelefono.IsEnabled = true;
            ventana.dpFechaRegistro.IsEnabled = true;
            ventana.tbCodigoAcceso.IsEnabled = true;
            ventana.tbMembresia.IsEnabled = true;

        }

        private void Consultar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del usuario que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDUsuarios
            CRUDMiembros ventana = new CRUDMiembros();

            // Configurar la ventana para mostrar la información del usuario consultado
            ventana.IdCliente = id;
            ventana.Consultar(id);
            ventana.Titulo.Text = "Consultar Miembro";

            // Deshabilitar todos los controles de edición en la ventana
            DisableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameUsuarios.Content = ventana;
        }

        private void Actualizar(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del usuario que se desea consultar desde el botón
            int id = (int)((Button)sender).CommandParameter;

            // Crear una instancia de la ventana CRUDMiembros
            CRUDMiembros ventana = new CRUDMiembros();

            // Configurar la ventana para mostrar la información del usuario consultado
            ventana.IdCliente = id;
            ventana.Editar(id);
            ventana.Titulo.Text = "Editar Usuario";
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

            // Crear una instancia de la ventana CRUDMiembros
            CRUDMiembros ventana = new CRUDMiembros();

            // Configurar la ventana para mostrar la información del usuario consultado
            ventana.IdCliente = id;
            ventana.Eliminar(id);
            ventana.Titulo.Text = "Eliminar Usuario";
            ventana.btnEliminar.Visibility = Visibility.Visible;


            // Deshabilitar todos los controles de edición en la ventana
            DisableEditControls(ventana);

            // Mostrar la ventana en el Frame
            FrameUsuarios.Content = ventana;
        }
    }
}
