using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.StudentManagement
{

    public class StudentRegistration
    {
        //Constructers
        #region
        public StudentRegistration(string RegNo)
        {
            //ErpShared.LastException = null;
            StudentRegistration a=GetRegistrationDetails(RegNo);
            if (a!=null)
            {
                this.RegDbuid = a.RegDbuid; 
                this.NationalID = a.NationalID;
                this.FirstName = a.FirstName;
                this.MiddleName = a.MiddleName;
                this.LastName = a.LastName;
                this.Gender = a.Gender;
                this.Email = a.Email;
                this.Phone = a.Phone;
                this.PostAddress = a.PostAddress;
                this.DOB = a.DOB;
                this.Nationality = a.Nationality;
                this.Religion = a.Religion;
                this.RegDate = a.RegDate;
                this.IsPWD = a.IsPWD;
                this.County = a.County;
                this.Subcounty = a.Subcounty;
                this.Ward = a.Ward;
                this.Location = a.Location;
            }
            //ErpShared.ShowException();
        }

        public StudentRegistration()
        {
           
        }
        #endregion

        //Class Properties
        #region
        public string RegDbuid { get; set; }
        public string NationalID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DOB { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string PostAddress { get; set; } 
        public string RegDate { get; set; }
        public string Status { get; set; } 
        public string IsPWD { get; set; }

        //location
        public string County { get; set; }
        public string Subcounty { get; set; }
        public string Ward { get; set; }
        public string Location { get; set; }

        //Parents
        public string MothersName { get; set; }
        public string Fathersname { get; set; }
        public string MothersOccupation { get; set; }
        public string Fathersoccupation { get; set; }
        public string FathersPhone { get; set; }
        public string MothersPhone { get; set; }
        public string IsmotherDeceased { get; set; }
        public string IsfatherDeceased { get; set; }
        public string GuardianName { get; set; }
        public string GuardianPhone { get; set; }
        public string GuardianOccupation { get; set; }
        public string GuardianRelationship { get; set; }
        #endregion

        //Methods
        public StudentRegistration GetRegistrationDetails(string RegNo)
        {
            //if (RegNo is null)
            //{
            //    throw new ArgumentNullException(nameof(RegNo));
            //}

            StudentRegistration a = null;
            try
            { 
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from studentsreg a, studentparents b where a.stdregno=@stdregno and b.stdregno=a.stdregno;", con);
                sql.Parameters.AddWithValue("@stdregno", RegNo);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        a= new StudentRegistration()
                        {
                            RegDbuid = R.GetString("dbuid"),
                            FirstName = R.GetString("firstname"),
                            MiddleName = R.GetString("middlename"),
                            LastName = R.GetString("lastname"),
                            Gender = R.GetString("gender"), 
                            NationalID = R.GetString("nationalid"),
                            Nationality = R.GetString("nationality"),
                            DOB = R.GetDateTime("regdate"),
                            Email = R.GetString("email"),
                            Phone = R.GetString("phone"),
                            PostAddress = R.GetString("postaddress"),
                            Religion = R.GetString("religion"),
                            IsPWD = R.GetString("ispwd"),
                            //location 
                            County = R.GetString("homecounty"),
                            Subcounty = R.GetString("subcounty"),
                            Ward = R.GetString("ward"),
                            Location = R.GetString("homelocation"),
                            //parents
                            MothersName = R.GetString("mothername"),
                            MothersPhone = R.GetString("motherphone"),
                            MothersOccupation = R.GetString("motheroccupation"),
                            IsmotherDeceased = R.GetString("ismotherdeaceased"),
                            Fathersname = R.GetString("fathersname"),
                            Fathersoccupation = R.GetString("fathersoccupation"),
                            FathersPhone = R.GetString("fathersphone"),
                            IsfatherDeceased = R.GetString("isfatherdeceased"),
                            GuardianName = R.GetString("guardian1name"),
                            GuardianOccupation = R.GetString("guardian1occupation"),
                            GuardianPhone = R.GetString("guardian1phone"),
                            GuardianRelationship = R.GetString("guardian1relationship")

                        };
                    }

                }
                else
                {
                    a = null;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                a = null;
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
        public StudentRegistration GetRegDetailsByNationalId(string RegNo)
        {
            //if (RegNo is null)
            //{
            //    throw new ArgumentNullException(nameof(RegNo));
            //}

            StudentRegistration a = null;
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from studentsreg a where a.nationalid=@nationalid;", con);
                sql.Parameters.AddWithValue("@nationalid", RegNo);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        a = new StudentRegistration()
                        {
                            RegDbuid = R.GetString("dbuid"),
                            FirstName = R.GetString("firstname"),
                            MiddleName = R.GetString("middlename"),
                            LastName = R.GetString("lastname"),
                            Gender = R.GetString("gender"),
                            NationalID = R.GetString("nationalid"),
                            Nationality = R.GetString("nationality"),
                            DOB = R.GetDateTime("regdate"),
                            Email = R.GetString("email"),
                            Phone = R.GetString("phone"),
                            PostAddress = R.GetString("postaddress"),
                            Religion = R.GetString("religion"),
                            IsPWD = R.GetString("ispwd"),
                            //location 
                            County = R.GetString("homecounty"),
                            Subcounty = R.GetString("subcounty"),
                            Ward = R.GetString("ward"),
                            Location = R.GetString("homelocation")

                        };
                    }

                }
                else
                {
                    a = null;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                a = null;
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
    }
}
