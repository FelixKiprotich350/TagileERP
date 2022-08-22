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
using TagileERP.BusinessModels.StudentFeeManagement;

namespace TagileERP.UserInterface.StudentsFeeManagement
{
    /// <summary>
    /// Interaction logic for StudentsFeeBalancesList.xaml
    /// </summary>
    public partial class StudentsFeeBalancesList : Page
    {
        readonly StudentsFeeAccount sa = new StudentsFeeAccount();
        ObservableCollection<StudentsFeeAccount> BalanceList = new ObservableCollection<StudentsFeeAccount>();
        public StudentsFeeBalancesList()
        {
            InitializeComponent();
        }

        private void ViewAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BalanceList.Clear();
                BalanceList = new ObservableCollection<StudentsFeeAccount>(sa.GetStudentsBalances());
                if (BalanceList != null)
                {
                    ////to find totals
                    //foreach (StudentsFeeAccount b in BalanceList)
                    //{
                    //    totalcredit += b.Credit;
                    //    totaldebit += b.Debit;
                    //}
                    
                    FeeBalanceListGrid.ItemsSource = null;
                    FeeBalanceListGrid.Items.Clear();
                    FeeBalanceListGrid.ItemsSource = BalanceList;
                }
                else
                {
                    MessageBox.Show("No records found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
    }
}
