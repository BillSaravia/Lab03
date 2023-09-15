using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=LAB1504-26\\SQLEXPRESS;Initial Catalog=Tecsup2023DB;User ID=userTecsup;Password=123456";
        private List<Productos> productosList;
        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            productosList = new List<Productos>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Productos", connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productosList.Add(new Productos
                            {
                                IdProducto = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Categoria = reader.GetString(2),
                                Precio = reader.GetDecimal(3),
                                FechaVencimiento = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
            }

            productosDataGrid.ItemsSource = productosList;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                List<Productos> filteredList = productosList.FindAll(producto =>
                    producto.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    producto.Categoria.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                );
                productosDataGrid.ItemsSource = filteredList;
            }
            else
            {
                productosDataGrid.ItemsSource = productosList;
            }
        }
    }

    public class Productos
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public Decimal Precio { get; set; }
        public DateTime FechaVencimiento { get; set; }


    }

}