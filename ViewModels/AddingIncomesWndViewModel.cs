using DevExpress.Mvvm;
using family_budget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class AddingIncomesWndViewModel : AddingTransactionModel
    {
        public override ICommand AddTransaction => new DelegateCommand(() =>
        {
            var newIncome = new Income
            {
                Classification = this.Classification,
                Date = this.Date,
                Cost = this.Cost,
                Description = this.Description,
                FamilyMemberId = SelectedFamilyMember.Id
            };
            mainWndViewModel.AddIncome(newIncome);

        }, () => IsCanExecute);
        public override bool IsCanExecute => Cost > 0 && SelectedFamilyMember != null
            && !string.IsNullOrEmpty(Classification);
    }
}
