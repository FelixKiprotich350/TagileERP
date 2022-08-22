using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.Administration
{
   public class AcademicIntake
    {
        
        public string Intake_dbuid { get; set; } 
        public string IntakeName { get; set; }
        public string AdmissionYear { get; set; }
        public string IntakeMonth { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string IntakeStatus { get; set; }
        public string Description { get; set; }
        public List<string> GetAdmYears()
        {
            List<string> a = new List<string>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select distinct(admyear) from enrollmentintakes;", con);
                sql.Parameters.AddWithValue("@status", "Active");
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        string y = R.GetString("admyear");
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
        public List<AcademicIntake> GetAdmissionIntakesList()
        {
            List<AcademicIntake> a = new List<AcademicIntake>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from enrollmentintakes;", con);
                sql.Parameters.AddWithValue("@status", "Open");
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        AcademicIntake y = new AcademicIntake
                        {
                            Intake_dbuid = R.GetString("dbuid"),
                            IntakeName = R.GetString("intakename"),
                            AdmissionYear = R.GetString("admyear"),
                            IntakeMonth = R.GetString("intakemonth"),
                            Startdate = R.GetDateTime("startdate"),
                            Enddate = R.GetDateTime("enddate"),
                            IntakeStatus = R.GetString("status")
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
    }
}
