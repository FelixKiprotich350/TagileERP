using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.AcademicsandExamination
{
    public class ProgrammeSubject
    {
        public ProgrammeSubject()
        {
            IsSelected = false;
        }
        public string SubjectDbuid { get; set; }
        public bool IsSelected { get; set; }
        public string ProgrammeCode { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string StudyYear { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<ProgrammeSubject> GetSubjectsList(string Program)
        {
            List<ProgrammeSubject> a = new List<ProgrammeSubject>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select a.dbuid,a.programmecode,a.yearofstudy,a.subjectcode,b.subjectname,a.regdate from programmesubjects a, subjectsmaster b where a.programmecode=@programmecode and a.subjectcode=b.subjectcode and a.isdeleted=@isdeleted;", con);
                sql.Parameters.AddWithValue("@programmecode", Program);
                sql.Parameters.AddWithValue("@isdeleted", "false");
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        ProgrammeSubject p = new ProgrammeSubject
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
    }
}
