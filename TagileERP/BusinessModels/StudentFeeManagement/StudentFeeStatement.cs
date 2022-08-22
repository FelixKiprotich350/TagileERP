using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TagileERP.BusinessModels.StudentManagement;

namespace TagileERP.BusinessModels.StudentFeeManagement
{
    public class StudentFeeStatement
    { 
        public string Dbuid { get; set; }
        public string StudentRegNo { get; set; }
        public string TransactionType { get; set; }
        public string PaymentMethod { get; set; }
        public string RefferenceID { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; } 
        public double Balance { get; set; } 
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }  
         public List<StudentFeeStatement> GetStudentFeeStatement(string RegNo)
        {
            List<StudentFeeStatement> FeeStatement = new List<StudentFeeStatement>();
            try
            {

                double totaldebit = 0;
                double totalcredit = 0;
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from studentfeeaccount where studentregno=@studentregno order by transactiondate asc;", con);
                sql.Parameters.AddWithValue("@studentregno", RegNo);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {

                    while (R.Read())
                    {
                        totaldebit += R.GetDouble("debit");
                        totalcredit += R.GetDouble("credit");
                        FeeStatement.Add(new StudentFeeStatement()
                        {
                            Dbuid = R.GetString("dbuid"),
                            StudentRegNo = R.GetString("studentregno"),
                            PaymentMethod = R.GetString("paymentmode"),
                            TransactionType = R.GetString("transactiontype"),
                            RefferenceID = R.GetString("referenceid"),
                            Debit = R.GetDouble("debit"),
                            Credit = R.GetDouble("credit"),
                            Balance = totaldebit - totalcredit,
                            TransactionDate = R.GetDateTime("transactiondate"),
                            Description = R.GetString("description")
                        });

                    }
                   
                }
                else
                {
                    

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
       
    }
}
