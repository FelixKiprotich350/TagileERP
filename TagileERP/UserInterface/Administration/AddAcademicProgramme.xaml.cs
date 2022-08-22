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
using System.Windows.Shapes;
using TagileERP.BusinessModels.AcademicsandExamination;
using TagileERP.BusinessModels.Administration;

namespace TagileERP.UserInterface.Administration
{
    /// <summary>
    /// Interaction logic for AddAcademicProgramme.xaml
    /// </summary>
    public partial class AddAcademicProgramme : Window
    {
        readonly InstitutionDepartment Adepartment = new InstitutionDepartment();
        readonly ObservableCollection<ProgrammeSubject> subjlist = new ObservableCollection<ProgrammeSubject>();
        public AddAcademicProgramme()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            { 
                Combobox_Department.Items.Clear();
                Combobox_Department.ItemsSource = Adepartment.GetDepartmentsList();
                Datagrid_SubjectsList.ItemsSource = subjlist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MySqlTransaction Tr;
            try
            {
                if (Textbox_Code.Text.Trim() == "")
                {
                    MessageBox.Show(this, "Enter the Code!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (Textbox_ProgrammeName.Text.Trim() == "")
                {
                    MessageBox.Show(this, "Enter the Programe Name!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (Combobox_Award.SelectedItem == null)
                {
                    MessageBox.Show(this, "Select StudyLevel!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }  
                if (Combobox_Department.SelectedItem == null)
                {
                    MessageBox.Show(this, "Select Department!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (Combobox_Status.SelectedItem == null)
                {
                    MessageBox.Show(this, "Select Programme Status!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (Combobox_CourseStudyMode.Text.Trim() == "")
                {
                    MessageBox.Show(this, "Select Study Mode!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                string id = Guid.NewGuid().ToString();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                Tr = con.BeginTransaction();
                MySqlCommand sql = new MySqlCommand("insert into academicprogrammes (dbuid, programmecode, programmename, award, department, programmeStatus, registrationdate, studymode) values(@dbuid, @programmecode, @programmename, @award, @department, @programmeStatus, @registrationdate, @studymode);", con,Tr);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@programmecode", Textbox_Code.Text);
                sql.Parameters.AddWithValue("@programmename", Textbox_ProgrammeName.Text);
                sql.Parameters.AddWithValue("@award", Combobox_Award.Text);
                sql.Parameters.AddWithValue("@department", ((InstitutionDepartment)Combobox_Department.SelectedItem).DepartmentCode);
                sql.Parameters.AddWithValue("@programmeStatus", Combobox_Status.Text);
                sql.Parameters.AddWithValue("@registrationdate", ErpShared.CurrentDate());
                sql.Parameters.AddWithValue("@studymode",Combobox_CourseStudyMode.Text);
                
                int x = sql.ExecuteNonQuery();
                if (x > 0)
                {
                    List<ProgrammeSubject> l = subjlist.ToList();
                    sql = new MySqlCommand("insert into programmesubjects (dbuid, programmecode, yearofstudy, subjectcode, regdate,isdeleted) values(@dbuid, @programmecode, @yearofstudy, @subjectcode, @regdate,@isdeleted);", con, Tr);
                    sql.Parameters.Add("@dbuid", MySqlDbType.VarChar);
                    sql.Parameters.Add("@programmecode", MySqlDbType.VarChar);
                    sql.Parameters.Add("@yearofstudy", MySqlDbType.VarChar);
                    sql.Parameters.Add("@subjectcode", MySqlDbType.VarChar);
                    sql.Parameters.Add("@regdate", MySqlDbType.DateTime);
                    sql.Parameters.Add("@isdeleted", MySqlDbType.VarChar);
                    int y = 0;
                    foreach (ProgrammeSubject p in l)
                    {
                        sql.Parameters["@dbuid"].Value = id;
                        sql.Parameters["@programmecode"].Value = Textbox_Code.Text;
                        sql.Parameters["@yearofstudy"].Value = p.StudyYear;
                        sql.Parameters["@subjectcode"].Value = p.SubjectCode;
                        sql.Parameters["@regdate"].Value = ErpShared.CurrentDate();
                        sql.Parameters["@isdeleted"].Value = "false";
                        y += sql.ExecuteNonQuery();
                    }

                    if (y == l.Count)
                    {
                        Tr.Commit();
                        MessageBox.Show(this, "Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        Tr.Rollback();
                        MessageBox.Show(this, "Failed to save some Programme Subjects!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
                else
                {
                    MessageBox.Show(this, "Failed to save the Programme!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                SelectSubject s = new SelectSubject();
                bool? f = s.ShowDialog();
                if ((bool)f&& s.finalsubjects != null)
                {
                    foreach (SubjectMaster m in s.finalsubjects)
                    {
                        if (!(subjlist.Where(k => k.SubjectCode == m.SubjectCode).Count() > 0))
                        {
                            subjlist.Add(new ProgrammeSubject() { SubjectCode = m.SubjectCode, SubjectName = m.SubjectName, StudyYear = s.finallevel.TransitLevel });
                        }
                    }  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Combobox_CourseStudyMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ClearSubjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Combobox_Award_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ClearSubjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ClearSubjects()
        {
            try
            {
                subjlist.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
