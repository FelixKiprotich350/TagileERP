using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using TagileERP.BusinessModels.StudentManagement;

namespace TagileERP.UserInterface.StudentsFeeManagement
{
    /// <summary>
    /// Interaction logic for IndividualStudentFeeStatement.xaml
    /// </summary>
    public partial class IndividualStudentFeeStatement : Page
    {
        readonly StudentAdmission s = new StudentAdmission();
        readonly StudentFeeStatement sf = new StudentFeeStatement();
        ObservableCollection<StudentFeeStatement> FeeStatement = new ObservableCollection<StudentFeeStatement>();
        public IndividualStudentFeeStatement()
        {
            InitializeComponent();
        }

        private void Button_SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetPage();
                FeeStatement.Clear();
                StudentAdmission a = s.GetStudentAdmissionDetails(Textbox_SearchRegNo.Text.Trim());
                if (a != null)
                {
                    FeeStatement = new ObservableCollection<StudentFeeStatement>(sf.GetStudentFeeStatement(a.AdmissionNumber));
                    if (FeeStatement.Count<=0)
                    {
                        MessageBox.Show("No Fee Statement Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    double totaldebit = 0;
                    double totalcredit = 0;
                    foreach (StudentFeeStatement b in FeeStatement)
                    {
                        totalcredit += b.Credit;
                        totaldebit += b.Debit;
                    }
                    Textbox_StudentRegistrationNo.Text = a.AdmissionNumber;
                    Textbox_Fullname.Text = a.FirstName + " " + a.MiddleName + " " + a.LastName;
                    TextBox_Gender.Text = a.Gender;
                    TextBox_Balance.Text = (totaldebit - totalcredit).ToString("N2");
                    Datagrid_FeeStatement.ItemsSource = null;
                    Datagrid_FeeStatement.Items.Clear();
                    Datagrid_FeeStatement.ItemsSource = FeeStatement;
                }
                else
                {
                    MessageBox.Show("The Registration Number provided does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetPage();
            }
        }

        private void ResetPage()
        {
            try
            {
                TextBox_Balance.Text = "";
                TextBox_Gender.Text = "";
                Textbox_Fullname.Text = "";
                Textbox_StudentRegistrationNo.Text = "";
                Datagrid_FeeStatement.ItemsSource = null;
                Datagrid_FeeStatement.Items.Clear();
                FeeStatement.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_PrintStatement_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            //if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            //{
            //    Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
            //    // sizing of the element.
            //    Datagrid_FeeStatement.Measure(pageSize);
            //    Datagrid_FeeStatement.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
            //    Printdlg.PrintVisual(Datagrid_FeeStatement, Title);
            //}
            
        }
    } 
}
