using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace TagileERP.UserInterface.Administration
{
    /// <summary>
    /// Interaction logic for CalendarSetup.xaml
    /// </summary>
    public partial class CalendarSetup : Page
    {
        readonly AcademicIntake Ayear = new AcademicIntake();
        readonly AcademicSession Asession = new AcademicSession();
         ObservableCollection<AcademicSession> SessionGrid_items = new ObservableCollection<AcademicSession>();
        ObservableCollection<AcademicIntake> AcademicYearGrid_items = new ObservableCollection<AcademicIntake>();
        public CalendarSetup()
        {
            InitializeComponent();
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Datagrid_SessionsList.ItemsSource = null;
            Datagrid_SessionsList.Items.Clear();
            Datagrid_SessionsList.ItemsSource = SessionGrid_items;
        }
        //sessions
        private void ButtonAddNewSession_Click(object sender, RoutedEventArgs e)
        {
            AddSession add = new AddSession();
            add.ShowDialog();
        }

        private void ButtonViewAllSessions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SessionGrid_items.Clear();
                Datagrid_SessionsList.ItemsSource = null;
                SessionGrid_items = new ObservableCollection<AcademicSession>(Asession.GetSessionsList());
                if (SessionGrid_items.Count <= 0)
                {
                    MessageBox.Show("No Sessions Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }  
                
                Datagrid_SessionsList.ItemsSource = SessionGrid_items;
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
        private void Session_AcademicYearTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = sender as TextBox;
                string filter = t.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_SessionsList.ItemsSource);
                if (filter == "")
                {
                    cv.Filter = null;
                }
                else
                {
                    cv.Filter = o =>
                    {
                        AcademicSession p = o as AcademicSession;
                        return p.Year.ToLower().Contains(filter.ToLower());

                    };
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        //academic year
        private void ButtonAddNewIntake_Click(object sender, RoutedEventArgs e)
        {
            AddIntake add = new AddIntake();
            add.ShowDialog();
        }

        private void Button_ViewAll_AcademicYears_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                AcademicYearGrid_items.Clear();
                AcademicYearGrid_items = new ObservableCollection<AcademicIntake>(Ayear.GetAdmissionIntakesList());
                if (AcademicYearGrid_items.Count<=0)
                {
                    MessageBox.Show("No Enrollment Intakes Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }  
                Datagrid_AcademicyearsList.ItemsSource = null;
                Datagrid_AcademicyearsList.Items.Clear();
                Datagrid_AcademicyearsList.ItemsSource = AcademicYearGrid_items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_AcademicYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = sender as TextBox;
                string filter = t.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_AcademicyearsList.ItemsSource);
                if (filter == "")
                {
                    cv.Filter = null;
                }
                else
                {
                    cv.Filter = o =>
                    {
                        AcademicIntake p = o as AcademicIntake;
                        return p.IntakeName.ToLower().Contains(filter.ToLower()); 
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Active calendar period seetings
        private void CalendarTabcontrol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl) //if this event fired from TabControl then enter
            {
                if (SettingsTabItem.IsSelected)
                {
                    try
                    { 
                        Combobox_SessionAcademicYear.Items.Clear();
                        Ayear.GetAdmYears().ForEach(h => Combobox_SessionAcademicYear.Items.Add(h));
                        Combobox_Sessions.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
 
      
        private void Combobox_SessionAcademicYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Combobox_Sessions.Items.Clear();
                Asession.GetSessionsList(Combobox_SessionAcademicYear.SelectedValue.ToString()).ForEach(h => Combobox_Sessions.Items.Add(h.SessionFullName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SetActiveSession_Click(object sender, RoutedEventArgs e)
        {
            MySqlTransaction Tr;
            try
            {
                if (MessageBox.Show("Are you sure you want to change the Current Session?" + "\nThis change will affect the whole System!", "Message Box", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                    con.Open();
                    Tr = con.BeginTransaction();
                    MySqlCommand sql = new MySqlCommand("update academicsessions set iscurrent='false';", con,Tr);
                    int x = sql.ExecuteNonQuery();
                    if (x >= 0)
                    {
                        sql = new MySqlCommand("update academicsessions set iscurrent='true' where sessionfullname=@sessionfullname and year=@year;", con,Tr);
                        sql.Parameters.AddWithValue("@year", Combobox_SessionAcademicYear.Text);
                        sql.Parameters.AddWithValue("@sessionfullname", Combobox_Sessions.Text);
                        int y = sql.ExecuteNonQuery();
                        if (y == 1)
                        {
                            Tr.Commit();
                            MessageBox.Show("Successfully Updated the Current Session!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Datagrid_SessionsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;
                var item = (AcademicSession)grid.SelectedItem;
                if (item != null)
                {
                    SessionDetails s = new SessionDetails(item);
                    s.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
