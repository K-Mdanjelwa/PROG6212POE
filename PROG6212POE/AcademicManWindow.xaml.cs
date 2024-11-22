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

            DataTable pendingResults = pendingClaims();
            dataGridResults3.ItemsSource = pendingResults.DefaultView;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBtn(object sender, RoutedEventArgs e)
        {
            search = txtLectureSearch.Text;


            if (!string.IsNullOrEmpty(search))
            {
                
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

            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            string query = "SELECT LecturerId, LName, ClaimNo, Notes FROM Lecturer WHERE LecturerId LIKE @Search";

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

        private void updateBtn(object sender, RoutedEventArgs e)
        {
            int amount = int.Parse(numRate.Text);

            if (amount != 0)
            {
                try
                {
                    update(int.Parse(numRate.Text));
                    MessageBox.Show($"Hour Rate updated successfully.");
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




        private void update(int amount)
        {
            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

            
            string query = "UPDATE Lecturer SET HourRate = @HourRate WHERE LecturerID = @LecturerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@LecturerID", search);
                    command.Parameters.AddWithValue("@HourRate", amount);

                   
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    
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

        private void calcBtn(object sender, RoutedEventArgs e)
        {
           
                DataTable searchResults2 = claimAmount();
                dataGridResults2.ItemsSource = searchResults2.DefaultView;
            

        }
        private DataTable claimAmount ()
        {
            DataTable dataTable = new DataTable();

            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            string query = "select LecturerId, LName,LSName,ClaimNo,HoursWorked*HourRate AS Total " +
                "from Lecturer";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable); 
                }
            }

            return dataTable;
        }

        private DataTable pendingClaims()
        {
            DataTable dataTable = new DataTable();

            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            string query = "select * from Track where TStatus='Pending';";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {


                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);  
                }
            }

            return dataTable;
        }



        private void approveBtn_Click(object sender, RoutedEventArgs e)
        {
            string accept = "Approved";

            statusChange(accept);
            MessageBox.Show($"updated successfully. \nApplication Accepted.");
        }

        private void rejectBtn_Click(object sender, RoutedEventArgs e)
        {
            string reject = "Rejected";

            statusChange(reject);
            MessageBox.Show($"updated successfully. \nApplication Rejected.");
        }
        private void statusChange(string option)
        {
            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

            
            string query = "UPDATE Track SET TStatus = @Status WHERE LecturerID = @LecturerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@LecturerID", search);
                    command.Parameters.AddWithValue("@Status", option);

                    // Open the connection and execute the update
                    connection.Open();
                    command.ExecuteNonQuery();

                    

                }

            }
        }

        private void goToLecturer(object sender, RoutedEventArgs e)
        {
            LoginWindow m = new LoginWindow();
            m.Show();
            this.Close();
        }


        

    }
}

