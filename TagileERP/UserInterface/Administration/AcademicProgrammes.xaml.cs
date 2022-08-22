using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using TagileERP.BusinessModels.Administration;

namespace TagileERP.UserInterface.Administration
{
    /// <summary>
    /// Interaction logic for AcademicProgrammes.xaml
    /// </summary>
    public partial class AcademicProgrammes : Page
    {
        readonly AcademicProgramme ap = new AcademicProgramme();
         ObservableCollection<AcademicProgramme> AcademicProgrammeGrid_items = new ObservableCollection<AcademicProgramme>();
        public AcademicProgrammes()
        {
            InitializeComponent();
        }

        private void ButtonAddProgramme_Click(object sender, RoutedEventArgs e)
        {
            AddAcademicProgramme AA = new AddAcademicProgramme();
            AA.Show();
        }
        private void TextBox_Searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = sender as TextBox;
                string filter = t.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_ProgrammesList.ItemsSource);
                if (filter == "")
                {
                    cv.Filter = null;
                }
                else
                {
                    cv.Filter = o =>
                    {
                        AcademicProgramme p = o as AcademicProgramme;
                        return p.ProgrammeDepartment.ToLower().Contains(filter.ToLower());
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ViewAll_Programmes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AcademicProgrammeGrid_items.Clear();
                AcademicProgrammeGrid_items = new ObservableCollection<AcademicProgramme>(ap.GetAcademicProgrammesList()); 
                if (AcademicProgrammeGrid_items.Count<=0)
                {
                    MessageBox.Show("No Programme Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                } 
                Datagrid_ProgrammesList.ItemsSource = null;
                Datagrid_ProgrammesList.Items.Clear();
                Datagrid_ProgrammesList.ItemsSource = AcademicProgrammeGrid_items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Datagrid_ProgrammesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;
                var cell= grid.CurrentCell;
                if (cell != null&&grid.SelectedItem!=null)
                {
                    //if (cell.Column.Header.ToString() == "Subjects")
                    //{

                    //}
                    AcademicProgramme ap = grid.SelectedItem as AcademicProgramme;
                    if (ap == null)
                    {
                        MessageBox.Show("The selected programme is null", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    ManageSingleProgramme ms = new ManageSingleProgramme(ap);
                    ms.ShowDialog();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }
    }
}
