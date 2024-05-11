using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace WPF_GymProManager.Views
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            CountUsers();
            CountMembers();
            CountEquipment();
            CountSale();
        }

        private void CountUsers()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM templeados";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    lbUsuarios.Content = count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CountMembers()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM tclientes";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    lbMiembros.Content = count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CountEquipment()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM tequipos";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    lbEquipo.Content = count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CountSale()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM tventaproducto";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    lbVentas.Content = count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
