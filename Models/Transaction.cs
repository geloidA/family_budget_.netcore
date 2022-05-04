using DevExpress.Mvvm;
using System;

namespace family_budget.Models
{
    public abstract class Transaction : BindableBase
    {
        public int Id { get; set; }
        public string Classification { get; set; }
        public double Cost { get; set; }
        public int FamilyMemberId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return string.Join('\t', Classification, Cost, Date.ToShortDateString());
        }
    }
}
