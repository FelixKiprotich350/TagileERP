using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TagileERP.BusinessModels.StudentManagement;

namespace TagileERP.BusinessModels.StudentManagement
{
    public class StudentAcademicProgress
    {
        public string Dbuid { get; set; }
        public string RegistrationNumber { get; set; } 
        public string ClassName { get; set; }
        public string Prevstudysession { get; set; }
        public string Prevstudylevel { get; set; }
        public string CurrentStudyLevel { get; set; } 
        public string IsLast { get; set; } 
        public DateTime ReportingDate { get; set; } 
        public StudentAcademicProgress GetLastProgressDetails(string RegNo)
        { 
            StudentAcademicProgress a = null;
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from studentacademicprogress  where stdregno=@stdregno AND islast=@islast;", con);
                sql.Parameters.AddWithValue("@stdregno", RegNo);
                sql.Parameters.AddWithValue("@islast", "true");
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        a = new StudentAcademicProgress()
                        {
                            Dbuid = R.GetString("dbuid"),
                            ClassName = R.GetString("classname"),
                            Prevstudysession = R.GetString("reportedsessionid"),
                            Prevstudylevel = R.GetString("prevsession"),
                            CurrentStudyLevel = R.GetString("currentsession"), 
                            RegistrationNumber = R.GetString("stdregno") 
                        };
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
