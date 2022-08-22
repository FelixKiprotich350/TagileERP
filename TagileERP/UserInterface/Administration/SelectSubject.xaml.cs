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

namespace TagileERP.UserInterface.Administration
{
    /// <summary>
    /// Interaction logic for SelectSubject.xaml
    /// </summary>
    public partial class SelectSubject : Window
    {
        readonly TransitionLevel tl = new TransitionLevel();
        public List<SubjectMaster> finalsubjects = null;
        public TransitionLevel finallevel = null;
        //
        readonly InstitutionDepartment Adepartment = new InstitutionDepartment();
        readonly SubjectMaster sm = new SubjectMaster();
        ObservableCollection<SubjectMaster> SubjectMasterGrid_items = new ObservableCollection<SubjectMaster>();
        public SelectSubject()
        {
            InitializeComponent(); 
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Combobox_Department.Items.Clear();
                Combobox_Department.ItemsSource = Adepartment.GetDepartmentsList();
                tl.GetAllProgressLevel().ForEach(g => Combobox_StudyYear.Items.Add(g));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void Button_AddSubjects_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (Combobox_StudyYear.SelectedItem != null)
                {

                   // finalsubjects = new List<SubjectMaster>();
                    finalsubjects = SubjectMasterGrid_items.Where(k => k.IsSelected == true).ToList(); 
                    finallevel = Combobox_StudyYear.SelectedItem as TransitionLevel;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show(this, "You must select THE Staudy Year!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Datagrid_SubjectsList.ItemsSource = null;
                Datagrid_SubjectsList.Items.Clear();
                if (Combobox_Department.SelectedItem != null)
                {
                    InstitutionDepartment d = Combobox_Department.SelectedItem as InstitutionDepartment;
                    SubjectMasterGrid_items = new ObservableCollection<SubjectMaster>(sm.GetSubjectsList().Where(k => k.Department == d.DepartmentCode));
                }
                else
                {
                    SubjectMasterGrid_items = new ObservableCollection<SubjectMaster>(sm.GetSubjectsList());
                }
                Datagrid_SubjectsList.ItemsSource = SubjectMasterGrid_items;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

       
    }
}
