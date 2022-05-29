using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family_budget.Models
{
    public class MemberCosts : BindableBase
    {
        public FamilyMember Member { get; set; }
        public double Incomes { get; set; }
        public double Expenses { get; set; }
    }
}
