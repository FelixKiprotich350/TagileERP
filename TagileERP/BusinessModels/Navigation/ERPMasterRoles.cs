using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagileERP.BusinessModels.Navigation;

namespace TagileERP.BusinessModels.Security
{
    public class ERPMasterRoles
    {
        public ERPMasterRoles()
        {
            MenuCategoriesList.Clear();
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Administration", GroupCode = "A",GroupIcon= "Administrator" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Students Management", GroupCode = "B", GroupIcon = "AccountStudent" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Academics & Examination", GroupCode = "C", GroupIcon = "Book" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Student Fees", GroupCode = "D", GroupIcon = "CurrencyUsd" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Hostels Management", GroupCode = "E", GroupIcon = "Hotel" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Human resource", GroupCode = "F", GroupIcon = "People" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Finance & Accounting", GroupCode = "G" ,GroupIcon= "CurrencyUsd" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Library Management", GroupCode = "H", GroupIcon = "Library" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Procurement & Inventory ", GroupCode = "I", GroupIcon = "Store" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Fleet Management", GroupCode = "J", GroupIcon = "Fleet" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Hospital ", GroupCode = "K", GroupIcon = "Hospital" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Catering Section", GroupCode = "L", GroupIcon = "Food" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Communication ", GroupCode = "M", GroupIcon = "Email" });
           // MenuCategoriesList.Add(new Level1menu() { GroupName = "My Account ", GroupCode = "N", GroupIcon = "Person" });
        }
        readonly public List<Level1menu> MenuCategoriesList = new List<Level1menu>();
    }
}
