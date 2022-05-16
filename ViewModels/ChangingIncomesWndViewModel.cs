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
    internal class ChangingIncomesWndViewModel : ChangingTransactionViewModel
    {
        public ChangingIncomesWndViewModel(Transaction changeIncome) : base(changeIncome)
        {
            if(changeIncome is not Income)
                throw new ArgumentException(nameof(changeIncome));
        }

        public override ICommand Command => 
            new DelegateCommand(() =>
            {
                var income = _changeTransaction as Income;
                //TODO
            });
    }
}
