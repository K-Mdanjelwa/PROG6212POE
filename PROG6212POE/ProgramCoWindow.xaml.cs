using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static PROG6212POE.ProgramCoWindow;

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
            load();

        }

        string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";



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

            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
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
            int DWorked;
            if (int.TryParse(txtDWorked.Text, out DWorked))
            {
                DWorked = int.Parse(txtDWorked.Text);
            }
            else
            {
                MessageBox.Show($"Enter valid format");
            }


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
            calculate();
            load();
            
        }


        private void update(int dWorked)
        {
            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

          
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
            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

            
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
        double totalPay = 0;
        //<Button Click = "LoadAndUpdateData_Click" Content="Refresh" HorizontalAlignment="Left" Margin="452,222,0,0" VerticalAlignment="Top" Width="51"/>
        private void calculate()
        {
            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            try
            {
                // Hardcoded Employee ID
                int employeeID = int.Parse(txtSearch.Text);

                // Variables to store HoursWorked and HourlyRate
                int hoursWorked;
                int hourlyRate;

                // Fetch data from the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT HoursWorked, HourRate FROM Lecturer WHERE LecturerId = @EmployeeID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hoursWorked = reader.GetInt32(reader.GetOrdinal("HoursWorked"));
                                hourlyRate = reader.GetInt32(reader.GetOrdinal("HourRate"));

                            }
                            else
                            {
                                MessageBox.Show("Employee not found.");
                                return;
                            }
                        }
                    }
                }

                // Calculate Total Pay
                totalPay = hoursWorked * hourlyRate;
                cala();

               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void cala()
        {
            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

            string query = "update Track set Amount=@pay where LecturerId=@LecID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to the SQL query
                    command.Parameters.AddWithValue("@LecID", search);
                    command.Parameters.AddWithValue("@pay", totalPay);


                    connection.Open();
                    command.ExecuteNonQuery();



                }

            }
        }



        private void goToLect(object sender, RoutedEventArgs e)
        {
            LoginWindow m = new LoginWindow();
            m.Show();
            this.Close();

        }

      

        

        public class Lecturer
        {
            public int LecturerId { get; set; }
            public string LName { get; set; }
            public string LSName { get; set; }
            public int HoursWorked { get; set; }
            public int HourRate { get; set; }
            public string Notes { get; set; }
            public int ClaimNo { get; set; }
            public string Mail { get; set; }
        }

        public class Track
        {
            public int TrackId { get; set; }
            public string TStatus { get; set; }
            public int LecturerId { get; set; }
            public int Amount { get; set; }
        }

        private void LoadAndUpdateData_Click(object sender, RoutedEventArgs e)
        {
            load();
        }

        private void load()
        {
            try
            {
                // Fetch data from the database
                var lecturers = GetLecturers();
                var tracks = GetTracks();

                // Business logic: Update TStatus and Amount in-memory
                foreach (var lecturer in lecturers)
                {
                    var track = tracks.FirstOrDefault(t => t.LecturerId == lecturer.LecturerId);
                    if (track != null)
                    {
                        if (lecturer.HoursWorked >= 50)
                        {
                            track.TStatus = "Approved";
                        }
                        track.Amount = lecturer.HoursWorked * lecturer.HourRate;
                    }
                }

                // Display updated data in DataGrid
                dataGridResults2.ItemsSource = tracks;

                // Optional: Update the database with the modified track data
                UpdateTracks(tracks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error 1: {ex.Message}");
            }
        }
        
        private List<Lecturer> GetLecturers()
        {
            var lecturers = new List<Lecturer>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Lecturer";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lecturers.Add(new Lecturer
                    {
                        LecturerId = reader.GetInt32(reader.GetOrdinal("LecturerId")),
                        LName = reader.GetString(reader.GetOrdinal("LName")),
                        LSName = reader.GetString(reader.GetOrdinal("LSName")),
                        HoursWorked = reader.IsDBNull(reader.GetOrdinal("HoursWorked")) ? 0 : reader.GetInt32(reader.GetOrdinal("HoursWorked")),
                        HourRate = reader.IsDBNull(reader.GetOrdinal("HourRate")) ? 0 : reader.GetInt32(reader.GetOrdinal("HourRate")),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                        ClaimNo = reader.GetInt32(reader.GetOrdinal("ClaimNo")),
                        Mail = reader.IsDBNull(reader.GetOrdinal("Mail")) ? null : reader.GetString(reader.GetOrdinal("Mail"))
                    });
                }
            }
            return lecturers;
        }

        // Fetch Track data
        private List<Track> GetTracks()
        {
            var tracks = new List<Track>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "select * from Track where TStatus='Approved'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tracks.Add(new Track
                    {
                        TrackId = reader.GetInt32(reader.GetOrdinal("TrackId")),
                        TStatus = reader.GetString(reader.GetOrdinal("TStatus")),
                        LecturerId = reader.GetInt32(reader.GetOrdinal("LecturerId")),
                        Amount = reader.GetInt32(reader.GetOrdinal("Amount"))
                    });
                }
            }
            return tracks;
        }

        private void UpdateTracks(List<Track> tracks)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var track in tracks)
                {
                    string query = "UPDATE Track SET TStatus = @TStatus, Amount = @Amount WHERE TrackId = @TrackId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TStatus", track.TStatus);
                    command.Parameters.AddWithValue("@Amount", track.Amount);
                    command.Parameters.AddWithValue("@TrackId", track.TrackId);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}

    
    
    

