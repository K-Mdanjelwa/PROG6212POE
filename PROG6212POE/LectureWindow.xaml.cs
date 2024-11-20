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
    /// Interaction logic for LectureWindow.xaml
    /// </summary>
    
    public partial class LectureWindow : Window
    {
        public static int LecturerID;
        public static string fileName;
        public LectureWindow()
        {
            InitializeComponent();
        }
        int HoursWorked = 0;
        int HourRate = 0;
        double totalAmount = 0;




        private void submitBtn1(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            SqlConnection con = new SqlConnection(connectionString);

            con.Open();

            LecturerID = int.Parse(numLectureId.Text);
            string Name = txtName.Text;
            string Surname = txtSurname.Text;
            HoursWorked = int.Parse(numHWorked.Text);
            HourRate = int.Parse(numHRate.Text);

            float calc = HoursWorked * HourRate;

            string Notes = txtNotes.Text;

            string Query = "INSERT INTO Lecturer (LecturerID, LName, LSName, HoursWorked, HourRate, Notes)" +
                " VALUES ('" + LecturerID + "', '" + Name + "', '" + Surname + "', '" + HoursWorked + "', '" + HourRate + "','" + Notes + "')";


            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.ExecuteNonQuery();
            con.Close();
            UploadFileToDatabase(filePath);
            MessageBox.Show("Data successfully saved!!");

            string query = "UPDATE Track SET Amount = @Cost WHERE LecturerID = @LecturerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@LecturerID", LecturerID);
                    command.Parameters.AddWithValue("@Cost", totalAmount);

                    // Open the connection and execute the update
                    connection.Open();
                    command.ExecuteNonQuery();



                }

            }

            //calculate();


        }
        public string filePath;

        private void Calc()
        {
            int numberOfHours;
            double Rate;

            // Validate the inputs
            if (int.TryParse(numHWorked.Text, out numberOfHours) &&
                double.TryParse(numHRate.Text, out Rate))
            {
                totalAmount = numberOfHours * Rate;
                label.Content = totalAmount.ToString("C"); // Display total as currency
            }
            else
            {
                label.Content = string.Empty; // Clear if invalid input
            }

        }

        private void uploadBtn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|Excel Worksheets (*.xlsx)|*.xlsx";

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                try
                {
                    byte[] fileData = File.ReadAllBytes(filePath);
                    fileName = System.IO.Path.GetFileName(filePath);
                    upBtn.Content = $"Selected: {fileName}";
                    MessageBox.Show("Uploaded successfully");
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length > 10485760)
                    {
                        MessageBox.Show("Error: File size exceeds 10MB.");
                        return;
                    }
                    string fileExtension = fileInfo.Extension.ToLower();
                    if (fileExtension != ".pdf" && fileExtension != ".docx" && fileExtension != ".xlsx")
                    {
                        MessageBox.Show("Error: Only PDF, DOCX, and XLSX files are allowed.");
                        return;
                    }
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

            string status = "Pending";

            string connectionString = "Data Source = LISAKHANYA\\SQLEXPRESS; Initial Catalog = MyFormDB; Integrated Security = True; Encrypt = True; Trust Server Certificate = True";


            string query = "INSERT INTO Files (LecturerId, FileName, FileData) VALUES ('" + LecturerID + "', @FileName, @FileData);" +
                "insert into Track(LecturerId,TStatus) values('" + LecturerID + "','" + status + "')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@FileName", fileName);
                    command.Parameters.AddWithValue("@FileData", fileData);


                    connection.Open();
                    command.ExecuteNonQuery();
                }


                MessageBox.Show("File uploaded successfully!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            FinanceManWindow run2 = new FinanceManWindow();
            run2.Show();
        }

        private void PogCo(object sender, RoutedEventArgs e)
        {
            ProgramCoWindow run = new ProgramCoWindow();
            run.Show();
        }

        private void statusBtn(object sender, RoutedEventArgs e)
        {
            int lecturerId = int.Parse(txtlectSearch.Text);

            if (lecturerId != 0)
            {

                DataTable searchResults = track(lecturerId);
                dataGridResults.ItemsSource = searchResults.DefaultView;
            }
            else
            {
                MessageBox.Show("Please enter a search value.");
            }
        }
        private DataTable track(int lectId)
        {
            DataTable dataTable = new DataTable();



            string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            string query = "select a.TrackId,a.Amount, a.TStatus,b.ClaimNo,c.FileName " +
                "from Track a, Lecturer b, Files c " +
                "where a.LecturerId=b.LecturerId AND b.LecturerId=c.LecturerId and b.LecturerId=@Search " +
                "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Search", lectId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        private void Hours_Changed(object sender, TextChangedEventArgs e)
        {
            Calc();
        }

        private void Rate_Changed(object sender, TextChangedEventArgs e)
        {
            Calc();
        }


    }
}
