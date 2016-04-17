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
using System.Data;
using System.Data.Entity;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Records.xaml
    /// </summary>
    public partial class Records : Window
    {
        public PlayerContext db;
        public Records()
        {
            InitializeComponent();
            db = new PlayerContext();
            db.Players.Load();
            Table.DataContext = db.Players.Local.ToBindingList();
        }
    }
}
