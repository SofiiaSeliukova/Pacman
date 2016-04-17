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
    /// Interaction logic for EnterName.xaml
    /// </summary>
    public partial class EnterName : Window
    {
        public String playerName;
        private bool flag;

        public EnterName()
        {
            InitializeComponent();
            flag = false;
            this.Closing += EnterName_Closing;
        }

        void EnterName_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            String name = EnterNameTextBox.Text;
            if (flag == false)
            {
                MessageBoxResult result = MessageBox.Show("Your score will not be saved!", "Attention", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel)
                  { e.Cancel = true; }
               this.playerName = "Noname";
               
            }
            
        }

        private void OkName_Click(object sender, RoutedEventArgs e)
        {
            String name = EnterNameTextBox.Text;
            if (name != String.Empty && name != null)
            {
                this.playerName = name;
                flag = true;
               this.Close();
            }
            else
            {
               
                this.Close();

            }
        }


    }
}
