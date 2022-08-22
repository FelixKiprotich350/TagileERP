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

namespace TagileERP
{
    /// <summary>
    /// Interaction logic for nonrectangle.xaml
    /// </summary>
    public partial class Nonrectangle : Window
    {
        public Nonrectangle()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BlackNWhiteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
