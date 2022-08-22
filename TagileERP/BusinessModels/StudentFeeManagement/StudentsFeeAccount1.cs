using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TagileERP.BusinessModels.Administration;

namespace TagileERP.BusinessModels.StudentFeeManagement
{
    class StudentsFeeAccount1
    {
        public string RecordDbuid { get; set; }
        public string StudentRegNo { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public string ProgrammeCode { get; set; }
        public string ProgrammeName { get; set; }
        public string StudyClass { get; set; }
        public string YearOfStudy { get; set; }
        public int Debit { get; set; }
        public int Credit { get; set; }
        public int Balance { get; set; } 
        public List<StudentsFeeAccount> GetStudentsBalances()
        {
            List<StudentsFeeAccount> FeeStatement = new List<StudentsFeeAccount>();
            try
            {

                int totaldebit = 0;
                int totalcredit = 0;
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                //,concat(b.firstname,' ',middlename,' ',lastname) as fullname
                MySqlCommand sql = new MySqlCommand("SELECT a.*,c.firstname,c.middlename,c.lastname,c.gender,b.stdregno,b.programme,b.currentstudyyear,b.studyclass FROM studentsfeemaster a , studentsadmission b, studentsreg c where a.regno=b.stdregno and c.dbuid=b.regdbuid;", con); 
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {

                    while (R.Read())
                    {
                        totaldebit = R.GetInt32("debit");
                        totalcredit = R.GetInt32("credit");
                        FeeStatement.Add(new StudentsFeeAccount()
                        {
                            RecordDbuid = R.GetString("dbuid"),
                            StudentRegNo = R.GetString("regno"),
                            StudentName = R.GetString("firstname") + " " + R.GetString("middlename") + " " + R.GetString("lastname"),
                            Gender = R.GetString("gender"),
                            ProgrammeCode = R.GetString("programme"),
                            ProgrammeName = R.GetString("programme"),
                            StudyClass = R.GetString("studyclass"),
                            YearOfStudy = R.GetString("currentstudyyear"),
                            //Debit = totaldebit,
                            //Credit = totalcredit,
                            Balance = totaldebit - totalcredit,

                        }) ;
                    }
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
