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
using TagileERP.BusinessModels.AcademicExamination;

namespace TagileERP.UserInterface.AcademicExamination
{
    /// <summary>
    /// Interaction logic for ManageEnrollmentClasses.xaml
    /// </summary>
    public partial class ManageEnrollmentClasses : Page
    {
        readonly EnrollmentClass EC = new EnrollmentClass();
        ObservableCollection<EnrollmentClass> classes = new ObservableCollection<EnrollmentClass>();
        public ManageEnrollmentClasses()
        {
            InitializeComponent();
        }

        private void ButtonRefreshClasses_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Datagrid_ClassesList.ItemsSource = null;
                Datagrid_ClassesList.Items.Clear();
                classes = new ObservableCollection<EnrollmentClass>(EC.GetEnrollmentClassList()); 
                Datagrid_ClassesList.ItemsSource= classes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAddNewClass_Click(object sender, RoutedEventArgs e)
        {
            AddEnrollmentClass ae = new AddEnrollmentClass();
            ae.ShowDialog();
        }

        private void ButtonPromoteClasses_Click(object sender, RoutedEventArgs e)
        {
            PromoteEnrollmentClass pe = new PromoteEnrollmentClass();
            pe.ShowDialog();
        }
    }
}
