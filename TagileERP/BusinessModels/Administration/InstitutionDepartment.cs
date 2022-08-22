using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.Administration
{
    public class InstitutionDepartment
    {
        public string Department_dbuid { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string ActiveStatus { get; set; }
        public bool IsAcademicDepartment { get; set; }
        public bool IsAdministrationDepartment { get; set; }
        public string DepartmentDescription { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<InstitutionDepartment> GetDepartmentsList()
        {
            List<InstitutionDepartment> a = new List<InstitutionDepartment>();
            try
            {
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from academicdepartments;", con); 
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        InstitutionDepartment p = new InstitutionDepartment
                        {
                            Department_dbuid = R["dbuid"].ToString(),
                            DepartmentCode = R["departmentcode"].ToString(),
                            DepartmentName = R["departmentname"].ToString(), 
                            ActiveStatus = R["activestatus"].ToString(),
                            IsAcademicDepartment = R.GetBoolean("isacademicdep"),
                            IsAdministrationDepartment = R.GetBoolean("isadministrationdep"),
                            DepartmentDescription = R["description"].ToString(),
                            RegistrationDate = R.GetDateTime("regdate")
                        };
                        a.Add(p);
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
        public InstitutionDepartment GetDepartmentDetails(string DepCode)
        {
            InstitutionDepartment a = null;
            try
            { 
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from academicdepartments where departmentcode=@departmentcode;", con);
                sql.Parameters.AddWithValue("@departmentcode",DepCode);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        InstitutionDepartment p = new InstitutionDepartment
                        {
                            Department_dbuid = R["dbuid"].ToString(),
                            DepartmentCode = R["departmentcode"].ToString(),
                            DepartmentName = R["departmentname"].ToString(), 
                            ActiveStatus = R["activestatus"].ToString(),
                            IsAcademicDepartment = R.GetBoolean("isacademicdep"),
                            IsAdministrationDepartment = R.GetBoolean("isadministrationdep"),
                            DepartmentDescription = R["description"].ToString(),
                            RegistrationDate = R.GetDateTime("regdate")
                        };
                        a = p;
                    }

                } 
                con.Close();

            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
    }
}