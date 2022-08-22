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
using TagileERP.BusinessModels.StudentFeeManagement;

namespace TagileERP.UserInterface.StudentsFeeManagement
{
    /// <summary>
    /// Interaction logic for ViewFeeStructureDetails.xaml
    /// </summary>
    public partial class ViewFeeStructureDetails : Window
    {
         ObservableCollection<FSVoteheadLog> Votehead_List_Items = new ObservableCollection<FSVoteheadLog>();
        private readonly FeeStructure FS =null;
        readonly FSVoteheadLog fs = new FSVoteheadLog();
        public ViewFeeStructureDetails(FeeStructure Fs)
        {
            InitializeComponent();
            this.FS = Fs;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_AcademicYear.Text = FS.FSAcademicYear;
                Textbox_GOKAmount.Text = FS.GOk.ToString();
                Textbox_TraineeAmount.Text = FS.Trainee.ToString();
                Textbox_TotalAmount.Text = (FS.Trainee+FS.GOk).ToString();
                Votehead_List_Items = new ObservableCollection<FSVoteheadLog>(fs.GetFS_VoteheadLogs(FS.FSCode)); 
                Session1_Datagrid.ItemsSource = Votehead_List_Items.Where(b => b.Session == "Term 1");
                Session2_Datagrid.ItemsSource = Votehead_List_Items.Where(b => b.Session == "Term 2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
