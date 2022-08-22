using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagileERP.BusinessModels.StudentFeeManagement;

namespace TagileERP.UserInterface.StudentsFeeManagement
{
    /// <summary>
    /// Interaction logic for FeeVoteheadSetup.xaml
    /// </summary>
    public partial class FeeVoteheadSetup : Page
    {
        readonly ObservableCollection<FeeVoteHead> VoteheadsGrid_items = new ObservableCollection<FeeVoteHead>();
        public FeeVoteheadSetup()
        {
            InitializeComponent();
        }

        private void ButtonAddNewVotehead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Voteheadname.Text.Trim() != "")
                {
                    string id = Guid.NewGuid().ToString();
                    int sno = GetNewVoteheadCode();
                    MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                    con.Open();
                    MySqlCommand sql = new MySqlCommand("insert into voteheadsmaster (dbuid, voteheadno,voteheadcode, voteheadname, voteheaddescription, dateposted) values(@dbuid, @voteheadno,@voteheadcode, @voteheadname, @voteheaddescription, @dateposted);", con);
                    sql.Parameters.AddWithValue("@dbuid", id);
                    sql.Parameters.AddWithValue("@voteheadno", sno);
                    sql.Parameters.AddWithValue("@voteheadcode", "V" + sno);
                    sql.Parameters.AddWithValue("@voteheadname", Textbox_Voteheadname.Text);
                    sql.Parameters.AddWithValue("@voteheaddescription", Textbox_VoteheadDescription.Text);
                    sql.Parameters.AddWithValue("@dateposted", ErpShared.CurrentDate());
                    int x = sql.ExecuteNonQuery();
                    if (x > 0)
                    {
                        MessageBox.Show("Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        ButtonRefreshVoteheads_Click(new object(), new RoutedEventArgs());
                    }
                    else
                    {
                        MessageBox.Show("Failed to save", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Enter the VoteHead Name!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonRefreshVoteheads_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VoteheadsGrid_items.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from voteheadsmaster;", con);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        VoteheadsGrid_items.Add(new FeeVoteHead()
                        {
                            Votehead_Dbuid = R.GetString("dbuid"),
                            VoteheadNo = R.GetString("voteheadno"),
                            VoteheadCode = R.GetString("voteheadcode"),
                            VoteheadName = R.GetString("voteheadname"),
                            VoteheadDescription = R.GetString("voteheaddescription"), 
                            RegistrationDate = R.GetDateTime("dateposted") 
                        }
                        );
                    }

                }
                else
                {
                    MessageBox.Show("No VoteHeads Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                con.Close();
                Datagrid_VoteheadsList.ItemsSource = null;
                Datagrid_VoteheadsList.Items.Clear();
                Datagrid_VoteheadsList.ItemsSource = VoteheadsGrid_items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetNewVoteheadCode()
        {
            int r = 0;
            try
            {
              
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select max(voteheadno) as id,count(voteheadno) as count from voteheadsmaster;", con);
               
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        int count = R.GetInt32("count");
                        if (count>0)
                        {
                            r = R.GetInt32("id");
                            r += 1;
                        }
                        else
                        {
                            r = 1;
                        }
                    }

                } 
                con.Close();
               
            }
            catch (Exception ex)
            {
                r = 0;
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }
    }
}
