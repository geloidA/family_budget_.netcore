using DevExpress.Mvvm;
using System;

namespace family_budget.Models
{
    public abstract class Transaction : BindableBase
    {
        public int Id { get; set; }
        public string Classification { get; set; }
        public double Value { get; set; }
        public int FamilyMemberId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
