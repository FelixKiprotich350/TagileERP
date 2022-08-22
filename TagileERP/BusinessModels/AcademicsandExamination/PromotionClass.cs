using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TagileERP.BusinessModels.StudentManagement;

namespace TagileERP.BusinessModels.Administration
{
    public class PromotionClass
    { 
        readonly TransitionLevel Tl = new TransitionLevel();
        public string Class_Dbuid { get; set; }
        public bool IsSelected { get; set; }
        public string ClassName { get; set; }//dict/jan/2020
        public string ClassAdmYear { get; set; }
        public string Programme { get; set; }
        public string PreviouStudyLevel { get; set; }
        public string CurrentStudyLevel { get; set; }
        public string ClassPromotionStatus { get; set; }
        public string PromotionDate { get; set; }

        public string NewSessionDetails(string Prevlevel,string ProgrammeAward,string StudyMode)
        { 
            string a;
            try
            {
                if (Prevlevel == "")
                {
                    a = "";
                }
                else
                {
                    TransitionLevel final = Tl.GetAllProgressLevel().Where(e => e.IsFinalLevel == true).First();
                    TransitionLevel c = Tl.GetAllProgressLevel().Where(w => w.ProgrammeStudyMode == StudyMode && w.ProgrammeAward == ProgrammeAward && w.TransitLevel == Prevlevel).First();
                    if (c != null)
                    {
                        if (c.LevelIndex < final.LevelIndex)
                        {
                            int newlevelindex = c.LevelIndex + 1;
                            TransitionLevel d = Tl.GetAllProgressLevel().Where(w => w.ProgrammeStudyMode == StudyMode && w.ProgrammeAward == ProgrammeAward && w.LevelIndex == newlevelindex).First();
                            if (d != null)
                            {
                                a = d.TransitLevel;
                            }
                            else
                            {
                                a = "";
                            }
                        }
                        else
                        {
                            a = "";
                        }
                    }
                    else
                    {
                        a = "";
                    }
                }

            }
            catch (Exception ex)
            {
                a = "";
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }

        public List<PromotionClass> GetClassPromotionList()
        {
            List<PromotionClass> a = new List<PromotionClass>();
            AcademicSession ass = new AcademicSession();
            try
            {
                string CurrSession = ass.GetCurrentSession().SessionFullName;
                a.Clear();
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("SELECT a.dbuid,a.classname,a.programme,a.admyear,b.prevstudylevel,b.progressname,b.respectivesession,b.progressdate FROM enrollmentclasses a left join enrollmentclassprogress b on a.classname=b.classname and  b.islast='true';", con);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        PromotionClass y = new PromotionClass
                        {
                            Class_Dbuid = R.GetString("dbuid"),
                            ClassName = R.GetString("classname"),
                            ClassAdmYear = R.GetString("admyear"),
                            Programme = R.GetString("programme"),
                            IsSelected = false,

                        };
                        if (!R.IsDBNull(5))
                        {
                            y.PreviouStudyLevel = R.GetString("prevstudylevel");
                            y.CurrentStudyLevel = R.GetString("progressname");
                            y.PromotionDate = R.GetDateTime("progressdate").ToShortDateString();
                            y.ClassPromotionStatus = CurrSession == R.GetString("respectivesession") ? "Promoted" : "Not Promoted";
                        }
                        else
                        {
                            y.PreviouStudyLevel = "";
                            y.CurrentStudyLevel = "";
                            y.PromotionDate = ""; 
                            y.ClassPromotionStatus = "Not Promoted";
                        }
                        a.Add(y);
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
    }
}
