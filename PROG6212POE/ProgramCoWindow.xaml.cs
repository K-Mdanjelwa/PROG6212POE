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
    /// Interaction logic for ProgramCoWindow.xaml
    /// </summary>
    public partial class ProgramCoWindow : Window
    {
        public int search;
        public ProgramCoWindow()
        {
            InitializeComponent();
        }





        private void searchBtn(object sender, RoutedEventArgs e)
        {
            search = int.Parse(txtSearch.Text);


            if (search != 0)
            {
                
                DataTable searchResults = searchFunction(search);
                dataGridResults.ItemsSource = searchResults.DefaultView;
            }
            else
            {
                MessageBox.Show("Please enter a search value.");
            }
        }

        private DataTable searchFunction(int search)
        {
            DataTable dataTable = new DataTable();

            string connectionString = "Data Source=labG9AEB3\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            string query = "SELECT LecturerId, LName, HoursWorked, Notes FROM Lecturer WHERE LecturerId LIKE @Search";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Search", "%" + search + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable); 
                }
            }

            return dataTable;

        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            int DWorked = int.Parse(txtDWorked.Text);


            try
            {
                update(int.Parse(txtDWorked.Text));
                MessageBox.Show($"Hours Worked updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            searchFunction(search);
        }

        private void update(int dWorked)
        {
            string connectionString = "Data Source=labG9AEB3\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

            
            string query = "UPDATE Lecturer SET HoursWorked = @HourWorked WHERE LecturerID = @LecturerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@LecturerID", search);
                    command.Parameters.AddWithValue("@HourWorked", dWorked);

                    
                    connection.Open();
                    command.ExecuteNonQuery();

                }

            }
        }

        private void approveBtn(object sender, RoutedEventArgs e)
        {
            string approve = "Approved";

            statusChange(approve);
            MessageBox.Show($"updated successfully. \nApplication Approved.");

        }

        private void rejectBtn(object sender, RoutedEventArgs e)
        {
            string reject = "Rejected";

            statusChange(reject);
            MessageBox.Show($"updated successfully. \nApplication Rejected.");
        }

        private void statusChange(string option)
        {
            string connectionString = "Data Source=labG9AEB3\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

            
            string query = "UPDATE Track SET TStatus = @Status WHERE LecturerID = @LecturerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to the SQL query
                    command.Parameters.AddWithValue("@LecturerID", search);
                    command.Parameters.AddWithValue("@Status", option);

                    
                    connection.Open();
                    command.ExecuteNonQuery();

                   
                    
                }

            }
        }

        private void goToLect(object sender, RoutedEventArgs e)
        {
            MainWindow run= new MainWindow();
            run.Show();
        }

        private void goToAcM(object sender, RoutedEventArgs e)
        {
            FinanceManWindow run= new FinanceManWindow();
            run.Show();
        }
    }
}

    
    
    

