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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for LoadLibraries.xaml
    /// </summary>
    public partial class LoadLibraries : Window
    {
        public int? selectedIndex;
        public LoadLibraries()
        {
            selectedIndex = null;
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (Libraries.SelectedItem != null)
            {
                selectedIndex = Libraries.SelectedIndex;
            }
            this.Close();
        }

        private void CANCELButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
