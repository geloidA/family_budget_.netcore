using DevExpress.Mvvm;
using System;

namespace family_budget.Models
{
    abstract class Transaction : ViewModelBase
    {
        public int Id { get; set; }
        public string Classification { get; set; }
        public decimal Value { get; set; }
        public FamilyMember FamilyMember { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
