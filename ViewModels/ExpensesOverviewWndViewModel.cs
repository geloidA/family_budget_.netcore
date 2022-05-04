using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class ExpensesOverviewWndViewModel : ViewModelBase
    {
        public ObservableCollection<ExpenseJoinFM> Expenses { get; set; }

        public ExpensesOverviewWndViewModel()
        {
            Expenses = new ObservableCollection<ExpenseJoinFM>(DataWorker.ExpensesJoinFamilyMembers);
        }

        public ICommand OpenAddingExpensesPresentation
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                    await rootRegistry.ShowModalPresentation(new AddingExpensesWndViewModel());
                });
            }
        }
    }
}
