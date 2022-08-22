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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagileERP.BusinessModels.StudentFeeManagement;
using TagileERP.BusinessModels.StudentManagement;

namespace TagileERP.UserInterface.StudentsFeeManagement
{
    /// <summary>
    /// Interaction logic for AddFeePayment.xaml
    /// </summary>
    public partial class AddFeePayment : Page
    {
        readonly StudentAdmission Std = new StudentAdmission();
        readonly StudentFeeStatement sf = new StudentFeeStatement();
        List<StudentFeeStatement> FeeStatement = new List<StudentFeeStatement>();
        public AddFeePayment()
        {
            InitializeComponent();
        }
        private void Button_SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StudentAdmission a=Std.GetStudentAdmissionDetails(TextBox_Searchstdregno.Text.Trim());
                if (a != null)
                {
                    Textbox_StudentRegistrationNo.Text = a.AdmissionNumber;
                    TextBox_Gender.Text = a.Gender;
                    TextBox_Status.Text = a.Status;
                    Textbox_Fullname.Text = a.FirstName + " " + a.MiddleName + " " + a.LastName;
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Combobox_PaymentMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                ComboBox a = sender as ComboBox;
                
                if (a.SelectedItem!=null)
                {
                    ResetPaymentInfo();
                    ComboBoxItem b = a.SelectedItem as ComboBoxItem;
                    Groupbox_BankReceiptPayment.Visibility = Visibility.Collapsed;
                    Groupbox_CheckPayment.Visibility = Visibility.Collapsed;
                    Groupbox_MpesaPayment.Visibility = Visibility.Collapsed;

                    if (b.Content.ToString() == "Cash")
                    {
                        Groupbox_BankReceiptPayment.Visibility = Visibility.Collapsed;
                        Groupbox_CheckPayment.Visibility = Visibility.Collapsed;
                        Groupbox_MpesaPayment.Visibility = Visibility.Collapsed;
                    }
                    else if (b.Content.ToString() == "BankReceipt")
                    {
                        Groupbox_BankReceiptPayment.Visibility = Visibility.Visible;
                    }
                    else if (b.Content.ToString() == "Check")
                    {
                        Groupbox_CheckPayment.Visibility = Visibility.Visible;
                    }
                    else if (b.Content.ToString() == "Mpesa")
                    {
                        Groupbox_MpesaPayment.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        a.SelectedItem = null;
                    }
                }
                else
                {
                    Groupbox_BankReceiptPayment.Visibility = Visibility.Collapsed;
                    Groupbox_CheckPayment.Visibility = Visibility.Collapsed;
                    Groupbox_MpesaPayment.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SavePayment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_StudentRegistrationNo.Text.Trim() != "")
                {
                    if (int.TryParse(Textbox_AmountPaid.Text, out int amountpaid))
                    {

                        if (DetailsComplete())
                        {
                            string refid = ErpShared.CurrentDate().ToString("yyyyMMddHHfffff");
                            MySqlTransaction Tr;
                            string id = Guid.NewGuid().ToString();
                            MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                            con.Open();
                            Tr = con.BeginTransaction();
                            MySqlCommand sql = new MySqlCommand("insert into studentfeeaccount(dbuid, studentregno, debit, credit, transactiontype, paymentmode, referenceid, transactiondate, description) values (@dbuid, @studentregno, @debit, @credit, @transactiontype, @paymentmode, @referenceid, @transactiondate, @description);", con, Tr);
                            sql.Parameters.AddWithValue("@dbuid", id);
                            sql.Parameters.AddWithValue("@studentregno", Textbox_StudentRegistrationNo.Text);
                            sql.Parameters.AddWithValue("@debit", 0);
                            sql.Parameters.AddWithValue("@credit", amountpaid);
                            sql.Parameters.AddWithValue("@transactiontype", ErpShared.AccountsTransactionType.Credit.ToString());
                            sql.Parameters.AddWithValue("@paymentmode", Combobox_PaymentMethod.Text);
                            sql.Parameters.AddWithValue("@referenceid", refid);
                            sql.Parameters.AddWithValue("@transactiondate", ErpShared.CurrentDate());
                            sql.Parameters.AddWithValue("@description", "Fee Payment");
                            int x = sql.ExecuteNonQuery();
                            if (x == 1)
                            {
                                FeeStatement = sf.GetStudentFeeStatement(Textbox_StudentRegistrationNo.Text);
                                double totaldebit = 0;
                                double totalcredit = 0;
                                foreach (StudentFeeStatement b in FeeStatement)
                                {
                                    totalcredit += b.Credit;
                                    totaldebit += b.Debit;
                                }
                                sql.Parameters.Clear();
                                sql = new MySqlCommand("update studentsfeemaster set debit= @debit, credit=@credit, lastpaymentdate=@lastpaymentdate where  regno=@regno;", con, Tr);
                                sql.Parameters.AddWithValue("@dbuid", id);
                                sql.Parameters.AddWithValue("@regno", Textbox_StudentRegistrationNo.Text);
                                sql.Parameters.AddWithValue("@debit",totaldebit);
                                sql.Parameters.AddWithValue("@credit", totalcredit + amountpaid);
                                sql.Parameters.AddWithValue("@lastpaymentdate", ErpShared.CurrentDate());
                                int y = sql.ExecuteNonQuery();
                                if (y == 1)
                                {
                                    Tr.Commit();
                                    MessageBox.Show("Success. Payment Received.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ResetPage();

                                }
                                else
                                {
                                    Tr.Rollback();
                                    MessageBox.Show("Failed to Update Student's Account!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                              
                            }
                            else
                            {
                                Tr.Rollback();
                                MessageBox.Show("Failed to Receive the Payment. Try again!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            con.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Icorrect Amount Paid Value!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Select a Student receiving the payment!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool DetailsComplete()
        {
            bool final = false;
            try
            {
                ComboBoxItem b = Combobox_PaymentMethod.SelectedItem as ComboBoxItem;
                if (b.Content.ToString() == "Cash")
                {
                    if(int.TryParse(Textbox_AmountPaid.Text, out int _))
                    {
                        final = true;
                        return final;
                    }
                    else
                    {
                        final = false;
                        MessageBox.Show("Icorrect Amount Paid Value!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                       return final;
                    }
                }
                else if (b.Content.ToString() == "BankReceipt")
                {
                    if (Combobox_Bank.Text.Trim() == "")
                    {
                        final = false;
                        MessageBox.Show("You must Select a Bank!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    if (Textbox_BankReceiptAccountNo.Text.Trim() == "")
                    {
                        final = false;
                        MessageBox.Show("You must enter Transacting Account Number!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    } 
                    if (Datepicker_BankReceiptTransactionDate.SelectedDate == null)
                    {
                        final = false;
                        MessageBox.Show("You must select the Transaction date!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    if (Datepicker_BankReceiptTransactionDate.SelectedDate > DateTime.Today)
                    {
                        final = false;
                        MessageBox.Show("Transaction date must not be greater than Today!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    final = true;
                    return final;
                }
                else if (b.Content.ToString() == "Check")
                {
                    if (Textbox_ChequeReceiverName.Text.Trim() == "")
                    {
                        final = false;
                        MessageBox.Show("You must enter the Cheque receiver Name!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    if (Textbox_ChequeNumber.Text.Trim() == "")
                    {
                        final = false;
                        MessageBox.Show("You must enter the Cheque Number!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    if (Textbox_ChequeAccountNo.Text.Trim() == "")
                    {
                        final = false;
                        MessageBox.Show("You must enter the Cheque Account Number!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    if (Datepicker_ChequeTransactionDate.SelectedDate ==null)
                    {
                        final = false;
                        MessageBox.Show("You must select cheque payment date!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    if (Datepicker_ChequeTransactionDate.SelectedDate>DateTime.Today)
                    {
                        final = false;
                        MessageBox.Show("Cheque payment date must not be greater than Today!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    final = true;
                    return final;
                }
                else if (b.Content.ToString() == "Mpesa")
                {
                    if (Textbox_MpesaTransactionPhoneNo.Text.Trim() == "")
                    {
                        final = false;
                        MessageBox.Show("You must enter the Mpesa Transacting Number!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    if (Textbox_MpesaTransactionCode.Text.Trim() == "")
                    {
                        final = false;
                        MessageBox.Show("You must enter the Mpesa Transaction Code!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    } 
                    if (DatePicker_MpesaTransactionDate.SelectedDate == null)
                    {
                        final = false;
                        MessageBox.Show("You must select the payment date!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    if (DatePicker_MpesaTransactionDate.SelectedDate > DateTime.Today)
                    {
                        final = false;
                        MessageBox.Show("Mpesa payment date must not be greater than Today!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return final;
                    }
                    final = true;
                    return final;
                }  
            }
            catch (Exception ex)
            {
                final = false;
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return final;
        }

        private void Button_ClearForm_Click(object sender, RoutedEventArgs e)
        {
            ResetPage();
        }
        private void ResetPage()
        {
            try
            {
                //general
                Combobox_PaymentMethod.SelectedItem = null;
                Textbox_AmountPaid.Text = "";
                ResetStudentDetails();
                ResetPaymentInfo();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ResetStudentDetails()
        {
            try
            {
                TextBox_Gender.Text = "";
                Textbox_Fullname.Text = "";
                Textbox_StudentRegistrationNo.Text = "";
                TextBox_Status.Text = "";
                TextBox_YearOfStudy.Text = "";
                TextBox_Programme.Text = ""; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ResetPaymentInfo()
        {
            try
            {
                
                //mpesa
                Textbox_MpesaTransactionCode.Text = "";
                Textbox_MpesaTransactionPhoneNo.Text = "";
                DatePicker_MpesaTransactionDate.SelectedDate=null;
                //bank
                Combobox_Bank.SelectedItem=null;
                Textbox_BankReceiptAccountNo.Text = "";
                Datepicker_BankReceiptTransactionDate.SelectedDate = null;
                //cheque 
                Textbox_ChequeReceiverName.Text = "";
                Textbox_ChequeAccountNo.Text = "";
                Textbox_ChequeNumber.Text = "";
                Datepicker_ChequeTransactionDate.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
    }
     
}
