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
using System.Windows.Media.Media3D;

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
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|Excel Worksheets (*.xlsx)|*.xlsx";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    UploadFileToDatabase(filePath);
                    upBtn.Content = $"Uploaded: {fileName}";
                    MessageBox.Show("Uploaded successfully");
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length > 10485760)
                    {
                        MessageBox.Show( "Error: File size exceeds 10MB.");
                        return;
                    }
                    string fileExtension = fileInfo.Extension.ToLower();
                    if (fileExtension != ".pdf" && fileExtension != ".docx" && fileExtension != ".xlsx")
                    {
                        MessageBox.Show( "Error: Only PDF, DOCX, and XLSX files are allowed.");
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
            
            string connectionString = "Data Source = labG9AEB3\\SQLEXPRESS; Initial Catalog = MyFormDB; Integrated Security = True; Encrypt = True; Trust Server Certificate = True";

            
            string query = "INSERT INTO Files (LecturerId, FileName, FileData) VALUES ('" + LecturerID + "', @FileName, @FileData);" +
                "insert into Track(LecturerId,TStatus) values('" + LecturerID + "','"+status+"')";

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
            
            FinanceManWindow run2=new FinanceManWindow();
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

            if (lecturerId!=0)
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

            

            string connectionString = "Data Source=labG9AEB3\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            string query = "select a.TrackId, a.TStatus,b.ClaimNo,c.FileName " +
                "from Track a, Lecturer b, Files c " +
                "where a.LecturerId=b.LecturerId AND b.LecturerId=c.LecturerId and b.LecturerId=@Search " +
                "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection)) 
                {
                    command.Parameters.AddWithValue("@Search",lectId );

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);  
                }
            }

            return dataTable;
        }
    }
}
