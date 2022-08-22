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

namespace TagileERP.UserInterface.AcademicExamination
{
    /// <summary>
    /// Interaction logic for ExaminationsList.xaml
    /// </summary>
    public partial class ExaminationsList : Page
    {
        ObservableCollection<ExamMaster> exams = new ObservableCollection<ExamMaster>();
        ExamMaster E = new ExamMaster();
        public ExaminationsList()
        {
            InitializeComponent(); 
        }

        private void ButtonViewall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                exams.Clear(); 
                exams = new ObservableCollection<ExamMaster>(E.GetExamsList());

                if (exams.Count>0)
                { 
                    Datagrid_ExaminationList.ItemsSource = null;
                    Datagrid_ExaminationList.Items.Clear();
                    Datagrid_ExaminationList.ItemsSource = exams;
                }
                else
                {
                    MessageBox.Show("No Examinations Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        private void ButtonAddNewExam_Click(object sender, RoutedEventArgs e)
        {
            NewExamination ne = new NewExamination();
            ne.ShowDialog();
        }
    }
}
