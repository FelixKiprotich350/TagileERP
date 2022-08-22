using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TagileERP.BusinessModels.Administration;

namespace TagileERP.BusinessModels.Finance
{
    class MasterPayment
    {
        public string MasterDbuid { get; set; }
        public string TransactionID { get; set; }
        public decimal Receivable { get; set; }
        public decimal Payable { get; set; } 
        public DateTime TransactionDate { get; set; } 
        public string LoggedinUser { get; set; } 
        public string LoggedinDevice { get; set; } 
        public List<MasterPayment> GetMasterPayments()
        {
            List<MasterPayment> FeeStatement = new List<MasterPayment>();
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                //,concat(b.firstname,' ',middlename,' ',lastname) as fullname
                MySqlCommand sql = new MySqlCommand("SELECT * FROM masterpayments;", con); 
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {

                    //while (R.Read())
                    //{
                    //    totaldebit = R.GetInt32("debit");
                    //    totalcredit = R.GetInt32("credit");
                    //    FeeStatement.Add(new MasterPayment()
                    //    {
                    //        MasterDbuid = R.GetString("dbuid"),
                    //        TransactionID = R.GetString("regno"),
                    //        Receivable = R.GetString("firstname") + " " + R.GetString("middlename") + " " + R.GetString("lastname"),
                    //        Payable = R.GetString("gender"),
                    //        TransactionDate = R.GetString("programme"),

                    //    }) ;
                    //}
                }
                 
                con.Close();
                return FeeStatement;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                return FeeStatement;
            }
        }
        public MasterPayment GetMasterPayment(string TransID)
        {
            MasterPayment f = null;
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                //,concat(b.firstname,' ',middlename,' ',lastname) as fullname
                MySqlCommand sql = new MySqlCommand("SELECT * FROM masterpayments where transid=@transid;", con);
                sql.Parameters.AddWithValue("@transid",TransID);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {

                    while (R.Read())
                    {

                        f = new MasterPayment()
                        {
                            MasterDbuid = R.GetString("transdbuid"),
                            TransactionID = R.GetString("transid"),
                            Receivable = R.GetDecimal("receivable"),
                            Payable = R.GetDecimal("payable"),
                            TransactionDate = R.GetDateTime("transdate"),
                            LoggedinDevice = R.GetString("loggedindevice"),
                            LoggedinUser = R.GetString("loggedinuser")

                        };
                    }
                }

                con.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                f = null;
            }
            return f;
        }
    }
}
