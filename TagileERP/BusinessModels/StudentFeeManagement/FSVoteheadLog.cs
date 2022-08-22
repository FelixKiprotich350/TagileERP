using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.StudentFeeManagement
{
    public class FSVoteheadLog
    {
        public string Log_Dbuid { get; set; }
        public string FeeStructureNo { get; set; }
        public string VoteheadCode { get; set; }
        public string VoteheadName { get; set; }
        public string Session { get; set; }
        public int GOK_Amount { get; set; }
        public int Trainee_Amount { get; set; }
        public int Total { get; set; }
        public List<FSVoteheadLog> GetFS_VoteheadLogs(string FSCode)
        {
            List<FSVoteheadLog> a = new List<FSVoteheadLog>();
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select a.dbuid,a.fscode,a.voteheadcode,a.session,a.gok,a.trainee,a.total,(select b.voteheadname from voteheadsmaster b where a.voteheadcode=b.voteheadcode) as voteheadname from feestructurevhlogs a where a.fscode=@fscode", con);
                sql.Parameters.AddWithValue("@fscode", FSCode);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        a.Add(new FSVoteheadLog()
                        {
                            Log_Dbuid = R["dbuid"].ToString(),
                            FeeStructureNo = R["fscode"].ToString(),
                            VoteheadCode = R["voteheadcode"].ToString(),
                            VoteheadName = R["voteheadname"].ToString(),
                            Session = R["session"].ToString(), 
                            GOK_Amount = R.GetInt32("gok"), 
                            Trainee_Amount = R.GetInt32("trainee"),
                            Total = R.GetInt32("gok")+ R.GetInt32("trainee")
                        });
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
