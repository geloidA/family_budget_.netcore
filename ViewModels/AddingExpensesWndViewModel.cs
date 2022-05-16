using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.ViewModels.Abstract;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class AddingExpensesWndViewModel : AddingTransactionViewModel
    {
        public override ICommand Command => new DelegateCommand(() =>
        {
            var newExpenses = new Expense
            {
                Classification = this.Classification,
                Date = this.Date,
                Cost = this.Cost,
                Description = this.Description,
                FamilyMemberId = SelectedFamilyMember.Id
            };

            //TODO Переделать на DataInProgram
            mainWndViewModel.AddExpense(newExpenses);
        }, () => IsCanExecute);

        public override bool IsCanExecute => Cost > 0 && SelectedFamilyMember != null
            && !string.IsNullOrEmpty(Classification);
    }
}
