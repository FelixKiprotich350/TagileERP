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
using TagileERP.BusinessModels.Administration;
using TagileERP.BusinessModels.StudentFeeManagement;

namespace TagileERP.UserInterface.StudentsFeeManagement
{
    /// <summary>
    /// Interaction logic for SetupNewFeeStructure.xaml
    /// </summary>
    public partial class SetupNewFeeStructure : Page
    {
        readonly ObservableCollection<VoteheadExtended> SessionVoteheadsGrid_items = new ObservableCollection<VoteheadExtended>();
        readonly FeeVoteHead  f = new FeeVoteHead();
        readonly AcademicProgramme ap = new AcademicProgramme();
        readonly AcademicIntake year = new AcademicIntake();
        public SetupNewFeeStructure()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //academic years
                Combobox_AcademicYear.ItemsSource = null;
                Combobox_AcademicYear.Items.Clear();
                Combobox_AcademicYear.ItemsSource = year.GetAdmYears();
                //academic programmes
                Combobox_Programme.ItemsSource = null;
                Combobox_Programme.Items.Clear();
                Combobox_Programme.ItemsSource = ap.GetAcademicProgrammesList();
                //voteheads
                Combobox_Votehead.ItemsSource = null;
                Combobox_Votehead.Items.Clear();
                Combobox_Votehead.ItemsSource = f.GetVoteHeadLists();
               

            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        class VoteheadExtended: FeeVoteHead
        {
            public string Session { get; set; } 
            public int GOKAmount { get; set; } 
            public int TraineeAmount { get; set; } 
            public int TotalAmount { get; set; } 
        }

        private void Button_AddVotehead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Combobox_Votehead.SelectedItem == null)
                {
                    MessageBox.Show("Select a VoteHead Item!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_Session.Text.Trim() == "")
                {
                    MessageBox.Show("Select a Session!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                if (!int.TryParse(Textbox_GOKAmount.Text, out int gok))
                {
                    MessageBox.Show("Invalid GOK Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                if (!int.TryParse(Textbox_TraineeAmount.Text, out int trainee))
                {
                    MessageBox.Show("Invalid Trainee Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (gok + trainee <= 0)
                {
                    MessageBox.Show("The total amount must be greater than Zero!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_Votehead.SelectedItem!=null)
                {
                    if (SessionVoteheadsGrid_items.Where(a=>a.VoteheadCode== ((FeeVoteHead)Combobox_Votehead.SelectedItem).VoteheadCode && a.Session==Combobox_Session.Text).Count()<=0)
                    {
                        VoteheadExtended ve = new VoteheadExtended()
                        {
                            VoteheadCode = ((FeeVoteHead)Combobox_Votehead.SelectedItem).VoteheadCode,
                            VoteheadName = ((FeeVoteHead)Combobox_Votehead.SelectedItem).VoteheadName,
                            Session = Combobox_Session.Text,
                            TraineeAmount = trainee,
                            GOKAmount = gok,
                            TotalAmount = trainee + gok
                        };
                        SessionVoteheadsGrid_items.Add(ve);
                        if (Combobox_Session.Text == "Term 1")
                        {
                            Session1_Datagrid.ItemsSource = SessionVoteheadsGrid_items.Where(b=>b.Session== "Term 1");
                        }
                        if (Combobox_Session.Text == "Term 2")
                        {
                            Session2_Datagrid.ItemsSource = SessionVoteheadsGrid_items.Where(b => b.Session == "Term 2");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You cannot duplicate VoteHead to same session!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_SaveFeeStructure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Combobox_AcademicYear.SelectedItem == null)
                {
                    MessageBox.Show("Select an Academic Year!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                //if (Combobox_Programme.SelectedItem == null)
                //{
                //    MessageBox.Show("Select a Programme!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                //if (Combobox_Yearofstudy.SelectedItem == null)
                //{
                //    MessageBox.Show("Select a Study Year!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}

                if (SessionVoteheadsGrid_items.Count > 0)
                {
                    if (FS_Exist() == 0)
                    {
                        MySqlTransaction Tr;
                        string id = Guid.NewGuid().ToString();
                        int sno = GetNewVoteheadCode();
                        string fscode = "FS-" + sno;
                        MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                        con.Open();
                        Tr = con.BeginTransaction();
                        MySqlCommand sql = new MySqlCommand("insert into feestructuremaster (dbuid, fsno, fscode, academicyear, fsstatus, postdate) values(@dbuid, @fsno, @fscode, @academicyear, @fsstatus, @postdate);", con, Tr);
                        sql.Parameters.AddWithValue("@dbuid", id);
                        sql.Parameters.AddWithValue("@fsno", sno);
                        sql.Parameters.AddWithValue("@fscode", fscode);
                        sql.Parameters.AddWithValue("@academicyear", Combobox_AcademicYear.SelectedItem.ToString());
                        sql.Parameters.AddWithValue("@fsstatus", ErpShared.FeeStructureStages.Preparation.ToString());
                        sql.Parameters.AddWithValue("@postdate", ErpShared.CurrentDate());
                        int x = sql.ExecuteNonQuery();
                        if (x > 0)
                        {
                            int logcount = 0;
                            sql.Parameters.Clear();
                            var votelogs = SessionVoteheadsGrid_items;
                            sql = new MySqlCommand("insert into feestructurevhlogs (dbuid, fscode, session, voteheadcode, gok, trainee, total) values(@dbuid, @fscode, @session, @voteheadcode, @gok, @trainee, @total);", con, Tr);
                            sql.Parameters.Add("@dbuid", MySqlDbType.VarString);
                            sql.Parameters.Add("@fscode", MySqlDbType.VarString);
                            sql.Parameters.Add("@session", MySqlDbType.VarString);
                            sql.Parameters.Add("@voteheadcode", MySqlDbType.VarString);
                            sql.Parameters.Add("@gok", MySqlDbType.Int32);
                            sql.Parameters.Add("@trainee", MySqlDbType.Int32);
                            sql.Parameters.Add("@total", MySqlDbType.Int32);
                            foreach (VoteheadExtended m in votelogs)
                            {
                                id = Guid.NewGuid().ToString();
                                sql.Parameters["@dbuid"].Value = id;
                                sql.Parameters["@fscode"].Value = fscode;
                                sql.Parameters["@session"].Value = m.Session;
                                sql.Parameters["@voteheadcode"].Value = m.VoteheadCode;
                                sql.Parameters["@gok"].Value = m.GOKAmount;
                                sql.Parameters["@trainee"].Value = m.TraineeAmount;
                                sql.Parameters["@total"].Value = m.TotalAmount;
                                logcount += sql.ExecuteNonQuery();
                            }
                            if (logcount == votelogs.Count())
                            {
                                Tr.Commit();
                                MessageBox.Show("Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                Tr.Rollback();
                                MessageBox.Show("Failed to save all voteheads. Fee structure not saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            Tr.Rollback();
                            MessageBox.Show("Failed to save", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        con.Close();
                    }

                }
                else
                {
                    MessageBox.Show("You must add atleast one voteHead!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int FS_Exist()
        {
            int r;
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select fscode from feestructuremaster where academicyear=@academicyear;", con);
                sql.Parameters.AddWithValue("@academicyear",Combobox_AcademicYear.Text); 

                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    r = 1;
                    MessageBox.Show("Fee Structure for the selected AcademicYear already Exists!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    r = 0;
                    
                }
                con.Close();

            }
            catch (Exception ex)
            {
                r = 2;
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }

        private int GetNewVoteheadCode()
        {
            int r = 0;
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select max(fsno) as id,count(fsno) as count from feestructuremaster;", con);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        int count = R.GetInt32("count");
                        if (count > 0)
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
