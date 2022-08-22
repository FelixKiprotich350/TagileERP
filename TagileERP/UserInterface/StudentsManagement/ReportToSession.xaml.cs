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
using TagileERP.BusinessModels.StudentManagement;
using TagileERP.BusinessModels.Administration;
using TagileERP.BusinessModels.StudentFeeManagement;

namespace TagileERP.UserInterface.StudentsManagement
{
    /// <summary>
    /// Interaction logic for ReportSession.xaml
    /// </summary>
    public partial class ReportToSession : Page
    {
        readonly StudentAdmission S = new StudentAdmission();
        readonly StudentAcademicProgress sap = new StudentAcademicProgress();
        readonly AcademicSession asession = new AcademicSession();
        readonly AcademicProgramme ap = new AcademicProgramme();
        readonly TransitionLevel Tl = new TransitionLevel();
        readonly FeeStructure fs = new FeeStructure();
        public ReportToSession()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Textbox_CurrentAcademicYear.Text = ErpShared.CurrentAcademicYear.IntakeName;
                //Textbox_CurrentAcademicYear.Tag = ErpShared.CurrentAcademicYear.AcademicYear_dbuid;
                //Textbox_CurrentSession.Text = ErpShared.CurrentSession.SessionFullName;
                //Textbox_CurrentSession.Tag = ErpShared.CurrentSession.Session_dbuid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ClearForm();
                CheckBox_Refer.IsEnabled = false;
                StudentAdmission sa = S.GetStudentAdmissionDetails(Textbox_SearchRegNo.Text);
                if (sa == null )
                {
                    MessageBox.Show("The student does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                AcademicSession acs = asession.GetCurrentSession();
                if (acs == null )
                {
                    MessageBox.Show("No current session set!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                StudentAcademicProgress a = sap.GetLastProgressDetails(Textbox_SearchRegNo.Text);
                if (a != null)
                {
                    Textbox_StdRegNo.Text = sa.AdmissionNumber;
                    Textbox_FullName.Text = sa.FirstName + " " + sa.MiddleName + " " + sa.LastName;
                    Textbox_Gender.Text = sa.Gender;
                    Textbox_Programme.Text = sa.Programme;
                    Textbox_PreviousClass.Text = a.ClassName;
                    Textbox_ProgressFrom.Text = a.CurrentStudyLevel;
                    Textbox_PreviousSession.Text = a.Prevstudysession;
                    Textbox_ReportingAdmYear.Text = acs.Year;
                    Textbox_ReportingSession.Text = acs.SessionFullName;
                    int status = AlreadyReported();
                    if (status == 0)
                    {
                        Textbox_ReportingStatus.Text = "Not Reported";
                    }
                    else if (status == 1)
                    {
                        Textbox_ReportingStatus.Text = "Reported";
                    }
                    else
                    {
                        Textbox_ReportingStatus.Text = "Unknown";
                    }
                    if (Textbox_ReportingStatus.Text == "Reported")
                    {
                        MessageBox.Show("Reporting Status : " + Textbox_ReportingStatus.Text, "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    TransitionLevel trl = new TransitionLevel();
                    trl = trl.GetProgrammeStudyLevels(sa.Programme).Where(b => b.IsFinalLevel).FirstOrDefault();
                    if (trl != null)
                    {
                        CheckBox_Refer.IsEnabled = true;
                    }
                    else
                    {
                        CheckBox_Refer.IsEnabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("The student's academic details do not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        } 

        private void Button_Report_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AlreadyReported() != 0)
                {
                    MessageBox.Show("You have already reported!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                MySqlTransaction Tr;
                
                //get the student's academic programme
                AcademicProgramme p = ap.GetAcademicProgramme(Textbox_Programme.Text);
                if (p == null)
                {
                    MessageBox.Show("The student's programme does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //get the new study level/progress to
                string StdNewStudyLevel = NewSessionDetails(Textbox_ProgressFrom.Text, p.ProgrammeStudyMode, p.ProgrammeAward);
                if (StdNewStudyLevel == "")
                {
                    MessageBox.Show("Failed to get new Study Level!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // get the current session inplace i.e Term 2
                AcademicSession acs = asession.GetCurrentSession();
                if (acs == null)
                {
                    MessageBox.Show("No current session set!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                //get the new class for studying
                string c = Newclass(StdNewStudyLevel, Textbox_Programme.Text, acs.SessionFullName);
                if (c == null)
                {
                    MessageBox.Show("No corresponding class matches your new study session and year.\n" + "Kindly wait until the next session.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

               //check promotion/progress
                if (p.ProgrammeStudyMode == "Modular")
                {
                    if ((bool)CheckBox_Refer.IsChecked)
                    {
                        StdNewStudyLevel = Textbox_ProgressFrom.Text;
                    }
                }
                else if (p.ProgrammeStudyMode == "Regular")
                {
                    //get average exam and determine if pass

                }
                else
                {
                    MessageBox.Show("Uknown Study Mode for Programme!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //invoice student
                string fsno = FeeStructurePresent();
                if (fsno =="0"|fsno=="-1") { return; }
                int SessionFeeInvoice = fs.GetFS_Trainee_PerSession(fsno, acs.SessionShortName);
                if (SessionFeeInvoice <= 0)
                {
                    MessageBox.Show("Failed to get fee for " + acs.SessionShortName + "!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // Report Student to database
                if (MessageBox.Show("Are you sure you want to report this student?\n" + Textbox_FullName.Text + "\n" + "New Session : " + StdNewStudyLevel, "Message Box", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    string id = Guid.NewGuid().ToString();
                    MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                    con.Open();
                    Tr = con.BeginTransaction();
                    MySqlCommand sql = new MySqlCommand("update studentacademicprogress set islast=@islast where stdregno=@stdregno", con, Tr);
                    sql.Parameters.AddWithValue("@islast", "false");
                    sql.Parameters.AddWithValue("@stdregno", Textbox_StdRegNo.Text);
                    sql.ExecuteNonQuery();
                    sql = new MySqlCommand("insert into studentacademicprogress(dbuid, stdregno, classname, prevsession, currentsession, reportedsessionid, islast, reportingdate) values " +
                           "(@dbuid, @stdregno, @classname, @prevsession, @currentsession, @reportedsessionid, @islast, @reportingdate);", con, Tr);

                    sql.Parameters.AddWithValue("@dbuid", id);
                    sql.Parameters.AddWithValue("@stdregno", Textbox_StdRegNo.Text);
                    sql.Parameters.AddWithValue("@classname", c);
                    sql.Parameters.AddWithValue("@prevsession", Textbox_ProgressFrom.Text);
                    sql.Parameters.AddWithValue("@currentsession", StdNewStudyLevel);
                    sql.Parameters.AddWithValue("@reportedsessionid", asession.GetCurrentSession().SessionFullName);
                    sql.Parameters.AddWithValue("@islast", "true");
                    sql.Parameters.AddWithValue("@reportingdate", ErpShared.CurrentDate());
                    int x = sql.ExecuteNonQuery();
                    if (x != 1)
                    {
                        Tr.Rollback();
                        MessageBox.Show("Failed to report!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        con.Close();
                        return;
                    } 
                    sql = new MySqlCommand("insert into studentfeeaccount (dbuid, studentregno, debit, credit, transactiontype, paymentmode, referenceid, transactiondate, description) " +
                        "values(@dbuid, @studentregno, @debit, @credit, @transactiontype, @paymentmode, @referenceid, @transactiondate, @description);", con, Tr);
                    sql.Parameters.AddWithValue("@dbuid", id);
                    sql.Parameters.AddWithValue("@studentregno", Textbox_StdRegNo.Text);
                    sql.Parameters.AddWithValue("@debit", SessionFeeInvoice);
                    sql.Parameters.AddWithValue("@credit", 0);
                    sql.Parameters.AddWithValue("@transactiontype", ErpShared.AccountsTransactionType.Debit.ToString());
                    sql.Parameters.AddWithValue("@paymentmode", "Invoice");
                    sql.Parameters.AddWithValue("@referenceid", ErpShared.CurrentDate().ToString("yyyyMMddHHfffff"));
                    sql.Parameters.AddWithValue("@transactiondate", ErpShared.CurrentDate());
                    sql.Parameters.AddWithValue("@description", "Fee Invoiced When reporting");
                    x = 0;
                    x = sql.ExecuteNonQuery();
                    if (x != 1)
                    {
                        Tr.Rollback();
                        MessageBox.Show("Failed to Invoice the Student!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        con.Close();
                        return;
                        
                    }
                    sql = new MySqlCommand("update studentsadmission set currentstudyyear=@currentstudyyear, studyclass=@studyclass where stdregno=@stdregno;", con, Tr);
                    sql.Parameters.AddWithValue("@stdregno", Textbox_StdRegNo.Text);
                    sql.Parameters.AddWithValue("@currentstudyyear", StdNewStudyLevel);
                    sql.Parameters.AddWithValue("@studyclass", c);
                    x = 0;
                    x = sql.ExecuteNonQuery();
                    if (x != 1)
                    {
                        Tr.Rollback();
                        MessageBox.Show("Failed to Update Students Academic Details!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        con.Close();
                        return;

                    }

                    Tr.Commit();
                    MessageBox.Show("Success. Reported!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int AlreadyReported()
        {
            int r;
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("SELECT * FROM studentacademicprogress where stdregno=@stdregno and reportedsessionid=@reportedsessionid;", con);
                sql.Parameters.AddWithValue("@stdregno", Textbox_StdRegNo.Text);
                sql.Parameters.AddWithValue("@reportedsessionid", asession.GetCurrentSession().SessionFullName);

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

        private string Newclass(string newstudylevel,string programme,string session)
        {
            string r = null;
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("SELECT * FROM enrollmentclassprogress where programme=@programme and progressname=@progressname and respectivesession=@respectivesession and islast=@islast;", con);
                sql.Parameters.AddWithValue("@respectivesession", session);
                sql.Parameters.AddWithValue("@progressname", newstudylevel);
                sql.Parameters.AddWithValue("@programme", programme);
                sql.Parameters.AddWithValue("@islast", "true");

                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        r = R.GetString("classname");
                    }
                }
                else
                {
                    r = null; 
                }
                con.Close();

            }
            catch (Exception ex)
            {
                r = null;
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }

        private string NewSessionDetails(string curlevel, string studymode, string Award)
        {
            string a;
            try
            {
                if (curlevel == "")
                {
                    a = "";
                }
                if (curlevel=="Admission")
                {
                    StudentAdmission st = S.GetStudentAdmissionDetails(Textbox_StdRegNo.Text);
                    if ( st == null)
                    {
                        a = "";
                        return a; 
                    }
                    TransitionLevel e = Tl.GetAllProgressLevel().Where(w => w.ProgrammeStudyMode == studymode && w.ProgrammeAward == Award && w.TransitLevel == st.FirstStudyYear).First();
                    if (e != null )
                    {
                        a = e.TransitLevel;
                    }
                    else
                    {
                        a = "";
                    }
                }
                else
                {
                    TransitionLevel final = Tl.GetAllProgressLevel().Where(e => e.IsFinalLevel == true).First();
                    TransitionLevel c = Tl.GetAllProgressLevel().Where(w => w.ProgrammeStudyMode == studymode && w.ProgrammeAward == Award&&w.TransitLevel==curlevel).First();
                    if (c != null)
                    {
                        if (c.LevelIndex < final.LevelIndex)
                        {
                            int newlevelindex = c.LevelIndex + 1;
                            TransitionLevel d = Tl.GetAllProgressLevel().Where(w => w.ProgrammeStudyMode == studymode && w.ProgrammeAward == Award && w.LevelIndex == newlevelindex).First();
                            if (d != null)
                            {
                                a = d.TransitLevel;
                            }
                            else
                            {
                                a = "";
                            }
                        }
                        else
                        {
                            a = "";
                        }
                    }
                    else
                    {
                        a = "";
                    }
                }
                 
            }
            catch (Exception ex)
            {
                a = "";
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
        
        //handle fee invoicing
        private string FeeStructurePresent()
        {
            string r="0";
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("SELECT * FROM feestructuremaster where academicyear=@academicyear AND fsstatus=@fsstatus;", con);
                sql.Parameters.AddWithValue("@academicyear", Textbox_ReportingAdmYear.Text);
                sql.Parameters.AddWithValue("@fsstatus", ErpShared.FeeStructureStages.Approved.ToString());
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        r = R.GetString("fscode");
                    }
                }
                else
                {
                    r = "0";
                    MessageBox.Show("Fee Structure for the year not Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                r = "-1";
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }

        private void ClearForm()
        {
            try
            {
                //prev
                Textbox_StdRegNo.Text = "";
                Textbox_FullName.Text = "";
                Textbox_Gender.Text = "";
                Textbox_Programme.Text = "";
                Textbox_ProgressFrom.Text = "";
                Textbox_PreviousSession.Text = "";
                Textbox_PreviousClass.Text = "";
                //new
                Textbox_ReportingStatus.Text = "";
                Textbox_ReportingAdmYear.Text = "";
                Textbox_ReportingSession.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
