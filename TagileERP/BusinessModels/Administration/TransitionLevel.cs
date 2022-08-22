using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TagileERP.BusinessModels.Administration
{
    public class TransitionLevel
    {
        public int LevelIndex { get; set; }
        public string ProgrammeAward { get; set; }
        public string ProgrammeStudyMode { get; set; }
        public string TransitLevel { get; set; }
        public bool IsFinalLevel { get; set; }
        //modular programmes
        public List<TransitionLevel> DiplomaModularProgressLevel()
        {
            try
            {
                List<TransitionLevel> a = new List<TransitionLevel>
                {
                    new TransitionLevel() { LevelIndex=0,ProgrammeAward = "Diploma", ProgrammeStudyMode = "Modular", TransitLevel = "Module 1-Term 1", IsFinalLevel = false },
                    new TransitionLevel() { LevelIndex=1,ProgrammeAward = "Diploma", ProgrammeStudyMode = "Modular", TransitLevel = "Module 1-Term 2", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=2, ProgrammeAward = "Diploma", ProgrammeStudyMode = "Modular", TransitLevel = "Module 2-Term 1", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=3, ProgrammeAward = "Diploma", ProgrammeStudyMode = "Modular", TransitLevel = "Module 2-Term 2", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=4, ProgrammeAward = "Diploma", ProgrammeStudyMode = "Modular", TransitLevel = "Module 3-Term 1", IsFinalLevel = false },
                    new TransitionLevel() { LevelIndex=5,ProgrammeAward = "Diploma", ProgrammeStudyMode = "Modular", TransitLevel = "Module 3-Term 2", IsFinalLevel = true }
                };

                return a;
            }
            catch
            {
                return null;
            }
        }
        public List<TransitionLevel> CertificateModularProgressLevel()
        {
            try
            {
                List<TransitionLevel> a = new List<TransitionLevel>
                {
                    new TransitionLevel() { LevelIndex=0,ProgrammeAward = "Certificate", ProgrammeStudyMode = "Modular", TransitLevel = "Module 1-Term 1", IsFinalLevel = false },
                    new TransitionLevel() { LevelIndex=1,ProgrammeAward = "Certificate", ProgrammeStudyMode = "Modular", TransitLevel = "Module 1-Term 2", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=2, ProgrammeAward = "Certificate", ProgrammeStudyMode = "Modular", TransitLevel = "Module 2-Term 1", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=3, ProgrammeAward = "Certificate", ProgrammeStudyMode = "Modular", TransitLevel = "Module 2-Term 2", IsFinalLevel = true }
                };

                return a;
            }
            catch
            {
                return null;
            }
        }
        public List<TransitionLevel> DiplomaRegularProgressLevel()
        {
            try
            {
                List<TransitionLevel> a = new List<TransitionLevel>
                {
                    new TransitionLevel() {LevelIndex=0, ProgrammeAward = "Diploma", ProgrammeStudyMode = "Regular", TransitLevel = "Year 1-Term 1", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=1, ProgrammeAward = "Diploma", ProgrammeStudyMode = "Regular", TransitLevel = "Year 1-Term 2", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=2, ProgrammeAward = "Diploma", ProgrammeStudyMode = "Regular", TransitLevel = "Year 2-Term 1", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=3, ProgrammeAward = "Diploma", ProgrammeStudyMode = "Regular", TransitLevel = "Year 2-Term 2", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=4, ProgrammeAward = "Diploma", ProgrammeStudyMode = "Regular", TransitLevel = "Year 3-Term 1", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=5, ProgrammeAward = "Diploma", ProgrammeStudyMode = "Regular", TransitLevel = "Year 3-Term 2", IsFinalLevel = true }
                };

                return a;
            }
            catch
            {
                return null;
            }
        }
        public List<TransitionLevel> CertificateRegularProgressLevel()
        {
            try
            {
                List<TransitionLevel> a = new List<TransitionLevel>
                {
                    new TransitionLevel() { LevelIndex=0,ProgrammeAward = "Certificate", ProgrammeStudyMode = "Regular", TransitLevel = "Year 1-Term 1", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=1, ProgrammeAward = "Certificate", ProgrammeStudyMode = "Regular", TransitLevel = "Year 1-Term 2", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=2, ProgrammeAward = "Certificate", ProgrammeStudyMode = "Regular", TransitLevel = "Year 2-Term 1", IsFinalLevel = false },
                    new TransitionLevel() {LevelIndex=3, ProgrammeAward = "Certificate", ProgrammeStudyMode = "Regular", TransitLevel = "Year 2-Term 2", IsFinalLevel = true }
                };

                return a;
            }
            catch
            {
                return null;
            }
        }
        public List<TransitionLevel> ArtisanProgressLevel()
        {
            try
            {
                List<TransitionLevel> a = new List<TransitionLevel>
                {
                    new TransitionLevel() { LevelIndex=0,ProgrammeAward = "Artisan", ProgrammeStudyMode = "Regular", TransitLevel = "Year 1-Term 1", IsFinalLevel = false },
                    new TransitionLevel() { LevelIndex=2,ProgrammeAward = "Artisan", ProgrammeStudyMode = "Regular", TransitLevel = "Year 1-Term 2", IsFinalLevel = true }
                };

                return a;
            }
            catch
            {
                return null;
            }
        }
        public List<TransitionLevel> GetAllProgressLevel()
        {
            List<TransitionLevel> a = new List<TransitionLevel>();
            try
            {
                a = a.Concat(DiplomaRegularProgressLevel()).Concat(DiplomaModularProgressLevel()).ToList();
                a = a.Concat(CertificateRegularProgressLevel()).Concat(CertificateModularProgressLevel()).ToList();
                a = a.Concat(ArtisanProgressLevel()).ToList();
                return a;
            }
            catch
            {
                a.Clear();
                return a;
            }
        }
        public List<TransitionLevel> GetProgrammeStudyLevels(string programme)
        {
            List<TransitionLevel> a;
            try
            {
                AcademicProgramme p = new AcademicProgramme();
                p = p.GetAcademicProgramme(programme);
                if (p == null)
                {
                    a = null;
                    MessageBox.Show("The Programme do not exist!","Message Box",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
                a=GetAllProgressLevel().Where(b=>b.ProgrammeStudyMode==p.ProgrammeStudyMode&&b.ProgrammeAward==p.ProgrammeAward).ToList();
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
