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
    /// Interaction logic for AddDepartment.xaml
    /// </summary>
    public partial class AddDepartment : Window
    {
        public AddDepartment()
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
                MySqlCommand sql = new MySqlCommand("insert into academicdepartments (dbuid, departmentcode, departmentname, activestatus,isacademicdep,isadministrationdep, regdate, description) values(@dbuid, @departmentcode, @departmentname, @activestatus, @isacademicdep, @isadministrationdep, @regdate, @description);", con);
                sql.Parameters.AddWithValue("@dbuid", id);
                sql.Parameters.AddWithValue("@departmentcode", Textbox_Code.Text);
                sql.Parameters.AddWithValue("@departmentname", Textbox_DepartmentName.Text);
                sql.Parameters.AddWithValue("@activestatus", Combobox_Status.Text);
                sql.Parameters.AddWithValue("@isacademicdep", (bool)CheckBox_IsAcademic.IsChecked ? 1 : 0);
                sql.Parameters.AddWithValue("@isadministrationdep", (bool)CheckBox_IsAdministration.IsChecked ? 1 : 0);
                sql.Parameters.AddWithValue("@regdate", ErpShared.CurrentDate()); 
                sql.Parameters.AddWithValue("@description", Textbox_Description.Text);
                int x = sql.ExecuteNonQuery();
                if (x > 0)
                {
                    MessageBox.Show(this, "Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(this, "Failed to save the Department!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
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
    }
}
