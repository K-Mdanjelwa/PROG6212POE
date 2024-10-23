using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PROG6212POE
{
    /// <summary>
    /// Interaction logic for FinanceManWindow.xaml
    /// </summary>
    public partial class FinanceManWindow : Window
    {
        public static string search;
        public FinanceManWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBtn(object sender, RoutedEventArgs e)
        {
            search = txtLectureSearch.Text;


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

        private void calcBtn(object sender, RoutedEventArgs e)
        {
            int amount = int.Parse(numRate.Text);

            if (amount != 0)
            {
                try
                {
                    calculateAmount(int.Parse(numRate.Text));
                    MessageBox.Show($"Lecturer name updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show($"Please enter a valid ");
            }
        }




        private void calculateAmount(int amount)
        {
            string connectionString = "Data Source=labG9AEB3\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

            // SQL query to update the lecturer name
            string query = "UPDATE Lecturer SET HourRate = @HourRate WHERE LecturerID = @LecturerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to the SQL query
                    command.Parameters.AddWithValue("@LecturerID", search);
                    command.Parameters.AddWithValue("@HourRate", amount);

                    // Open the connection and execute the update
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if any row was updated
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Lecturer name updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No lecturer found with the provided ID.");
                    }
                }


            }
        }
    }
}

