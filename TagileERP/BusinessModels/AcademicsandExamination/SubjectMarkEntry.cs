
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.AcademicsandExamination
{
    public class SubjectMarkEntry
    {
        public string Mark_dbuid { get; set; }
        public string StdRegNo { get; set; }
        public string ExamNo { get; set; }
        public string SubjectCode { get; set; }
        public string MarkType { get; set; }
        public int MarkScore { get; set; }
        public DateTime FirstEntryDate { get; set; }
        public DateTime LastUpdatedate { get; set; }  
        public List<SubjectMarkEntry> GetMarksList()
        {
            List<SubjectMarkEntry> a = new List<SubjectMarkEntry>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from examsmaster;", con); 
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        SubjectMarkEntry y = new SubjectMarkEntry
                        {
                            Mark_dbuid = R.GetString("dbuid"),
                            ExamNo = R.GetString("examno"),
                            StdRegNo = R.GetString("examname"),
                            SubjectCode = R.GetString("session"),
                            FirstEntryDate = R.GetDateTime("startdate"),
                            LastUpdatedate = R.GetDateTime("enddate"),
                            MarkType = R.GetString("markentrystatus")
                        };
                        a.Add(y);
                    }

                }
                else
                {
                    a.Clear();
                }
                con.Close();

            }
            catch 
            {
                a.Clear();
            }
            return a;
        }
    }
}
