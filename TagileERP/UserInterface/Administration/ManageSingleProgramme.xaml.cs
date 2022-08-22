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
using System.Windows.Shapes;
using TagileERP.BusinessModels.AcademicsandExamination;
using TagileERP.BusinessModels.Administration;

namespace TagileERP.UserInterface.Administration
{
    /// <summary>
    /// Interaction logic for ManageSingleProgramme.xaml
    /// </summary>
    public partial class ManageSingleProgramme : Window
    {
        readonly AcademicProgramme SelectedProgramme = null;
         InstitutionDepartment dep = new InstitutionDepartment();
         ObservableCollection<ProgrammeSubject> subjlist_View = new ObservableCollection<ProgrammeSubject>();
         ObservableCollection<ProgrammeSubject> subjlist_Edit = new ObservableCollection<ProgrammeSubject>();
        public ManageSingleProgramme(AcademicProgramme x)
        {
            InitializeComponent();
            SelectedProgramme = x;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dep = dep.GetDepartmentDetails(SelectedProgramme.ProgrammeDepartment);
            subjlist_View= new ObservableCollection<ProgrammeSubject>(SelectedProgramme.GetAcademicProgrammesSubjects(SelectedProgramme.ProgrammeCode)); 
            subjlist_Edit= new ObservableCollection<ProgrammeSubject>(SelectedProgramme.GetAcademicProgrammesSubjects(SelectedProgramme.ProgrammeCode)); 
            //get departments
            Combobox_Department.Items.Clear();
            Combobox_Department.ItemsSource = dep.GetDepartmentsList(); 
            //view tab
            Textbox_View_Code.Text = SelectedProgramme.ProgrammeCode;
            Textbox_View_ProgrammeName.Text = SelectedProgramme.GetAcademicProgramme(SelectedProgramme.ProgrammeCode).ProgrammeName;
            Textbox_View_Award.Text = SelectedProgramme.ProgrammeAward;
            Textbox_View_Department.Text = dep.DepartmentName;
            Textbox_View_Status.Text = SelectedProgramme.ProgrammeEnrollmentStatus;
            Textbox_View_StudyMode.Text = SelectedProgramme.ProgrammeStudyMode;
            Datagrid_View_SubjectsList.ItemsSource = subjlist_View;
            //edit tab
            Textbox_Code.Text = SelectedProgramme.ProgrammeCode;
            Textbox_ProgrammeName.Text = SelectedProgramme.GetAcademicProgramme(SelectedProgramme.ProgrammeCode).ProgrammeName;
            Combobox_Award.SelectedItem = Combobox_Award.Items.Cast<ComboBoxItem>().FirstOrDefault(k => k.Content.ToString() == SelectedProgramme.ProgrammeAward);
            Combobox_Status.Text = SelectedProgramme.ProgrammeEnrollmentStatus;
            Combobox_CourseStudyMode.Text = SelectedProgramme.ProgrammeStudyMode;
            Datagrid_SubjectsList.ItemsSource = subjlist_Edit;
            Combobox_Department.SelectedItem = Combobox_Department.Items.OfType<InstitutionDepartment>().FirstOrDefault(textBlock => textBlock.DepartmentCode == dep.DepartmentCode);
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectSubject s = new SelectSubject();
                bool? f = s.ShowDialog();
                if ((bool)f && s.finalsubjects != null)
                {
                    foreach (SubjectMaster m in s.finalsubjects)
                    {
                        if (!(subjlist_Edit.Where(k => k.SubjectCode == m.SubjectCode).Count() > 0))
                        {
                            subjlist_Edit.Add(new ProgrammeSubject() { SubjectCode = m.SubjectCode, SubjectName = m.SubjectName, StudyYear = s.finallevel.TransitLevel });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
