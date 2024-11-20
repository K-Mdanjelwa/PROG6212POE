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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LectureWindow m=new LectureWindow();
            m.Show();
            //LoginWindow r = new LoginWindow();
            //r.Show();
            this.Close();
        }
    }
}
