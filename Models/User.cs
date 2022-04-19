using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using family_budget.Models.DataBase;

namespace family_budget.Models
{
    public class User : BindableBase
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }        
    }
}
