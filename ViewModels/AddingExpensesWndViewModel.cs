using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.Services;
using family_budget.ViewModels.Abstract;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class AddingExpensesWndViewModel : BaseTransactionViewModel
    {
        public override ICommand Command => new DelegateCommand(() =>
        {
            var newExpense = new Expense
            {
                Classification = this.Classification,
                Date = this.Date,
                Cost = this.Cost,
                Description = this.Description,
                FamilyMemberId = SelectedFamilyMember.Id
            };
            //TODO
            DataWorker.AddExpense(newExpense);
        }, () => IsCanExecute);
    }
}
