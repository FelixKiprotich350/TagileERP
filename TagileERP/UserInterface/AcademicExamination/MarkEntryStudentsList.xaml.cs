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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagileERP.BusinessModels.AcademicExamination;
using TagileERP.BusinessModels.Administration;
using TagileERP.BusinessModels.StudentManagement;

namespace TagileERP.UserInterface.AcademicExamination
{
    /// <summary>
    /// Interaction logic for MarkEntryStudentsList.xaml
    /// </summary>
    public partial class MarkEntryStudentsList : Page
    {
        readonly StudentAdmission sea = new StudentAdmission();
        readonly EnrollmentClass ec = new EnrollmentClass();
        public MarkEntryStudentsList()
        {
            InitializeComponent();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Combobox_Class.Items.Clear();
                ec.GetEnrollmentClassList().ForEach(h => Combobox_Class.Items.Add(h.ClassName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonViewall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Combobox_Class.Text=="")
                {
                    MessageBox.Show("Select a class!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                List<StudentRegistration> a = new List<StudentRegistration>();
                a = sea.GetStudentsAdmissionsList();
               //a = sea.GetAdmissionsStudentsList().Where(q => q.EnrolledClass == Combobox_Class.Text).ToList();
                if (a.Count <=0)
                {
                    MessageBox.Show("No students Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Datagrid_StudentsList.Items.Clear();
                Datagrid_StudentsList.ItemsSource = a;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Datagrid_StudentsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGrid d = sender as DataGrid;
                if (d.SelectedItem != null)
                {
                    StudentAdmission s = (StudentAdmission)d.SelectedItem;
                    MarkEntry m = new MarkEntry();
                    m.Textbox_FullName.Text = s.FirstName + " " + s.MiddleName + " " + s.LastName;
                    m.Textbox_StdRegNo.Text = s.AdmissionNumber;
                    m.Textbox_Gender.Text = s.Gender;
                    m.Textbox_Programme.Text = s.Programme;
                    m.Textbox_StudyYear.Text = s.CurrentYearofStudy;
                    m.Textbox_StudyClass.Text = s.EnrolledClass;

                    m.ShowDialog();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
