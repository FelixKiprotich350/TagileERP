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
using MySql.Data.MySqlClient;
using TagileERP.BusinessModels.Administration;

namespace TagileERP.UserInterface.Administration
{
    /// <summary>
    /// Interaction logic for AddSession.xaml
    /// </summary>
    public partial class AddSession : Window
    {
        readonly AcademicIntake Ayear = new AcademicIntake();
        public AddSession()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Ayear.GetAdmYears().ForEach(h => ComboboxAdmissionyear.Items.Add(h));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("insert into academicsessions (dbuid, year, sessionshortname, sessionfullname, fromdate, todate, dateposted, sessionstatus, description) values(@dbuid, @year, @sessionshortname, @sessionfullname, @fromdate, @todate, @dateposted, @sessionstatus, @description);", con);
                sql.Parameters.AddWithValue("@dbuid",id);
                sql.Parameters.AddWithValue("@year", ComboboxAdmissionyear.Text);
                sql.Parameters.AddWithValue("@sessionshortname", Combobox_SessionName.Text);
                sql.Parameters.AddWithValue("@sessionfullname", Combobox_SessionName.Text+"-"+ ComboboxAdmissionyear.Text);
                sql.Parameters.AddWithValue("@fromdate",FromDate.SelectedDate);
                sql.Parameters.AddWithValue("@todate",ToDate.SelectedDate);
                sql.Parameters.AddWithValue("@dateposted",ErpShared.CurrentDate());
                sql.Parameters.AddWithValue("@sessionstatus", Textbox_SessionStatus.Text);
                sql.Parameters.AddWithValue("@description",Description.Text);
                int x = sql.ExecuteNonQuery();
                if (x > 0)
                {
                    MessageBox.Show(this, "Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(this, "Failed to save", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
