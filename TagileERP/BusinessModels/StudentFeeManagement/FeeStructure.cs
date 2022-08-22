using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.StudentFeeManagement
{
    public class FeeStructure
    { 
        public string FS_Dbuid { get; set; }
        public string FSNo { get; set; }
        public string FSCode { get; set; }
        public string FSAcademicYear { get; set; } 
        public DateTime RegistrationDate { get; set; } 
        public string Status { get; set; } 
        public int GOk { get; set; } 
        public int Trainee { get; set; }  
        public List<FeeStructure> GetFeeStructures()
        {
            List<FeeStructure> a = new List<FeeStructure>();
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select a.dbuid, a.fsno, a.fscode, a.academicyear, a.fsstatus, a.postdate, (SELECT sum(gok)  FROM feestructurevhlogs WHERE fscode=a.fscode) as gok, (SELECT sum(trainee) FROM feestructurevhlogs WHERE fscode=a.fscode) as trainee from feestructuremaster a", con);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        try
                        {
                            a.Add(new FeeStructure()
                            {
                                FS_Dbuid = R.GetString("dbuid"),
                                FSNo = R.GetString("fsno"),
                                FSCode = R.GetString("fscode"),
                                FSAcademicYear = R.GetString("academicyear"), 
                                Status = R.GetString("fsstatus"),
                                GOk = R.GetInt32("gok"),
                                Trainee = R.GetInt32("trainee"),
                                RegistrationDate = R.GetDateTime("postdate")
                            });
                        }
                        catch { }
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
       
        public int GetFS_SumGok(string FSCode)
        {
            int a=0;
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select sum(gok) as gok from feestructurevhlogs a where fscode=@fscode;", con);
                sql.Parameters.AddWithValue("@fscode", FSCode);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        a += R.GetInt32("gok");
                    }

                }
                else
                {
                    a = 0;
                }
                con.Close();
            }
            catch  
            {
                a = -1; 
            }
            return a;
        }

        public int GetFS_SumTrainee(string FSCode)
        {
            int a = 0;
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select sum(trainee) as trainee from feestructurevhlogs a where fscode=@fscode;", con);
                sql.Parameters.AddWithValue("@fscode", FSCode);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        a += R.GetInt32("trainee");
                    }

                }
                else
                {
                    a = 0;
                }
                con.Close();
            }
            catch  
            {
                a = -1; 
            }
            return a;
        }

        public int GetFS_Trainee_PerSession(string FSCode,string Session)
        {
            int a = 0;
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select sum(trainee) as trainee from feestructurevhlogs a where fscode=@fscode and session=@session;", con);
                sql.Parameters.AddWithValue("@fscode", FSCode);
                sql.Parameters.AddWithValue("@session", Session);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        a += R.GetInt32("trainee");
                    }

                }
                else
                {
                    a = 0;
                }
                con.Close();
            }
            catch
            {
                a = -1;
            }
            return a;
        }

    }
}
