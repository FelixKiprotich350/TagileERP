using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
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
using TagileERP.BusinessModels.Administration;
using TagileERP.BusinessModels.StudentManagement;

namespace TagileERP.UserInterface.StudentsManagement
{
    /// <summary>
    /// Interaction logic for NewStudent.xaml
    /// </summary>
    public partial class NewStudent : Page
    { 
       readonly AcademicIntake Aintake = new AcademicIntake();
        readonly AcademicProgramme Aprogramme = new AcademicProgramme();
        readonly TransitionLevel TL = new TransitionLevel();
        readonly Random rr = new Random();
      
        public NewStudent()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //intake
                Combobox_AdmissionIntake.Items.Clear();
                Combobox_AdmissionIntake.ItemsSource = Aintake.GetAdmissionIntakesList().Where(j => j.IntakeStatus == "Open");
              ///  academic Programme
                Combobox_Programme.Items.Clear();
                Combobox_Programme.ItemsSource = Aprogramme.GetAcademicProgrammesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Textbox_RegNo.Text = "Auto";
            Textbox_RegNo.IsEnabled = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Textbox_RegNo.IsReadOnly = false;
            Textbox_RegNo.IsEnabled = true;
            Textbox_RegNo.Text = "";
            Textbox_RegNo.Focus();

        }

        private void ButtonBrowseImage_Click(object sender, RoutedEventArgs e)
        { 
            StdProfileImage.Source = null;
            OpenFileDialog of = new OpenFileDialog();

            if ((bool)of.ShowDialog())
            {
                try
                {
                    StdProfileImage.Source = new BitmapImage(new Uri(of.FileName.ToString(), UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                StdProfileImage.Source = null;
            }
        }

        private void Button_SaveStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Firstname.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the first Name!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Textbox_Lastname.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the Last Name!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (TextBox_NationalID.Text.Trim() == "")
                {
                    MessageBox.Show("National ID required!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (DatePicker_DOB.SelectedDate==null)
                {
                    MessageBox.Show("Select Date of Birth!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (DatePicker_DOB.SelectedDate==null)
                {
                    MessageBox.Show("Select Date of Birth!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int reg = rr.Next(5555, 9999);
                MySqlTransaction Tr;
                string id = Guid.NewGuid().ToString();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                Tr = con.BeginTransaction();
                MySqlCommand sql;
                int x = 0;
                string RegDbuid;
                StudentRegistration sr = new StudentRegistration();
                sr = sr.GetRegDetailsByNationalId(TextBox_NationalID.Text);
                if (sr != null)
                {
                    RegDbuid = sr.RegDbuid;
                    MessageBox.Show("You have been registered before. The First registration details will be used.","Message Box",MessageBoxButton.OK,MessageBoxImage.Information);
                    StudentAdmission sa = new StudentAdmission();
                    if(sa.GetAdmissionStatus(TextBox_NationalID.Text.Trim()) != "Completed")
                    {
                        return;
                    }
                    goto SkipRegistration;
                }
                sql = new MySqlCommand("insert into studentsreg (dbuid, nationalid, firstname, middlename, lastname, gender, nationality, religion, email, phone, postaddress, ispwd, birthdate, regdate, homecounty, subcounty, ward, homelocation, profilephoto, password) " +
                    "values(@dbuid,  @nationalid, @firstname, @middlename, @lastname, @gender, @nationality, @religion, @email, @phone, @postaddress, @ispwd, @birthdate, @regdate," +
                    " @homecounty, @subcounty, @ward, @homelocation, @profilephoto, @password);", con, Tr);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@nationalid", TextBox_NationalID.Text);
                sql.Parameters.AddWithValue("@firstname", Textbox_Firstname.Text);
                sql.Parameters.AddWithValue("@middlename", Textbox_MiddleName.Text);
                sql.Parameters.AddWithValue("@lastname", Textbox_Lastname.Text);
                sql.Parameters.AddWithValue("@gender", Combobox_Gender.Text);
                sql.Parameters.AddWithValue("@nationality", Combobox_Nationality.Text);
                sql.Parameters.AddWithValue("@religion", Combobox_Religion.Text);
                sql.Parameters.AddWithValue("@email", Textbox_EmailAddress.Text);
                sql.Parameters.AddWithValue("@phone", Textbox_TelephoneNo.Text);
                sql.Parameters.AddWithValue("@postaddress", TextBox_PostalAddress.Text);
                sql.Parameters.AddWithValue("@ispwd", BoolString(Checkbox_Ispwd.IsChecked));
                sql.Parameters.AddWithValue("@birthdate", DatePicker_DOB.SelectedDate);
                sql.Parameters.AddWithValue("@regdate", ErpShared.CurrentDate());
                sql.Parameters.AddWithValue("@homecounty", Combobox_County.Text);
                sql.Parameters.AddWithValue("@subcounty", Combobox_Subcounty.Text);
                sql.Parameters.AddWithValue("@ward", Combobox_Ward.Text);
                sql.Parameters.AddWithValue("@homelocation", Combobox_Location.Text);
                sql.Parameters.AddWithValue("@profilephoto", id);
                sql.Parameters.AddWithValue("@password", TextBox_NationalID.Text);
                x = sql.ExecuteNonQuery();
                if (x != 1)
                {
                    Tr.Rollback();
                    MessageBox.Show("Failed to save the Student Details!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                sql.Parameters.Clear();
                x = 0;
                RegDbuid = id;
            SkipRegistration: 

                sql = new MySqlCommand("insert into studentsadmission (dbuid, stdregno, admdate, admyear, admintake, academicstatus, programme, firstyearofstudy, currentstudyyear, studyclass, isdeleted,regdbuid) " +
                    "values(@dbuid, @stdregno,@admdate, @admyear, @admintake, @academicstatus, @programme, @firstyearofstudy, @currentstudyyear, @studyclass,@isdeleted,@regdbuid);", con, Tr);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@stdregno", reg); 
                sql.Parameters.AddWithValue("@admdate", Datepicker_AdmissionDate.SelectedDate);
                sql.Parameters.AddWithValue("@admyear", ((AcademicIntake)Combobox_AdmissionIntake.SelectedItem).AdmissionYear);
                sql.Parameters.AddWithValue("@admintake", ((AcademicIntake)Combobox_AdmissionIntake.SelectedItem).IntakeName);
                sql.Parameters.AddWithValue("@academicstatus", "Admitted"); 
                sql.Parameters.AddWithValue("@programme", ((AcademicProgramme)Combobox_Programme.SelectedItem).ProgrammeCode); 
                sql.Parameters.AddWithValue("@firstyearofstudy", Combobox_FirstyearofStudy.SelectedItem.ToString());
                sql.Parameters.AddWithValue("@currentstudyyear", "Admitted");
                sql.Parameters.AddWithValue("@studyclass", Textbox_EnrolledClass.Text);
                sql.Parameters.AddWithValue("@isdeleted", "false");
                sql.Parameters.AddWithValue("@regdbuid", RegDbuid);
                x = sql.ExecuteNonQuery();
                if (x != 1)
                {
                    Tr.Rollback();
                    MessageBox.Show("Failed to save the Student Academic Details!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                sql.Parameters.Clear(); 
                sql = new MySqlCommand("insert into studentacademicprogress (dbuid, stdregno, classname, prevsession, currentsession, reportedsessionid, islast, reportingdate) values " +
                    "(@dbuid, @stdregno, @classname, @prevsession, @currentsession, @reportedsessionid, @islast, @reportingdate)", con, Tr);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@stdregno", reg);
                sql.Parameters.AddWithValue("@classname", Textbox_EnrolledClass.Text);
                sql.Parameters.AddWithValue("@prevsession", "Admission");
                sql.Parameters.AddWithValue("@currentsession", "Admission");
                sql.Parameters.AddWithValue("@reportedsessionid", "Admission");
                sql.Parameters.AddWithValue("@islast", "true");
                sql.Parameters.AddWithValue("@reportingdate", ErpShared.CurrentDate());
                int t = sql.ExecuteNonQuery();
                if (t != 1)
                {
                    Tr.Rollback();
                    MessageBox.Show("Failed to create students academic profile!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                sql.Parameters.Clear();
                sql = new MySqlCommand("insert into studentsfeemaster (dbuid, regno, debit, credit, lastinvoicedate, lastpaymentdate) values(@dbuid, @regno, @debit, @credit, @lastinvoicedate, @lastpaymentdate)", con, Tr);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@regno", reg);
                sql.Parameters.AddWithValue("@debit", 0);
                sql.Parameters.AddWithValue("@credit", 0);
                sql.Parameters.AddWithValue("@lastinvoicedate", ErpShared.CurrentDate());
                sql.Parameters.AddWithValue("@lastpaymentdate", ErpShared.CurrentDate());
                int y = sql.ExecuteNonQuery();
                if (y != 1)
                {
                    Tr.Rollback();
                    MessageBox.Show("Failed to create Student's Account!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                sql.Parameters.Clear();
                sql = new MySqlCommand("insert into studentparents (dbuid, stdregno, fathersname, fathersoccupation, fathersphone, isfatherdeceased, mothername, motheroccupation,  motherphone, ismotherdeaceased, guardian1name, guardian1occupation, guardian1phone, guardian1relationship) values(@dbuid, @stdregno, @fathersname, @fathersoccupation, @fathersphone, @isfatherdeceased,  @mothername, @motheroccupation,  @motherphone, @ismotherdeaceased, @guardian1name, @guardian1occupation, @guardian1phone, @guardian1relationship)", con, Tr);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@stdregno", reg);
                sql.Parameters.AddWithValue("@fathersname", Textbox_Fathersname.Text);
                sql.Parameters.AddWithValue("@fathersoccupation", Textbox_FathersOccupation.Text);
                sql.Parameters.AddWithValue("@fathersphone", Textbox_FathersPhone.Text);
                sql.Parameters.AddWithValue("@isfatherdeceased", BoolString(Checkbox_IsfatherDeceased.IsChecked));
                sql.Parameters.AddWithValue("@mothername", Textbox_Mothersname.Text);
                sql.Parameters.AddWithValue("@motheroccupation", Textbox_MothersOccupation.Text);
                sql.Parameters.AddWithValue("@motherphone", Textbox_MothersPhone.Text);
                sql.Parameters.AddWithValue("@ismotherdeaceased", BoolString(Checkbox_IsmotherDeceased.IsChecked));
                sql.Parameters.AddWithValue("@guardian1name", Textbox_Guardiansname.Text);
                sql.Parameters.AddWithValue("@guardian1relationship", Textbox_GuardiansRelationship.Text);
                sql.Parameters.AddWithValue("@guardian1occupation", Textbox_GuardianOccupation.Text);
                sql.Parameters.AddWithValue("@guardian1phone", Textbox_GuardiansPhone.Text);
                int z = sql.ExecuteNonQuery();
                if (z == 1)
                {
                    Tr.Commit();
                    MessageBox.Show("Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Tr.Rollback();
                    MessageBox.Show("Failed to save the Parental Information!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string BoolString(bool? x)
        {
            try
            {
                if ((bool)x )
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            catch
            {
                return "false";
            }
        }

        private void ButtonRemoveImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StdProfileImage.Source = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Combobox_Programme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Combobox_Programme.SelectedItem!=null)
                {
                    try
                    {
                        Combobox_FirstyearofStudy.Items.Clear();
                        AcademicProgramme AP = Combobox_Programme.SelectedItem as AcademicProgramme;
                        if (AP != null && Combobox_AdmissionIntake.SelectedItem is AcademicIntake AIn)
                        {
                            Textbox_EnrolledClass.Text = AP.ProgrammeCode + "/" + AIn.IntakeMonth.Substring(0, 1) + "/" + AIn.AdmissionYear;
                        }
                        List<TransitionLevel> a = new List<TransitionLevel>();
                        TL.GetAllProgressLevel().Where(q => q.ProgrammeAward == AP.ProgrammeAward && q.ProgrammeStudyMode == AP.ProgrammeStudyMode).ToList().ForEach(f => Combobox_FirstyearofStudy.Items.Add(f.TransitLevel));
                    }
                    catch { }
                }
                else
                {
                    Textbox_EnrolledClass.Text = "";
                    Combobox_FirstyearofStudy.Text = "";
                    Combobox_FirstyearofStudy.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                Textbox_EnrolledClass.Text = "";
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void Combobox_AdmissionIntake_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Combobox_AdmissionIntake.SelectedItem != null)
                {
                    try
                    {
                        if (Combobox_Programme.SelectedItem is AcademicProgramme AP && Combobox_AdmissionIntake.SelectedItem is AcademicIntake AIn)
                        {
                            Textbox_EnrolledClass.Text = AP.ProgrammeCode + "/" + AIn.IntakeMonth.Substring(0, 1) + "/" + AIn.AdmissionYear;
                        }
                    }
                    catch { }
                }
                else
                {
                    Textbox_EnrolledClass.Text = "";
                }
            }
            catch (Exception ex)
            {
                Textbox_EnrolledClass.Text = "";
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
