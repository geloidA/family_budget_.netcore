using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class ChangingExpensesWndViewModel : AddingTransactionModel
    {
        private Expense changeExpense;

        public ChangingExpensesWndViewModel(Expense changeExpense) : base()
        {
            this.changeExpense = changeExpense;
            Cost = changeExpense.Cost;
            Date = changeExpense.Date;
            Description = changeExpense.Description;
            SelectedFamilyMember = DataWorker.FamilyMembers.FirstOrDefault(m => m.Id == changeExpense.FamilyMemberId);
            Classification = changeExpense.Classification;
        }

        public override ICommand AddTransaction => 
            new DelegateCommand(() =>
            {
                //TODO 
            }); 
    }
}
