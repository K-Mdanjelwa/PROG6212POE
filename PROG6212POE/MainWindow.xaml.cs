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

namespace PROG6212POE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*"; // You can specify file types here

            // Show dialog and get the result
            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file path and file name
                string filePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(filePath);

                // Read the file into a byte array
                byte[] fileData = File.ReadAllBytes(filePath);

                // Save the file to the database
                SaveFileToDatabase(fileName, fileData);
            }
        }

        // Method to save the file to the database
        private void SaveFileToDatabase(string fileName, byte[] fileData)
        {
            // Define the connection string (replace with your database details)
            string connectionString = "Data Source=labG9AEB3\\SQLEXPRESS;Initial Catalog=WpfApp2;Integrated Security=True;Trust Server Certificate=True";

            // Define the SQL query for inserting the file into the database
            string query = "INSERT INTO Documents (FileName, FileData) VALUES (@FileName, @FileData)";

            // Open the connection and execute the command
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FileName", fileName);
                command.Parameters.AddWithValue("@FileData", fileData);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("File uploaded successfully!");
            }
        }

      
    }
}
