using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.AcademicExamination
{
    public class EnrollmentClassProgress
    {
        public string Dbuid { get; set; }
        public string ClassName { get; set; }//dict/jan/2020
        public string ClassAcademicYearIntake { get; set; }
        public string Programme { get; set; }
        public string ClassStatus { get; set; } 
        public DateTime PromotionDate { get; set; } 
        public string ProgressToLevel { get; set; }

        public List<EnrollmentClass> GetEnrollmentClassProgressList()
        {
            List<EnrollmentClass> a = new List<EnrollmentClass>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from enrollmentclasses;", con); 
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        EnrollmentClass y = new EnrollmentClass
                        {
                            Class_Dbuid = R.GetString("dbuid"),
                            ClassName = R.GetString("classname"),
                            ClassAcademicYearIntake = R.GetString("admyear"), 
                            Programme = R.GetString("programme"),
                            ClassStatus = R.GetString("classstatus"),
                            Description = R.GetString("description"),
                            RegistrationDate = R.GetDateTime("classregdate")
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
