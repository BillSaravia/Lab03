using System.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;
using Lab03;

class Program
{
    // Cadena de conexión a la base de datos
    public static string connectionString = "Data Source=LAB1504-26\\SQLEXPRESS;Initial Catalog=Tecsup2023DB;User ID=userTecsup;Password=123456";


    static void Main()
        {
        #region FormaDesconectada
        //Datatable
        DataTable dataTable = ListarProductosDataTable();


        Console.WriteLine("Lista de Producto:");
        foreach (DataRow row in dataTable.Rows)
        {
            Console.WriteLine($"IdProducto: {row["IdProducto"]}, Nombre: {row["Nombre"]}, Categoria: {row["Categoria"]}, Precio: {row["Precio"]},  FechaVencimiento: {row["FechaVencimiento"]}");
        }
        #endregion


        #region FormaConectada
        //Datareader
        List<Productos> productos = ListarProductos();
        foreach (var item in productos)
        {
            Console.WriteLine($"IdProducto: {item.IdProducto}, Nombre: {item.Nombre}, Categoria: {item.Categoria}, Precio: {item.Precio}, FechaVencimiento: {item.FechaVencimiento}");
        }
        #endregion
    }



    private static DataTable ListarProductosDataTable()
    {
        // Crear un DataTable para almacenar los resultados
        DataTable dataTable = new DataTable();
        // Crear una conexión a la base de datos
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Abrir la conexión
            connection.Open();

            // Consulta SQL para seleccionar datos
            string query = "SELECT * FROM Productos";

            // Crear un adaptador de datos
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);



            // Llenar el DataTable con los datos de la consulta
            adapter.Fill(dataTable);

            // Cerrar la conexión
            connection.Close();

        }
        return dataTable;
    }


    //De forma conectada
    private static List<Productos> ListarProductos()
    {
        List<Productos> productos = new List<Productos>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Abrir la conexión
            connection.Open();

            // Consulta SQL para seleccionar datos
            string query = "SELECT IdProducto,Nombre,Categoria,Precio,FechaVencimiento FROM Productos";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Verificar si hay filas
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Lista de Productos");
                        while (reader.Read())
                        {
                            // Leer los datos de cada fila

                            productos.Add(new Productos
                            {
                                IdProducto = (int)reader["IdProducto"],
                                Nombre = reader["Nombre"].ToString(),
                                Categoria = reader["Categoria"].ToString(),
                                Precio = (Decimal)reader["Precio"],
                                FechaVencimiento = (DateTime)reader["FechaVencimiento"],
                            });

                        }
                    }
                }
            }

            // Cerrar la conexión
            connection.Close();


        }
        return productos;

    }


}