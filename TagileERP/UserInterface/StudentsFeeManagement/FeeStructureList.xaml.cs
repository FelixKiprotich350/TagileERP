using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagileERP.BusinessModels.StudentFeeManagement;

namespace TagileERP.UserInterface.StudentsFeeManagement
{
    /// <summary>
    /// Interaction logic for FeeStructureList.xaml
    /// </summary>
    public partial class FeeStructureList : Page
    {
         ObservableCollection<FeeStructure> FeeStructure_List_Items = new ObservableCollection<FeeStructure>();
        readonly FeeStructure f = new FeeStructure();
        public FeeStructureList()
        {
            InitializeComponent();
        }

        private void ViewAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FeeStructure_List_Items.Clear();
                List<FeeStructure> a = f.GetFeeStructures();  
                
                if (a != null)
                {
                    FeeStructure_List_Items = new ObservableCollection<FeeStructure>(a);
                    FeeStructureListGrid.ItemsSource = null;
                    FeeStructureListGrid.Items.Clear();
                    FeeStructureListGrid.ItemsSource = FeeStructure_List_Items;
                }
                else
                {
                    MessageBox.Show("You haven't created any fee structure!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FeeStructureListGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;
                DataGridColumn c = FeeStructureListGrid.CurrentColumn;
                if (grid.SelectedItem != null&& c.DisplayIndex==1)
                {
                    var fs = grid.SelectedItem as FeeStructure;
                    ViewFeeStructureDetails v = new ViewFeeStructureDetails(fs);
                    v.ShowDialog();
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }
    }
}
