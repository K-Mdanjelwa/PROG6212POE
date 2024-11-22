using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private string connectionString = "Data Source=LISAKHANYA\\SQLEXPRESS;Initial Catalog=MyFormDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text = "Please enter both username and password.";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); 

                    var role = command.ExecuteScalar() as string;

                    if (role != null)
                    {
                        lblMessage.Text = "Login successful!";
                        OpenRoleWindow(role);
                    }
                    else
                    {
                        lblMessage.Text = "Invalid credentials.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
            }
        }

        private void OpenRoleWindow(string role)
        {
            Window roleWindow = role switch
            {
                "Lecturer" => new LectureWindow(),
                "Programme Coordinator" => new ProgramCoWindow(),
                "Academic Manager" => new FinanceManWindow(),
                "HR" => new HRWindow(),
                _ => null
            };

            roleWindow?.Show();
            this.Close();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow m= new RegisterWindow();
            m.Show();
            this.Close();
        }
    }


}

