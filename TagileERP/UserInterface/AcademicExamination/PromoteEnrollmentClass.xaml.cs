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
using System.Windows.Shapes;
using TagileERP.BusinessModels.Administration;

namespace TagileERP.UserInterface.AcademicExamination
{
    /// <summary>
    /// Interaction logic for PromoteEnrollmentClass.xaml
    /// </summary>
    public partial class PromoteEnrollmentClass : Window
    {
        readonly PromotionClass p = new PromotionClass();
        ObservableCollection<PromotionClass> ClassesGrid_items = new ObservableCollection<PromotionClass>();
        readonly AcademicSession ass = new AcademicSession();
        public PromoteEnrollmentClass()
        {
            InitializeComponent();
        }

        private void ButtonViewAllClasses_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassesGrid_items = new ObservableCollection<PromotionClass>(p.GetClassPromotionList());
                Datagrid_ClassesList.ItemsSource = null;
                Datagrid_ClassesList.Items.Clear();
                Datagrid_ClassesList.ItemsSource = ClassesGrid_items;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonPromoteClasses_Click(object sender, RoutedEventArgs e)
        {
            MySqlTransaction Tr;
            try
            {
                AcademicSession cc = ass.GetCurrentSession();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                Tr = con.BeginTransaction();
                List<PromotionClass> a = Datagrid_ClassesList.Items.Cast<PromotionClass>().Where(s => s.IsSelected == true).ToList();
                foreach (PromotionClass b in a)
                {
                    if (b.ClassPromotionStatus=="Not Promoted")
                    {
                        string c = p.NewSessionDetails(b.CurrentStudyLevel, "Diploma", "Modular");
                        if (c!="")
                        {
                            if (AlreadyPromoted(b.ClassName, c) == 0)
                            {
                                string id = Guid.NewGuid().ToString();
                                MySqlCommand sql = new MySqlCommand("update enrollmentclassprogress set islast=@islast where classname=@classname and programme=@programme;", con);
                                sql.Parameters.AddWithValue("@programme", b.Programme);
                                sql.Parameters.AddWithValue("@classname", b.ClassName);
                                sql.Parameters.AddWithValue("@islast", "false");
                                sql.ExecuteNonQuery();
                                sql.Parameters.Clear();
                                sql = new MySqlCommand("insert into enrollmentclassprogress (dbuid, programme, classname, prevstudylevel, progressname, respectivesession, progressdate, islast, parentclassdbuid) values " +
                                    "(@dbuid, @programme, @classname, @prevstudylevel, @progressname, @respectivesession, @progressdate, @islast, @parentclassdbuid);", con);
                                sql.Parameters.AddWithValue("@dbuid", id);
                                sql.Parameters.AddWithValue("@programme", b.Programme);
                                sql.Parameters.AddWithValue("@classname", b.ClassName); 
                                sql.Parameters.AddWithValue("@prevstudylevel", b.CurrentStudyLevel);
                                sql.Parameters.AddWithValue("@progressname", c);
                                sql.Parameters.AddWithValue("@respectivesession", cc.SessionFullName);
                                sql.Parameters.AddWithValue("@progressdate", ErpShared.CurrentDate());
                                sql.Parameters.AddWithValue("@islast", "true");
                                sql.Parameters.AddWithValue("@parentclassdbuid", b.Class_Dbuid);
                                int y = sql.ExecuteNonQuery();
                                if (y > 0)
                                {
                                    MessageBox.Show(this, "Successfully Promoted.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show(this, "Failed to promote the class!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                            else
                            {
                               // MessageBox.Show(this, "This class has been Promoted!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Failed to get the Class Progress!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } 

                }
                Tr.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int AlreadyPromoted(string classname, string newyear)
        {
            int r;
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("SELECT * FROM enrollmentclassprogress where classname=@classname and progressname=@progressname;", con);
                sql.Parameters.AddWithValue("@classname", classname);
                sql.Parameters.AddWithValue("@progressname", newyear);

                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    r = 1;
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
         
    }
}
