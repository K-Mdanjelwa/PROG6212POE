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
    /// Interaction logic for ProgramCoWindow.xaml
    /// </summary>
    public partial class ProgramCoWindow : Window
    {
        public ProgramCoWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProgramCoWindow AcWindow = new ProgramCoWindow();
            AcWindow.Show();
        }
    }
}
