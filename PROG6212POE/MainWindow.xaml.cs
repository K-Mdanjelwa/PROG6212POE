using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Diagnostics.Metrics;

namespace PROG6212POE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int LecturerID;
        public static string fileName;
        public MainWindow()
        {
            InitializeComponent();
        }

     

        // Method to save the file to the database
       

        private void submitBtn1(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=labG9AEB3\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            SqlConnection con=new SqlConnection(connectionString);

            con.Open();

            LecturerID = int.Parse(numLectureId.Text);
            string Name=txtName.Text;
            string Surname=txtSurname.Text;
            int HoursWorked = int.Parse(numHWorked.Text);
            int HourRate=int.Parse(numHRate.Text);
            
            string Notes = txtNotes.Text;

            string Query= "INSERT INTO Lecturer (LecturerID, LName, LSName, HoursWorked, HourRate, Notes)" +
                " VALUES ('" + LecturerID + "', '" + Name + "', '" + Surname + "', '"+HoursWorked+ "', '" + HourRate + "','"+Notes+"')";

            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Data successfully saved!!");

        }

        private void uploadBtn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    UploadFileToDatabase(filePath);
                    upBtn.Content = $"Uploaded: {fileName}";
                    MessageBox.Show("Uploaded successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        private void UploadFileToDatabase(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            fileName = System.IO.Path.GetFileName(filePath);

            // Database connection string (replace with your actual database details)
            string connectionString = "Data Source = labG9AEB3\\SQLEXPRESS; Initial Catalog = MyFormDB; Integrated Security = True; Encrypt = True; Trust Server Certificate = True";

            // SQL query to insert the file data
            string query = "INSERT INTO Files (LecturerId, FileName, FileData) VALUES ('" + LecturerID + "', @FileName, @FileData)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Adding parameters for FileName and FileData
                    command.Parameters.AddWithValue("@FileName", fileName);
                    command.Parameters.AddWithValue("@FileData", fileData);

                    // Open the connection and execute the query
                    connection.Open();
                    command.ExecuteNonQuery();
                }


                MessageBox.Show("File uploaded successfully!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            FinanceManWindow run2=new FinanceManWindow();
           run2.Show();
        }
    }
}
