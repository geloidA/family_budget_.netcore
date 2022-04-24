using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class AddingExpensesWndViewModel : ViewModelBase
    {
        private MainWndViewModel mainWndViewModel;

        public string Classification { get; set; }
        public double Value { get; set; }
        public int FamilyMemberId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public AddingExpensesWndViewModel()
        {
            mainWndViewModel = (Application.Current as App).MainWindowViewModel;
        }

        public ICommand AddExpense
            => new DelegateCommand(() =>
            {
                var newExpenses = new Expense
                {
                    Classification = this.Classification,
                    Date = this.Date,
                    Value = this.Value,
                    Description = this.Description,
                    FamilyMemberId = this.FamilyMemberId
                };

                DataWorker.AddExpense(newExpenses);
                mainWndViewModel.AddExpense(newExpenses);
            }, 
            () => !IsExpenseDataFilled);

        private bool IsExpenseDataFilled =>
            this.Classification.Length > 0 || Value > 0 || FamilyMemberId > 0;
    }
}
