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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagileERP.BusinessModels.Administration;
using TagileERP.BusinessModels.StudentManagement;

namespace TagileERP.UserInterface.StudentsManagement
{
    /// <summary>
    /// Interaction logic for StudentProfile.xaml
    /// </summary>
    public partial class StudentProfile : Page
    {
        readonly StudentAdmission S = new StudentAdmission();
        readonly AcademicProgramme AP = new AcademicProgramme();
        public StudentProfile()
        {
            InitializeComponent();
        }

        private void Button_SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StudentAdmission a = S.GetStudentAdmissionDetails(Textbox_SearchRegNo.Text);
                
                if (a != null)
                {
                    
                    Groupbox_PersonalDetails_headerText.Content =  a.AdmissionNumber;
                    Textbox_Firstname.Text = a.FirstName;
                    Textbox_Lastname.Text = a.FirstName;
                    Textbox_MiddleName.Text = a.FirstName;
                    TextBox_NationalID.Text = a.NationalID.ToString();
                    Textbox_TelephoneNo.Text = a.Phone;
                    Textbox_EmailAddress.Text = a.Email;
                    Textbox_Gender.Text = a.Gender;
                    Textbox_DOB.Text = a.DOB.ToShortDateString();
                    Textbox_Nationality.Text = a.Nationality;
                    Textbox_Religion.Text = a.Religion;
                    TextBox_PostalAddress.Text = a.PostAddress;
                    Textbox_County.Text = a.County;
                    Textbox_Subcounty.Text = a.Subcounty;
                    Textbox_Ward.Text = a.Ward;
                    Textbox_Location.Text = a.Location;

                    //parental info
                    Textbox_Mothersname.Text = a.MothersName;
                    Textbox_MothersOccupation.Text = a.MothersOccupation;
                    Textbox_MothersPhone.Text = a.MothersPhone;
                    Textbox_IsmotherDeceased.Text = a.IsmotherDeceased;
                    Textbox_Fathersname.Text = a.Fathersname;
                    Textbox_FathersOccupation.Text = a.Fathersoccupation;
                    Textbox_FathersPhone.Text = a.FathersPhone;
                    Textbox_IsfatherDeceased.Text = a.IsfatherDeceased;
                    Textbox_GuardianOccupation.Text = a.GuardianOccupation;
                    Textbox_Guardiansname.Text = a.GuardianName;
                    Textbox_GuardiansPhone.Text = a.GuardianPhone;
                    Textbox_GuardiansRelationship.Text = a.GuardianRelationship;

                    //academic
                    AcademicProgramme p = AP.GetAcademicProgramme(a.Programme);
                    if (p!=null)
                    {
                        Textbox_ProgrammeCode.Text = p.ProgrammeCode;
                        TextBox_ProgrammeName.Text = p.ProgrammeName;
                        TextBox_AdmissionIntake.Text = a.AdmIntake;
                        Textbox_ProgrammeAward.Text = p.ProgrammeAward;
                        TextBox_AdmissionDate.Text = a.AdmDate.ToShortDateString();
                        
                        StudentAcademicProgress sap = new StudentAcademicProgress();
                        StudentAcademicProgress _sap = sap.GetLastProgressDetails(a.AdmissionNumber);
                        if (_sap != null)
                        {
                            TextBox_yearofStudy.Text = _sap.CurrentStudyLevel;
                            Textbox_EnrolledClass.Text = _sap.ClassName;
                        }
                        else
                        {
                            MessageBox.Show("The Students last progress details does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("The Selected Programme info does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    
                }
                else
                {
                    MessageBox.Show("The Students Registration Number does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
