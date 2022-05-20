using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.ViewModels.Abstract;
using System;
using System.Linq;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class ChangingIncomeWndViewModel : ChangingTransactionViewModel
    {
        public ChangingIncomeWndViewModel()
        {
            Cost = ToChange.Cost;
            Date = ToChange.Date;
            Description = ToChange.Description;
            SelectedFamilyMember = DataWorker.FamilyMembers.FirstOrDefault(m => m.Id == ToChange.FamilyMemberId);
            Classification = ToChange.Classification;
        }
        public override ICommand Command =>
            new DelegateCommand(() =>
            {
                var income = ToChange as Income;

                if(income == null)
                    throw new ArgumentException(nameof(income));
                var updated = new Income
                {
                    Classification = this.Classification,
                    Date = this.Date,
                    Description = this.Description,
                    Cost = this.Cost,
                    FamilyMemberId = this.SelectedFamilyMember.Id
                };

                DataWorker.UpdateIncome(income.Id, updated);
                //TODO
            });
    }
}
