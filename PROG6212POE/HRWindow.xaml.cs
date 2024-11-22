using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// Interaction logic for HRWindow.xaml
    /// </summary>
    public partial class HRWindow : Window
    {
        public HRWindow()
        {
            InitializeComponent();
            LoadDataGrid();
        }
        string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        private void SearchClaims_Click(object sender, RoutedEventArgs e)
        {

        }


        private void LoadDataGrid()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            L.LName, 
                            L.LSName, 
                            T.TStatus, 
                            (L.HoursWorked * L.HourRate) AS Amount, 
                            L.LecturerId
                        FROM Lecturer L
                        JOIN Track T ON L.LecturerId = T.LecturerId";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridLecturers.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void UpdateLecturer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int lecturerId;
                if (!int.TryParse(txtLecturerId.Text, out lecturerId))
                {
                    MessageBox.Show("Please enter a valid Lecturer ID.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE Lecturer
                        SET LName = @LName, LSName = @LSName
                        WHERE LecturerId = @LecturerId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@LecturerId", lecturerId);
                    cmd.Parameters.AddWithValue("@LName", txtLName.Text);
                    cmd.Parameters.AddWithValue("@LSName", txtLSName.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Lecturer updated successfully.");
                        LoadDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("No Lecturer found with the given ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating lecturer: {ex.Message}");
            }
        }

        private void DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button deleteButton = sender as Button;
                int lecturerId = Convert.ToInt32(deleteButton.Tag);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        DELETE FROM Track WHERE LecturerId = @LecturerId;
                        DELETE FROM Files WHERE LecturerId = @LecturerId;
                        DELETE FROM Lecturer WHERE LecturerId = @LecturerId;";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@LecturerId", lecturerId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully.");
                        LoadDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("No record found with the given ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting record: {ex.Message}");
            }
        }









        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Example: Generating a report for approved claims
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to fetch approved claims
                    string query = @"
                SELECT 
                    T.TrackId,
                    L.LecturerId,
                    L.LName + ' ' + L.LSName AS LecturerName,
                    L.HoursWorked,
                    L.HourRate,
                    (L.HoursWorked * L.HourRate) AS TotalAmount,
                    T.TStatus
                FROM Lecturer L
                JOIN Track T ON L.LecturerId = T.LecturerId
                WHERE T.TStatus = 'Approved'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Generate report content
                    string report = "Approved Claims Report\n\n";
                    report += "Track ID, Lecturer ID, Lecturer Name, Hours Worked, Hourly Rate, Total Amount, Status\n";

                    while (reader.Read())
                    {
                        report += $"{reader["TrackId"]}, {reader["LecturerId"]}, {reader["LecturerName"]}, " +
                                  $"{reader["HoursWorked"]}, {reader["HourRate"]}, {reader["TotalAmount"]}, {reader["TStatus"]}\n";
                    }

                    // Display the report in a message box
                    MessageBox.Show(report, "Report");

                    // Save report to a file
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Text files (*.txt)|*.txt|CSV files (*.csv)|*.csv",
                        DefaultExt = "txt",
                        AddExtension = true
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        File.WriteAllText(saveFileDialog.FileName, report);
                        MessageBox.Show("Report saved successfully!", "Success");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}");
            }
        }






    }
}
