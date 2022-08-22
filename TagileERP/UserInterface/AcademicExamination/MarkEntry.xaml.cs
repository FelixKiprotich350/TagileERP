using MySql.Data.MySqlClient;
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
using TagileERP.BusinessModels.AcademicsandExamination;

namespace TagileERP.UserInterface.AcademicExamination
{
    /// <summary>
    /// Interaction logic for MarkEntry.xaml
    /// </summary>
    public partial class MarkEntry : Window
    {
        readonly ExamMaster em = new ExamMaster();
        readonly ProgrammeSubject ps = new ProgrammeSubject();
        public MarkEntry()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Combobox_Examination.Items.Clear();
                Combobox_Examination.ItemsSource = em.GetExamsList();
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
                MySqlTransaction Tr;
                if (Textbox_StdRegNo.Text == "")
                {
                    MessageBox.Show("The Student Registration number is required!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_Subject.SelectedItem == null)
                {
                    MessageBox.Show("You must Select the Subject!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_MarkType.Text == "")
                {
                    MessageBox.Show("You must Select the MarkType!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(Textbox_Markscored.Text,out int marks))
                {
                    MessageBox.Show("The MarkScore entered is not allowed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (marks<=0)
                {
                    MessageBox.Show("The MarkScore entered is not allowed. Must be greater than Zero!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_MarkType.Text=="CAT 1"|| Combobox_MarkType.Text == "CAT 2")
                {
                    if (marks > 30)
                    {
                        MessageBox.Show("The MarkScore entered is not allowed. Must be less or Equal to 30!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else if (Combobox_MarkType.Text== "Main Exam")
                {
                    if (marks > 70)
                    {
                        MessageBox.Show("The MarkScore entered is not allowed. Must be less or Equal to 70!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("The selected marktype is not known!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int z = MarkPresent();
                if (z == 1 )
                {
                    MessageBox.Show("The MarkScore has already been entered!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (z == 2)
                {
                    MessageBox.Show("Failed to determine markstatus!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ProgrammeSubject sub = Combobox_Subject.SelectedItem as ProgrammeSubject;
                string id = Guid.NewGuid().ToString();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                Tr = con.BeginTransaction();
                MySqlCommand sql = new MySqlCommand("insert into examsresults(dbuid, stdregno, examname, subjectcode, marktype, scores, lastmodifieddate) values " +
                       "(@dbuid, @stdregno, @examname, @subjectcode, @marktype, @scores, @lastmodifieddate);", con, Tr);

                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@stdregno", Textbox_StdRegNo.Text);
                sql.Parameters.AddWithValue("@examname", em.GetCurrentExam().ExamName);
                sql.Parameters.AddWithValue("@subjectcode",sub.SubjectCode);
                sql.Parameters.AddWithValue("@marktype", Combobox_MarkType.Text);
                sql.Parameters.AddWithValue("@scores", marks); 
                sql.Parameters.AddWithValue("@lastmodifieddate", ErpShared.CurrentDate());
                int x = sql.ExecuteNonQuery();
                if (x == 1)
                {
                    Tr.Commit();
                    MessageBox.Show("Marks Entered. Success!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    Tr.Rollback();
                    MessageBox.Show("Failed to save the marks!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning); 
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private int MarkPresent()
        {
            int r;
            try
            {
                ProgrammeSubject sub = Combobox_Subject.SelectedItem as ProgrammeSubject;
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("SELECT * FROM examsresults where stdregno=@stdregno and examname=@examname and subjectcode=@subjectcode and marktype=@marktype;", con);
                sql.Parameters.AddWithValue("@stdregno", Textbox_StdRegNo.Text); 
                sql.Parameters.AddWithValue("@subjectcode", sub.SubjectCode);
                sql.Parameters.AddWithValue("@marktype", Combobox_MarkType.Text);
                sql.Parameters.AddWithValue("@examname", em.GetCurrentExam().ExamName);

                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    r = 1;
                }
                else
                {
                    r = 0;
                }
                con.Close();

            }
            catch (Exception ex)
            {
                r = 2;
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }
        private void Textbox_ProgressFrom_KeyDown(object sender, KeyEventArgs e)
        {
           // MessageBox.Show(e.Key.ToString());
        }

        private void Combobox_Examination_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Combobox_Subject.Items.Clear();
                Combobox_MarkType.SelectedItem=null;
                Textbox_Markscored.Text = "";
                Combobox_Subject.ItemsSource = ps.GetSubjectsList(Textbox_Programme.Text);
                if (Combobox_Subject.Items.Count==0)
                {
                    MessageBox.Show("No subjects Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
