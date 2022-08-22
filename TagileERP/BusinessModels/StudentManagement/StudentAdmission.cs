using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.StudentManagement
{
    public class StudentAdmission : StudentRegistration
    {
        public string AdmissionDbuid { get; set; }
        public string AdmissionNumber { get; set; }
        public DateTime AdmDate { get; set; }
        public string AdmYear { get; set; }
        public string AdmIntake { get; set; }
        public string Programme { get; set; }
        public string FirstStudyYear { get; set; }
        public string CurrentYearofStudy { get; set; }
        public string EnrolledClass { get; set; }
        public string AcademicStatus { get; set; }
        public string IsDeleted { get; set; }
        public string IsExist { get; set; }

        public StudentAdmission GetStudentAdmissionDetails(string RegNo)
        {
            if (RegNo is null)
            {
                throw new ArgumentNullException(nameof(RegNo)); 
            }

            StudentAdmission a = null;
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select a.dbuid as regdbuid, a.nationalid, a.firstname, a.middlename, a.lastname, a.gender, a.nationality, a.religion,a.email, a.phone, a.postaddress, a.ispwd, a.birthdate, a.regdate, a.homecounty, a.subcounty, a.ward, a.homelocation, a.profilephoto, a.password, " +
                    "b.dbuid as admdbuid, b.stdregno, b.admdate, b.admyear, b.admintake, b.academicstatus, b.programme, b.firstyearofstudy, b.currentstudyyear, b.studyclass, b.isdeleted,b.regdbuid from studentsreg a right join studentsadmission b on a.dbuid=b.regdbuid where b.stdregno=@stdregno;", con);
                sql.Parameters.AddWithValue("@stdregno", RegNo);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        a = new StudentAdmission()
                        {
                            AdmissionDbuid = R.GetString("admdbuid"),
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


                            //academics
                            AdmissionNumber = R.GetString("stdregno"),
                            AcademicStatus = R.GetString("academicstatus"),
                            AdmDate = R.GetDateTime("admdate"),
                            AdmYear = R.GetString("admyear"),
                            Programme = R.GetString("programme"),
                            AdmIntake = R.GetString("admintake"),
                            EnrolledClass = R.GetString("studyclass"),
                            FirstStudyYear = R.GetString("firstyearofstudy"),
                            CurrentYearofStudy = R.GetString("currentstudyyear"),

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
        public string GetAdmissionStatus(string nationalid)
        {
            string f = "Incomplete";
            try
            {
                string programmecode = "";
                string programmeName = "";
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select b.programme from studentsreg a , studentsadmission b where b.regdbuid=a.dbuid and b.academicstatus!=@academicstatus and a.nationalid=@nationalid;", con);
                sql.Parameters.AddWithValue("@nationalid", nationalid);
                sql.Parameters.AddWithValue("@academicstatus", ErpShared.StudentStatuses.Completed.ToString());
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                { 
                    while (R.Read())
                    {
                        programmecode = R.GetString("programme");
                    }

                }
                else
                {
                    f = "Completed";
                    return f;
                }
                R.Close();
                sql.Parameters.Clear();
                sql = new MySqlCommand("Select * from academicprogrammes where programmecode=@programmecode;", con);
                sql.Parameters.AddWithValue("@programmecode", programmecode);
                R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        programmeName = R.GetString("programmename");
                    }

                }
                MessageBox.Show("The student is enrolled in the following Programme:" + "\n\nProgramme Code : " + programmecode + "\n\nProgramme Name : " + programmeName + "\n\nKindly ensure that the previous Programme is completed in Status!","Message Box",MessageBoxButton.OK,MessageBoxImage.Information);

                con.Close();
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return f;
        }
        public List<StudentRegistration> GetStudentsAdmissionsList()
        {

            List<StudentRegistration> a = new List<StudentRegistration>();
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                //MySqlCommand sql = new MySqlCommand("Select a.nationalid,a.firstname,a.middlename,a.lastname,a.,a,a from studentsreg a right join studentsadmission b on a.dbuid=b.regdbuid;", con);
                MySqlCommand sql = new MySqlCommand("Select a.dbuid as regdbuid, a.nationalid, a.firstname, a.middlename, a.lastname, a.gender, a.nationality, a.religion,a.email, a.phone, a.postaddress, a.ispwd, a.birthdate, a.regdate, a.homecounty, a.subcounty, a.ward, a.homelocation, a.profilephoto, a.password, " +
                    "b.dbuid as admdbuid, b.stdregno, b.admdate, b.admyear, b.admintake, b.academicstatus, b.programme, b.firstyearofstudy, b.currentstudyyear, b.studyclass, b.isdeleted,b.regdbuid from studentsreg a right join studentsadmission b on a.dbuid=b.regdbuid;", con);
                sql.Parameters.AddWithValue("@stdregno", "");
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        StudentAdmission b = new StudentAdmission()
                        {
                            AdmissionDbuid = R.GetString("admdbuid"),
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


                            //academics
                            AdmissionNumber = R.GetString("stdregno"),
                            AcademicStatus = R.GetString("academicstatus"),
                            AdmDate = R.GetDateTime("admdate"),
                            AdmYear = R.GetString("admyear"),
                            Programme = R.GetString("programme"),
                            AdmIntake = R.GetString("admintake"),
                            EnrolledClass = R.GetString("studyclass"),
                            FirstStudyYear = R.GetString("firstyearofstudy"),
                            CurrentYearofStudy = R.GetString("currentstudyyear"),

                        };
                        a.Add(b);
                    }

                }
                else
                {
                    a.Clear();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                a.Clear();
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
        public StudentRegistration GetStudentReportDetails(string RegNo)
        {
            if (RegNo is null)
            {
                throw new ArgumentNullException(nameof(RegNo));
            }

            StudentRegistration a = null;
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from studentsreg where stdregno=@stdregno;", con);
                sql.Parameters.AddWithValue("@stdregno", RegNo);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        //a = new StudentDetails()
                        //{
                        //    Dbuid = R.GetString("dbuid"),
                        //    FirstName = R.GetString("firstname"),
                        //    MiddleName = R.GetString("middlename"),
                        //    LastName = R.GetString("lastname"),
                        //    Gender = R.GetString("gender"),
                        //    RegistrationNumber = R.GetString("stdregno"),
                        //    NationalID = R.GetString("nationalid"),
                        //    Nationality = R.GetString("nationality"),
                        //    DOB = R.GetDateTime("regdate"),
                        //    Email = R.GetString("email"),
                        //    Phone = R.GetString("phone"),
                        //    PostAddress = R.GetString("postaddress"),
                        //    Religion = R.GetString("religion"),
                        //    IsPWD = R.GetString("ispwd"),
                        //    //location 
                        //    County = R.GetString("homecounty"),
                        //    Subcounty = R.GetString("subcounty"),
                        //    Ward = R.GetString("ward"),
                        //    Location = R.GetString("homelocation"),
                        //    Programme = R.GetString("programme"),
                        //    EnrolledClass = R.GetString("studyclass"),
                        //    CurrentYearofStudy = R.GetString("currentstudyyear"),
                        //    Status = R.GetString("status")

                        //};
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
