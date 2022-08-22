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

namespace TagileERP.UserInterface.Administration
{
    /// <summary>
    /// Interaction logic for InstitutionSetup.xaml
    /// </summary>
    public partial class InstitutionSetup : Page
    {
        readonly InstitutionModelProperty imp = new InstitutionModelProperty();
        public InstitutionSetup()
        {
            InitializeComponent();
        }

        private void Buttton_Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<InstitutionModelProperty> a =imp.GetInstitutionProperties();
                if (a.Where(v=>v.ItemId==1).First().Value1== "Total Agile Solutions")
                {
                    MessageBox.Show("You have not set the institution info.","Message Box",MessageBoxButton.OK,MessageBoxImage.Stop);
                    return;
                } 
                if (a.Count>5)
                {
                    Textbox_InstitutionName.Text = a.Where(b => b.ItemName == "instname").First().Value1;
                    Textbox_InstitutionPhysicalAddress.Text = a.Where(b=>b.ItemName=="instaddress").First().Value1;
                    Textbox_InstitutionEmailaddress.Text = a.Where(b=>b.ItemName=="instemail").First().Value1;
                    Textbox_InstitutionTelephone1.Text = a.Where(b=>b.ItemName=="instphone1").First().Value1;
                    Textbox_InstitutionTelephone2.Text = a.Where(b=>b.ItemName=="instphone2").First().Value1;
                    Textbox_InstitutionWebaddress.Text = a.Where(b=>b.ItemName== "instwebsite").First().Value1;
                    Textbox_InstitutionMotto.Text = a.Where(b=>b.ItemName== "instmotto").First().Value1;
                }
                else
                {
                    MessageBox.Show("Incomplete Institution Details. Contact Developer for more info.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
