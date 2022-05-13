using DevExpress.Mvvm;
using System;

namespace family_budget.Models
{
    public class ExpenseJoinFM : BindableBase
    {
        public int ExpenseId { get; set; }
        public string Classification { get; set; }
        public double Cost { get; set; }
        public string FamilyRole { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
