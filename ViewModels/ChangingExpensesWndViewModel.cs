using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class ChangingExpensesWndViewModel : ChangingTransactionViewModel
    {
        public ChangingExpensesWndViewModel(Transaction changeExpense) : base(changeExpense)
        {
            if(changeExpense is not Expense)
                throw new ArgumentException(nameof(changeExpense));
        }

        public override ICommand Command => 
            new DelegateCommand(() =>
            {
                var expense = _changeTransaction as Expense;
                //TODO
            });
    }
}
