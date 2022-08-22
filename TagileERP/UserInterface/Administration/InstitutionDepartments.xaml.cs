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
    /// Interaction logic for InstitutionDepartments.xaml
    /// </summary>
    public partial class InstitutionDepartments : Page
    {
        ObservableCollection<InstitutionDepartment> DepartmentsGrid_items = new ObservableCollection<InstitutionDepartment>();
        readonly InstitutionDepartment ad = new InstitutionDepartment();
        public InstitutionDepartments()
        {
            InitializeComponent();
        }

        private void Button_ViewAll_Departments_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DepartmentsGrid_items.Clear();
                DepartmentsGrid_items = new ObservableCollection<InstitutionDepartment>(ad.GetDepartmentsList());
                if (DepartmentsGrid_items.Count<=0)
                {
                    MessageBox.Show("No Departments Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                } 
                Datagrid_DepartmentsList.ItemsSource = null;
                Datagrid_DepartmentsList.Items.Clear();
                Datagrid_DepartmentsList.ItemsSource = DepartmentsGrid_items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         

        private void TextBox_Searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = sender as TextBox;
                string filter = t.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_DepartmentsList.ItemsSource);
                if (filter == "")
                {
                    cv.Filter = null;
                }
                else
                {
                    cv.Filter = o =>
                    {
                        InstitutionDepartment p = o as InstitutionDepartment;
                        return p.DepartmentCode.ToLower().Contains(filter.ToLower());
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            AddDepartment ad = new AddDepartment();
            ad.ShowDialog();
        }
    }
}
