
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.AcademicsandExamination
{
    public class ExamMaster
    {
        public string Exam_dbuid { get; set; }
        public string ExamNo { get; set; }
        public string ExamName { get; set; }
        public string SessionName { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; } 
        public string MarkEntryStatus { get; set; }  
        public List<ExamMaster> GetExamsList()
        {
            List<ExamMaster> a = new List<ExamMaster>();
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
                        ExamMaster y = new ExamMaster
                        {
                            Exam_dbuid = R.GetString("dbuid"),
                            ExamNo = R.GetString("examno"),
                            ExamName = R.GetString("examname"),
                            SessionName = R.GetString("session"),
                            Startdate = R.GetDateTime("startdate"),
                            Enddate = R.GetDateTime("enddate"),
                            MarkEntryStatus = R.GetString("markentrystatus")
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
            catch (Exception ex)
            {
                a.Clear();
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
        public ExamMaster GetCurrentExam()
        {
            ExamMaster b;
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from examsmaster where markentrystatus=@markentrystatus;", con);
                sql.Parameters.AddWithValue("@markentrystatus", "Open");
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    ExamMaster a = new ExamMaster();
                    while (R.Read())
                    {

                        a.Exam_dbuid = R.GetString("dbuid");
                        a.ExamName = R.GetString("examname");
                        a.ExamNo = R.GetString("examno"); 
                        a.Startdate = R.GetDateTime("startdate");
                        a.Enddate = R.GetDateTime("enddate"); 
                        a.MarkEntryStatus = R.GetString("markentrystatus");
                        a.SessionName = R.GetString("session");
                    }
                    b = a;
                }
                else
                {
                    b = null;
                }
                con.Close();
                return b;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

        }
    }
}
