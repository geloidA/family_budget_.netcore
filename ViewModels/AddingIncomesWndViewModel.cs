using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.ViewModels.Abstract;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class AddingIncomesWndViewModel : BaseTransactionViewModel
    {
        public override ICommand Command => new DelegateCommand(() =>
        {
            var newIncome = new Income
            {
                Classification = this.Classification,
                Date = this.Date,
                Cost = this.Cost,
                Description = this.Description,
                FamilyMemberId = SelectedFamilyMember.Id
            };

            DataWorker.AddIncome(newIncome);

        }, () => IsCanExecute);
    }
}