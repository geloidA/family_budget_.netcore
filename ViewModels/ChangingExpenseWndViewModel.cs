using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.ViewModels.Abstract;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class ChangingExpenseWndViewModel : ChangingTransactionViewModel
    {
        public override ICommand Command =>
            new DelegateCommand(() =>
            {
                var expense = ToChange as Expense;

                var updated = new Expense
                {
                    Id = expense.Id,
                    Classification = this.Classification,
                    Date = this.Date,
                    Description = this.Description,
                    Cost = this.Cost,
                    FamilyMemberId = this.SelectedFamilyMember.Id
                };

                DataWorker.UpdateExpense(expense.Id, updated);
            });
    }
}