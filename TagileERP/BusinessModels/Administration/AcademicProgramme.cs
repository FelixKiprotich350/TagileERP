using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TagileERP.BusinessModels.AcademicsandExamination;

namespace TagileERP.BusinessModels.Administration
{
   public class AcademicProgramme
    {
        public string Programme_dbuid { get; set; }
        public string ProgrammeCode { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeAward { get; set; }
        public string ProgrammeDepartment { get; set; }
        public string ProgrammeEnrollmentStatus { get; set; }
        public string ProgrammeLifeStatus { get; set; }
        public string ProgrammeStudyMode { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<AcademicProgramme> GetAcademicProgrammesList()
        {
            List<AcademicProgramme> a = new List<AcademicProgramme>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from academicprogrammes;", con);
                sql.Parameters.AddWithValue("@status", "Active");
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        AcademicProgramme p = new AcademicProgramme
                        {
                            Programme_dbuid = R["dbuid"].ToString(),
                            ProgrammeCode = R["programmecode"].ToString(),
                            ProgrammeName = R["programmename"].ToString(),
                            ProgrammeAward = R["award"].ToString(),
                            ProgrammeDepartment = R["department"].ToString(),
                            ProgrammeEnrollmentStatus = R["programmeStatus"].ToString(),
                            ProgrammeStudyMode = R["studymode"].ToString(),
                            RegistrationDate = R.GetDateTime("registrationdate")
                        };
                        a.Add(p);
                    }

                }
                else
                {
                    a.Clear();
                }
                con.Close();

            }
            catch (Exception ex)
            {
                a.Clear();
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
        public List<ProgrammeSubject> GetAcademicProgrammesSubjects(string ProgrammeCode)
        {
            List<ProgrammeSubject> a = new List<ProgrammeSubject>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select a.*,b.subjectname from programmesubjects a,subjectsmaster b where  a.programmecode=@programmecode and a.subjectcode=b.subjectcode;", con);
                sql.Parameters.AddWithValue("@programmecode",ProgrammeCode);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        ProgrammeSubject s = new ProgrammeSubject
                        {
                            SubjectDbuid = R["dbuid"].ToString(),
                            IsSelected = false,
                            SubjectCode = R["subjectcode"].ToString(),
                            SubjectName = R["subjectname"].ToString(),
                            StudyYear = R["yearofstudy"].ToString(),
                            ProgrammeCode = R["programmecode"].ToString(),
                            RegistrationDate = R.GetDateTime("regdate"),
                            IsDeleted = R.GetBoolean("isdeleted")
                        };
                        a.Add(s);
                    }

                }
                else
                {
                    a.Clear();
                }
                con.Close();

            }
            catch (Exception ex)
            {
                a = null;
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
        public AcademicProgramme GetAcademicProgramme(string ProgrammeCode)
        {
            AcademicProgramme a = new AcademicProgramme();
            try
            { 
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from academicprogrammes where programmecode=@programmecode;", con);
                sql.Parameters.AddWithValue("@programmecode",ProgrammeCode);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        AcademicProgramme p = new AcademicProgramme
                        {
                            Programme_dbuid = R["dbuid"].ToString(),
                            ProgrammeCode = R["programmecode"].ToString(),
                            ProgrammeName = R["programmename"].ToString(),
                            ProgrammeAward = R["award"].ToString(),
                            ProgrammeDepartment = R["department"].ToString(),
                            ProgrammeEnrollmentStatus = R["programmeStatus"].ToString(),
                            ProgrammeStudyMode = R["studymode"].ToString(),
                            RegistrationDate = R.GetDateTime("registrationdate")
                        };
                        a = p;
                    }
                    
                }
                else
                {
                    a = null;
                }
                con.Close();

            }
            catch (Exception ex)
            {
                a = null;
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
    }
}
