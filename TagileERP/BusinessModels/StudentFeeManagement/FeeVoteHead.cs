using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.StudentFeeManagement
{
    public class FeeVoteHead
    {
        public string Votehead_Dbuid { get; set; }
        public string VoteheadNo { get; set; }
        public string VoteheadCode { get; set; }
        public string VoteheadName { get; set; }
        public string VoteheadDescription { get; set; }
        public DateTime RegistrationDate { get; set; } 
        public List<FeeVoteHead> GetVoteHeadLists()
        {
            List<FeeVoteHead> a = new List<FeeVoteHead>(); 
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from voteheadsmaster;", con);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        a.Add(new FeeVoteHead()
                        {
                            Votehead_Dbuid = R.GetString("dbuid"),
                            VoteheadNo = R.GetString("voteheadno"),
                            VoteheadCode = R.GetString("voteheadcode"),
                            VoteheadName = R.GetString("voteheadname"),
                            VoteheadDescription = R.GetString("voteheaddescription"), 
                            RegistrationDate = R.GetDateTime("dateposted")
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
