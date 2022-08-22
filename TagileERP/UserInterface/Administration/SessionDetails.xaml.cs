using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TagileERP.BusinessModels.Administration;

namespace TagileERP.UserInterface.Administration
{
    /// <summary>
    /// Interaction logic for SessionDetails.xaml
    /// </summary>
    public partial class SessionDetails : Window
    {
        readonly AcademicSession session = new AcademicSession();
        public SessionDetails(AcademicSession s)
        {
            InitializeComponent();
            session = s;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_SessionFullname.Text = session.SessionFullName;
                Textbox_ShortName.Text = session.SessionShortName;
                Textbox_Year.Text = session.Year;
                Textbox_Startdate.Text = session.Startdate.ToShortDateString();
                Textbox_Enddate.Text = session.Enddate.ToShortDateString();
                Textbox_Iscurrentsession.Text = session.IsCurrentSession;
                Textbox_ReportingStatus.Text = session.ReportingStatus;
                if (session.IsCurrentSession == "true")
                {
                    Groupbox_ReportingStatus.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
        private void Button_SetCurrentSession_Click(object sender, RoutedEventArgs e)
        {
            MySqlTransaction Tr;
            try
            {
                if (MessageBox.Show("Are you sure you want to change the Current Session?" + "\nThis change will affect the whole System!", "Message Box", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                    con.Open();
                    Tr = con.BeginTransaction();
                    MySqlCommand sql = new MySqlCommand("update academicsessions set iscurrent='false';", con, Tr);
                    int x = sql.ExecuteNonQuery();
                    if (x >= 0)
                    {
                        sql = new MySqlCommand("update academicsessions set iscurrent=@iscurrent where sessionfullname=@sessionfullname and year=@year;", con, Tr);
                        sql.Parameters.AddWithValue("@iscurrent", Combobox_IscurrentStatus.Text);
                        sql.Parameters.AddWithValue("@year", Textbox_Year.Text);
                        sql.Parameters.AddWithValue("@sessionfullname", Textbox_SessionFullname.Text);
                        int y = sql.ExecuteNonQuery();
                        if (y == 1)
                        {
                            Tr.Commit();
                            MessageBox.Show("Successfully Updated the Current Session!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                        else
                        {
                            Tr.Rollback();
                            MessageBox.Show("Failed to Update the Current Session!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        Tr.Rollback();
                        MessageBox.Show("Failed to deactivate the Previous Session!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ChangeReportingStatus_Click(object sender, RoutedEventArgs e)
        {
            MySqlTransaction Tr;
            try
            {
                if (MessageBox.Show("Are you sure you want to change the status of the Current Session?" + "\nThis change will affect the whole System!", "Message Box", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                    con.Open();
                    Tr = con.BeginTransaction();
                    MySqlCommand sql = new MySqlCommand("update academicsessions set reportingstatus='Closed';", con, Tr);
                    int x = sql.ExecuteNonQuery();
                    if (x >= 0)
                    {
                        sql = new MySqlCommand("update academicsessions set reportingstatus=@reportingstatus where sessionfullname=@sessionfullname and year=@year;", con, Tr);
                        sql.Parameters.AddWithValue("@reportingstatus", Combobox_ReportingStatus.Text);
                        sql.Parameters.AddWithValue("@year", Textbox_Year.Text);
                        sql.Parameters.AddWithValue("@sessionfullname", Textbox_SessionFullname.Text);
                        int y = sql.ExecuteNonQuery();
                        if (y == 1)
                        {
                            Tr.Commit();
                            MessageBox.Show("Successfully Updated the Status of the Current Session!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                        else
                        {
                            Tr.Rollback();
                            MessageBox.Show("Failed to Update the Current Session!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        Tr.Rollback();
                        MessageBox.Show("Failed to deactivate the Previous Session!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
