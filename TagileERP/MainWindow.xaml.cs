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
using TagileERP.UserInterface;
using TagileERP.BusinessModels.Security;
using TagileERP.UserInterface.Security;
using TagileERP.UserInterface.StudentsManagement;
using TagileERP.UserInterface.StudentsFeeManagement; 
using TagileERP.UserInterface.Administration;
using TagileERP.UserInterface.AcademicExamination;
using TagileERP.BusinessModels.Administration;
using TagileERP.BusinessModels.Navigation;
using MaterialDesignColors;
using MaterialDesignThemes;
using TagileERP.UserInterface.UserAccountProfile;
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;

namespace TagileERP
{
    /// <summary>
    /// MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly ERPMasterRoles Master_Roles = new ERPMasterRoles();
        readonly InstitutionModelProperty imp = new InstitutionModelProperty(); 
        public MainWindow()
        {
            InitializeComponent();
            TextBox_Date.Text = ErpShared.CurrentDate().ToLongDateString();
            //Login l = new Login();
            //l.Textbox_Username.Text = "a";
            //l.Password_Box.Password = "123";
            //l.Button_Login_Click(new object(), new RoutedEventArgs());
            ////bool? response = l.ShowDialog();
            ////if (response != null && response == true)
            ////{
            ////    if (ErpShared.CurrentUser != null)
            ////    {
            ////        SetupMenu();
            ////    }
            ////    else
            ////    {
            ////        this.Close();
            ////    }
            ////}
            ////else
            ////{
            ////    this.Close();
            ////}
            ErpShared.CurrentAcademicYear = new AcademicIntake() { IntakeName = "2020", Intake_dbuid = "123" };
            ErpShared.CurrentSession = new AcademicSession() { SessionFullName = "Term 1", Session_dbuid = "123session" };
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from institutioninfo;", con);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        InstitutionModelProperty s = new InstitutionModelProperty
                        {
                            ItemId = Convert.ToInt32(R.GetString("itemid")),
                            ItemName = R.GetString("itemname"),
                            Value1 = R.GetString("value1"),
                            Value2 = R.GetString("value2"),
                            Value3 = R.GetString("value3")
                        };
                        List<InstitutionModelProperty> im = imp.GetInstitutionProperties();
                        MyInstitution myi = new MyInstitution();
                        myi.InstitutionName = im.Where(b => b.ItemId == 1).First().Value1;
                        ErpShared.InstitutionDetails = myi;
                    }

                }
                else
                {
                    ///do something
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                if (MessageBox.Show("Do you want to configure the server now?", "Server Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    InitialServerConfiguration isc = new InitialServerConfiguration();
                   bool? feed= isc.ShowDialog();
                    if ((bool)feed)
                    {
                        Window_Loaded(this,new RoutedEventArgs());
                    }
                }
            }
            SetupUIForUser(false); 
            
            Frame1.Content = new HomePage();
        }

        private void SetupUIForUser(bool UserLoggedIn)
        {
            if (ErpShared.InstitutionDetails!=null)
            {
                this.TextBox_InstitutionTitle.Text = ErpShared.InstitutionDetails.InstitutionName;
            }
            if (UserLoggedIn)
            {
                SetupMenu();
                Label_Login.Visibility = Visibility.Collapsed;
                TextBlock_LoggedinUserDetails.Visibility = Visibility.Visible;
                GridMenu.Visibility = Visibility.Visible;
            }
            else
            {
                Label_Login.Visibility = Visibility.Visible;
                TextBlock_LoggedinUserDetails.Visibility = Visibility.Collapsed;
                GridMenu.Visibility = Visibility.Collapsed;
            }

        }

        private void SetupMenu()
        {
            try
            {
                NavigationMenu menu = new NavigationMenu
                {
                    MenuCategories = Master_Roles.MenuCategoriesList
                };
                //select required roles for the current user and add them to specific categories
                foreach (Level2menu x in ErpShared.CurrentUser.UserRoles)
                {
                    if (menu.MenuCategories.Where(m => m.GroupCode.ToUpper() == x.Category.ToUpper()).Count() > 0)
                    {
                        menu.MenuCategories.Where(m => m.GroupCode.ToUpper() == x.Category.ToUpper()).First().SubMenuItems.Add(x);
                    }
                }
              
                //toggle visibility to visible for used categories
                menu.MenuCategories.RemoveAll(x => x.SubMenuItems.Count == 0);
                //bind datacontext
                 this.DataContext = menu;
                //ModulesListView.ItemsSource = menu.MenuCategories;
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Label_Login_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ErpShared.CurrentUser == null)
            {
                Login l = new Login();
                l.Textbox_Username.Text = "a";
                l.Password_Box.Password = "123";
                // l.Button_Login_Click(new object(), new RoutedEventArgs());
                bool? response = l.ShowDialog();
                if (response != null && response == true)
                {
                    if (ErpShared.CurrentUser == null)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            SetupUIForUser(true);
        }

        private void ActivateItem(object Param)
        {
            if (Param.GetType() != Frame1.Content.GetType())
            {
                Frame1.Content = Param;
            }
            else
            {
                MessageBox.Show("The following is activated");
            }
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
          
        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            UserInterface.Help.HelpMainWindow hm = new UserInterface.Help.HelpMainWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            hm.ShowDialog();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel Submenuobject = sender as StackPanel;
            string tag = Submenuobject.Tag.ToString();
            if (ErpShared.CurrentAcademicYear != null && ErpShared.CurrentSession != null)
            {
                if (tag != "")
                {
                    if (tag.Contains("A"))
                    {
                        switch (tag)
                        {
                            case "A1":
                                //Frame1.Content = new InstitutionSetup();
                                ActivateItem(new InstitutionSetup());
                                break;
                            case "A2":
                                ActivateItem(new DatabaseManager()); 
                                break;
                            case "A3":
                                ActivateItem(new CalendarSetup());
                                break;
                            case "A4":
                                ActivateItem(new AcademicProgrammes());
                                break;
                            case "A5":
                                ActivateItem(new InstitutionDepartments());
                                break;
                            case "A6":
                                ActivateItem(new ManageSystemUsers());
                                break;
                            case "A7":
                                ActivateItem(new ManagePermissions());
                                break;   
                            default:
                                MessageBox.Show(this, "The Function does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }
                    }
                    else if (tag.Contains("B"))
                    {
                        switch (tag)
                        {
                            case "B1":
                                Frame1.Content = new NewStudent();
                                break;
                            case "B2":
                                Frame1.Content = new ReportToSession();
                                break;
                            case "B3":
                                Frame1.Content = new StudentProfile();
                                break;
                            case "B4":
                                Frame1.Content = new AdmissionBook();
                                break;
                            case "B5":
                                Frame1.Content = new StudentsApplicants();
                                break; 
                            case "B7":
                                Frame1.Content = new StudentProgrammeTransfer();
                                break;
                            case "B8":
                                Frame1.Content = new StudentInstitutionalTransfer();
                                break;
                            case "B9":
                                Frame1.Content = new StudentsDeferment();
                                break; 
                            default:
                                MessageBox.Show(this, "The Function does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }
                    }
                    else if (tag.Contains("C"))
                    {
                        switch (tag)
                        {
                            case "C1":
                                Frame1.Content = new ExaminationsList();
                                break;
                            case "C2":
                                Frame1.Content = new MarkEntryStudentsList();
                                break;
                            case "C3":
                                Frame1.Content = new SubjectsManager();
                                break;
                            case "C4":
                                Frame1.Content = new AdmissionBook();
                                break;
                            case "C5": 
                                Frame1.Content = new ManageEnrollmentClasses();
                                break;
                            default:
                                MessageBox.Show(this, "The Function does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }
                    }
                    else if (tag.Contains("D"))
                    {
                        switch (tag)
                        {
                            case "D1":
                                Frame1.Content = new AddFeePayment();
                                break;
                            case "D2":
                                Frame1.Content = new IndividualStudentFeeStatement();
                                break;
                            case "D3":
                                Frame1.Content = new FeeStructureList();
                                break;
                            case "D4":
                                Frame1.Content = new SetupNewFeeStructure();
                                break; 
                            case "D5":
                                Frame1.Content = new FeeVoteheadSetup();
                                break;
                            case "D6":
                                Frame1.Content = new StudentsFeeBalancesList();
                                break;
                            default:
                                MessageBox.Show(this, "The Function does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }
                    }
                    else if (tag.Contains("E"))
                    {
                        switch (tag)
                        {
                            case "E1":
                                Frame1.Content = new ExaminationsList();
                                break;
                            case "E2":
                                Frame1.Content = new NewExamination();
                                break;
                            case "D3":
                                Frame1.Content = new FeeStructureList();
                                break;
                            case "D4":
                                Frame1.Content = new SetupNewFeeStructure();
                                break;
                            case "D5":
                                Frame1.Content = new FeeVoteheadSetup();
                                break;
                            case "D6":
                                Frame1.Content = new StudentsFeeBalancesList();
                                break;
                            default:
                                MessageBox.Show(this, "The Function does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "The Function does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //MCDU_Page_Frame.Content.GetType();
                }
                else
                {
                    Frame1.Content = "";
                    MessageBox.Show(this, "The feature does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show(this, "Active Academic Year and session must be set!." + "\n" + "Ask the administrator to enable!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void StackPanel_MyAccount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                UserProfileDashboard upd = new UserProfileDashboard();
                upd.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StackPanel_Home_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Frame1.Content = new HomePage();
        }
    }
}