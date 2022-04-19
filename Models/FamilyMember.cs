using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;

namespace family_budget.Models
{
    public class FamilyMember : BindableBase
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FamilyRole { get; set; }
    }
}
