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

namespace TagileERP.UserInterface.Administration
{
    /// <summary>
    /// Interaction logic for AddAcademicYear.xaml
    /// </summary>
    public partial class AddIntake : Window
    {
        public AddIntake()
        {
            InitializeComponent();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string id = Guid.NewGuid().ToString();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                
                MySqlCommand sql = new MySqlCommand("insert into enrollmentintakes (dbuid, intakename, admyear, intakemonth, startdate, enddate, status, regdate, description) values(@dbuid, @intakename, @admyear, @intakemonth, @startdate, @enddate, @status, @regdate, @description);", con);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@intakename", Combobox_AdmissionIntakeMonth.Text+"/"+ Combobox_AdmissionYear.Text); 
                sql.Parameters.AddWithValue("@admyear", Combobox_AdmissionYear.Text); 
                sql.Parameters.AddWithValue("@intakemonth", Combobox_AdmissionIntakeMonth.Text ); 
                sql.Parameters.AddWithValue("@startdate", Startdate.SelectedDate);
                sql.Parameters.AddWithValue("@enddate", Enddate.SelectedDate);
                sql.Parameters.AddWithValue("@regdate", ErpShared.CurrentDate());
                sql.Parameters.AddWithValue("@status", Textbox_YearStatus.Text);
                sql.Parameters.AddWithValue("@description", Textbox_Description.Text);
                int x = sql.ExecuteNonQuery();
                if (x > 0)
                {
                    
                    MessageBox.Show(this, "Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                { 
                    MessageBox.Show(this, "Failed to save! Try again.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Combobox_AdmissionYear.Items.Clear();
                int y = DateTime.Now.Year;
                Combobox_AdmissionYear.Items.Add(y - 1);
                Combobox_AdmissionYear.Items.Add(y);
                Combobox_AdmissionYear.Items.Add(y + 1);
                Combobox_AdmissionYear.Items.Add(y + 2);
                Combobox_AdmissionYear.Items.Add(y + 3);
                Combobox_AdmissionYear.Items.Add(y + 4);
                Combobox_AdmissionYear.Items.Add(y + 5);
                Combobox_AdmissionYear.Items.Add(y + 6);
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
