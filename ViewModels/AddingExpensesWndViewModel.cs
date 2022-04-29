using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class AddingExpensesWndViewModel : ViewModelBase
    {
        private MainWndViewModel mainWndViewModel;

        public ObservableCollection<FamilyMember> FamilyMembers{ get; set; }
        public FamilyMember SelectedFamilyMember { get; set; }
        public string Classification { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }

        public AddingExpensesWndViewModel()
        {
            mainWndViewModel = (Application.Current as App).MainWindowViewModel;
            FamilyMembers = new ObservableCollection<FamilyMember>(DataWorker.FamilyMembers);
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
                    FamilyMemberId = SelectedFamilyMember.Id
                };

                mainWndViewModel.AddExpense(newExpenses);
            },
            () => IsExpenseDataFilled);

        private bool IsExpenseDataFilled => Value > 0 && SelectedFamilyMember != null
            && !string.IsNullOrEmpty(Classification);
    }
}
