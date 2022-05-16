using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class AddingIncomesWndViewModel : AddingTransactionViewModel
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

            //TODO DataInProgram
            mainWndViewModel.AddIncome(newIncome);

        }, () => IsCanExecute);
        public override bool IsCanExecute => Cost > 0 && SelectedFamilyMember != null
            && !string.IsNullOrEmpty(Classification);
    }
}
