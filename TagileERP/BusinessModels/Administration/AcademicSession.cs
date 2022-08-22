using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.Administration
{
   public class AcademicSession
    {
        public string Session_dbuid { get; set; }
        public string SessionFullName { get; set; }
        public string SessionShortName { get; set; }
        public string Year { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public DateTime DatePosted { get; set; }
        public string IsCurrentSession { get; set; }
        public string ReportingStatus { get; set; }
        public List<AcademicSession> GetSessionsList(string academicyear)
        {
            List<AcademicSession> a = new List<AcademicSession>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from academicsessions where year=@year;", con);
                sql.Parameters.AddWithValue("@year", academicyear);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        AcademicSession s = new AcademicSession
                        {
                            Session_dbuid = R.GetString("dbuid"),
                            Year = R.GetString("year"),
                            SessionFullName = R.GetString("sessionfullname"),
                            SessionShortName = R.GetString("sessionshortname"),
                            Startdate = R.GetDateTime("fromdate"),
                            Enddate = R.GetDateTime("todate"),
                            DatePosted = R.GetDateTime("dateposted"),
                            IsCurrentSession = R.GetString("iscurrent"),
                            ReportingStatus = R.GetString("reportingstatus")
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
                a.Clear();
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        } 
        public List<AcademicSession> GetSessionsList()
        {
            List<AcademicSession> a = new List<AcademicSession>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from academicsessions;", con); 
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        AcademicSession s = new AcademicSession
                        {
                            Session_dbuid = R.GetString("dbuid"),
                            Year = R.GetString("year"),
                            SessionFullName = R.GetString("sessionfullname"),
                            SessionShortName = R.GetString("sessionshortname"),
                            Startdate = R.GetDateTime("fromdate"),
                            Enddate = R.GetDateTime("todate"),
                            DatePosted = R.GetDateTime("dateposted"),
                            IsCurrentSession = R.GetString("iscurrent"),
                            ReportingStatus = R.GetString("reportingstatus")
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
                a.Clear();
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
        public  AcademicSession GetCurrentSession()
        {
            AcademicSession b;
            try
            {
                
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from academicsessions where iscurrent=@sessionstatus;", con);
                sql.Parameters.AddWithValue("@sessionstatus", "true");
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    AcademicSession a =new AcademicSession();
                    while (R.Read())
                    {

                        a.Session_dbuid = R.GetString("dbuid");
                        a.Year = R.GetString("year");
                        a.SessionFullName = R.GetString("sessionfullname");
                        a.SessionShortName = R.GetString("sessionshortname");
                        a.Startdate = R.GetDateTime("fromdate");
                        a.Enddate = R.GetDateTime("todate");
                        a.DatePosted = R.GetDateTime("dateposted");
                        a.IsCurrentSession = R.GetString("iscurrent");
                        a.ReportingStatus = R.GetString("reportingstatus");
                    }  
                    b= a;
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
