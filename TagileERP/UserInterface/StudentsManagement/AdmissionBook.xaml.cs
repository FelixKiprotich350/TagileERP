using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TagileERP.BusinessModels.StudentManagement;

namespace TagileERP.UserInterface.StudentsManagement
{
    /// <summary>
    /// Interaction logic for StudentsList.xaml
    /// </summary>
    public partial class AdmissionBook : Page
    {
         ObservableCollection<StudentRegistration> Students_List_Items = new ObservableCollection<StudentRegistration>();
        readonly StudentAdmission std = new StudentAdmission();

        public AdmissionBook()
        {
            InitializeComponent();
        }

        private void StdId_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox t = (TextBox)sender;
            string filter = t.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(StudentsListGrid.ItemsSource);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    StudentAdmission p = o as StudentAdmission;
                    return p.AdmissionNumber.ToLower().Contains(filter.ToLower()); 
                };
            } 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StudentsListGrid.ItemsSource = null;
            StudentsListGrid.Items.Clear();
            
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //use datatble to query dynamic column set by the user
                Students_List_Items.Clear();
                Students_List_Items = new ObservableCollection<StudentRegistration>(std.GetStudentsAdmissionsList());

                if (Students_List_Items.Count<=0)
                {
                    MessageBox.Show("No Students Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }  
                StudentsListGrid.ItemsSource = null;
                StudentsListGrid.Items.Clear();
                StudentsListGrid.ItemsSource = Students_List_Items;
                Textbox_TotalStudents.Text = Students_List_Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    } 
}