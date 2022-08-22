using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.AcademicsandExamination
{
    public class SubjectMaster
    {
        public string Dbuid { get; set; }
        public bool IsSelected { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string Department { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<SubjectMaster> GetSubjectsList()
        {
            List<SubjectMaster> a = new List<SubjectMaster>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from subjectsmaster;", con); 
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        SubjectMaster p = new SubjectMaster
                        {
                            Dbuid = R["dbuid"].ToString(),
                            IsSelected = false,
                            SubjectCode = R["subjectcode"].ToString(),
                            SubjectName = R["subjectname"].ToString(), 
                            Department = R["department"].ToString(),  
                            RegistrationDate = R.GetDateTime("regdate")
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
