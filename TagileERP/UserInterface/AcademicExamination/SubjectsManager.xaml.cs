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
using TagileERP.BusinessModels.AcademicsandExamination;
using TagileERP.BusinessModels.Administration;

namespace TagileERP.UserInterface.AcademicExamination
{
    /// <summary>
    /// Interaction logic for SubjectsManager.xaml
    /// </summary>
    public partial class SubjectsManager : Page
    {
        readonly InstitutionDepartment Adepartment = new InstitutionDepartment();
        readonly SubjectMaster sm = new SubjectMaster();
        ObservableCollection<SubjectMaster> SubjectMasterGrid_items = new ObservableCollection<SubjectMaster>();
        public SubjectsManager()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //departments
                Combobox_Department.Items.Clear();
                Combobox_Department.ItemsSource = Adepartment.GetDepartmentsList();
                //departments
                Combobox_FilterDepartment.Items.Clear();
                Combobox_FilterDepartment.ItemsSource = Adepartment.GetDepartmentsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_SubjectCode.Text.Trim() == "")
                {
                    MessageBox.Show( "Enter the Code!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (Textbox_SubjectName.Text.Trim() == "")
                {
                    MessageBox.Show( "Enter the Subject Name!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                } 
                if (Combobox_Department.SelectedItem == null)
                {
                    MessageBox.Show( "Select Department!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                string id = Guid.NewGuid().ToString();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("insert into subjectsmaster (dbuid, subjectcode, subjectname, department, regdate) values(@dbuid, @subjectcode, @subjectname, @department, @regdate);", con);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@subjectcode", Textbox_SubjectCode.Text);
                sql.Parameters.AddWithValue("@subjectname", Textbox_SubjectName.Text); 
                sql.Parameters.AddWithValue("@department", ((InstitutionDepartment)Combobox_Department.SelectedItem).DepartmentCode);
                sql.Parameters.AddWithValue("@regdate", ErpShared.CurrentDate());
                int x = sql.ExecuteNonQuery();
                if (x > 0)
                {
                    MessageBox.Show( "Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show( "Failed to save the Subject!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(  ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ViewAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SubjectMasterGrid_items.Clear();
                SubjectMasterGrid_items = new ObservableCollection<SubjectMaster>(sm.GetSubjectsList());
                if (SubjectMasterGrid_items.Count <= 0)
                {
                    MessageBox.Show("No Programme Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }  
                Datagrid_SubjectsList.ItemsSource = null;
                Datagrid_SubjectsList.Items.Clear();
                Datagrid_SubjectsList.ItemsSource = SubjectMasterGrid_items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
