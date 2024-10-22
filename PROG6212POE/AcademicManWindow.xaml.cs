using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PROG6212POE
{
    /// <summary>
    /// Interaction logic for FinanceManWindow.xaml
    /// </summary>
    public partial class FinanceManWindow : Window
    {
        public FinanceManWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBtn(object sender, RoutedEventArgs e)
        {
            string search=txtLectureSearch.Text;

            
            if (!string.IsNullOrEmpty(search))
            {
                // Call the method to search files and populate the DataGrid
                DataTable searchResults = Search(search);
                dataGridResults.ItemsSource = searchResults.DefaultView;
            }
            else
            {
                MessageBox.Show("Please enter a search value.");
            }
        }
        private DataTable Search(string search)
        {
            DataTable dataTable = new DataTable();

            string connectionString = "Data Source=labG9AEB3\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            string query = "SELECT LecturerId, LName, Notes FROM Lecturer WHERE LecturerId LIKE @Search";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Search", "%" + search + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);  // Fill the DataTable with the search results
                }
            }

            return dataTable;
        }
    }
}

