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
using TagileERP.BusinessModels.Administration;

namespace TagileERP.UserInterface.AcademicExamination
{
    /// <summary>
    /// Interaction logic for AddEnrollmentClass.xaml
    /// </summary>
    public partial class AddEnrollmentClass : Window
    {
        readonly AcademicIntake Ayear = new AcademicIntake();
        readonly AcademicProgramme Aprogramme = new AcademicProgramme();
        readonly AcademicSession Asession = new AcademicSession();
        readonly TransitionLevel Tl = new TransitionLevel();
        public AddEnrollmentClass()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Combobox_AdmissionIntake.ItemsSource=new ObservableCollection<AcademicIntake>(Ayear.GetAdmissionIntakesList());
                Combobox_Programmes.ItemsSource=new ObservableCollection<AcademicProgramme>(Aprogramme.GetAcademicProgrammesList());
                Combobox_FirstSession.ItemsSource = new ObservableCollection<AcademicSession>(Asession.GetSessionsList());
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
                if (Combobox_Programmes.SelectedItem == null)
                {
                    MessageBox.Show(this, "Select a Programme!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_AdmissionIntake.SelectedItem == null)
                {
                    MessageBox.Show(this, "Select an Intake!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_FirstSession.SelectedItem == null)
                {
                    MessageBox.Show(this, "Select the First Ssession!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                AcademicProgramme p = (AcademicProgramme)Combobox_Programmes.SelectedItem;
                if (Tl.GetAllProgressLevel().Where(t=>t.ProgrammeAward==p.ProgrammeAward&&t.ProgrammeStudyMode==p.ProgrammeStudyMode && t.LevelIndex == 0).Count()<=0)
                {
                    MessageBox.Show(this, "No study Year for this class!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                AcademicIntake a = (AcademicIntake)Combobox_AdmissionIntake.SelectedItem;
                string intakeinitial = a.IntakeName.Substring(0,3);
                string id = Guid.NewGuid().ToString();
                string Genclassname = p.ProgrammeCode + "/" + intakeinitial + "/" + a.AdmissionYear;
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                Tr = con.BeginTransaction();
                MySqlCommand sql = new MySqlCommand("insert into enrollmentclasses (dbuid, classname, admyear, programme, classstatus, classregdate, description) values(@dbuid, @classname, @admyear, @programme, @classstatus, @classregdate, @description);", con);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@classname", Genclassname);
                sql.Parameters.AddWithValue("@admyear", a.AdmissionYear);
                sql.Parameters.AddWithValue("@programme", p.ProgrammeCode); 
                sql.Parameters.AddWithValue("@classstatus", "Closed");
                sql.Parameters.AddWithValue("@description", Textbox_Description.Text);
                sql.Parameters.AddWithValue("@classregdate", ErpShared.CurrentDate()); 
                int x = sql.ExecuteNonQuery();
                if (x > 0)
                {
                    if (AlreadyRegistered(Genclassname, ((AcademicSession)Combobox_FirstSession.SelectedItem).SessionFullName)==0)
                    {
                        sql.Parameters.Clear();
                        sql = new MySqlCommand("insert into enrollmentclassprogress (dbuid, classname, programme, prevstudylevel, progressname, progressdate, respectivesession,islast,parentclassdbuid) values(@dbuid, @classname, @programme, @prevstudylevel, @progressname, @progressdate, @respectivesession, @islast,@parentclassdbuid);", con);
                        sql.Parameters.AddWithValue("@dbuid", id);
                        sql.Parameters.AddWithValue("@classname", Genclassname);
                        sql.Parameters.AddWithValue("@programme", p.ProgrammeCode);
                        sql.Parameters.AddWithValue("@prevstudylevel", "Registration");
                        sql.Parameters.AddWithValue("@progressname", Tl.GetAllProgressLevel().Where(t => t.ProgrammeAward == p.ProgrammeAward && t.ProgrammeStudyMode == p.ProgrammeStudyMode && t.LevelIndex == 0).First().TransitLevel);
                        sql.Parameters.AddWithValue("@respectivesession", ((AcademicSession)Combobox_FirstSession.SelectedItem).SessionFullName);
                        sql.Parameters.AddWithValue("@progressdate", ErpShared.CurrentDate());
                        sql.Parameters.AddWithValue("@islast", "true");
                        sql.Parameters.AddWithValue("@parentclassdbuid",p.Programme_dbuid);
                        int y = sql.ExecuteNonQuery();
                        if (y > 0)
                        {
                            Tr.Commit();
                            MessageBox.Show(this, "Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            Tr.Rollback();
                            MessageBox.Show(this, "Failed to report the class", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        Tr.Rollback();
                        MessageBox.Show(this, "This class has been registered before!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    Tr.Rollback();
                    MessageBox.Show(this, "Failed to save the class", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Combobox_AdmissionIntake_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Combobox_AdmissionIntake.SelectedItem != null)
                {
                    AcademicIntake a = (AcademicIntake)Combobox_AdmissionIntake.SelectedItem;
                    Textbox_AdmissionYear.Text = a.AdmissionYear;

                }
                else
                {
                    Textbox_AdmissionYear.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int AlreadyRegistered(string classname,string Session)
        {
            int r;
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("SELECT * FROM enrollmentclassprogress where classname=@classname and progressname=@progressname;", con);
                sql.Parameters.AddWithValue("@classname", classname); 
                sql.Parameters.AddWithValue("@progressname", Session); 

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
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }

    }
}
