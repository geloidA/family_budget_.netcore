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
        public override ICommand Command =>
            new DelegateCommand(() =>
            {
                var income = ToChange as Income;

                if(income == null)
                    throw new ArgumentException(nameof(income));
                var updated = new Income
                {
                    Id = income.Id,
                    Classification = this.Classification,
                    Date = this.Date,
                    Description = this.Description,
                    Cost = this.Cost,
                    FamilyMemberId = this.SelectedFamilyMember.Id
                };

                DataWorker.UpdateIncome(income.Id, updated);
            });
    }
}
