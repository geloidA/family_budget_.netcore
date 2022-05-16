using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.ViewModels.Abstract;
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
    internal class ExpensesOverviewWndViewModel : TransactionOverviewModel
    {
        public ExpensesOverviewWndViewModel()
        {
            //TODO: Inizialization
            Transactions = new ObservableCollection<TransactionJoinFM>(DataWorker.ExpensesJoinFamilyMembers);
            SelectedTransactionJoinFM = Transactions.FirstOrDefault();
        }

        public override ICommand OpenAddingTransactionPresentation
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

        public override ICommand DeleteTransaction
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Transactions.Remove(SelectedTransactionJoinFM);
                    var toRemove = DataWorker.Expenses.FirstOrDefault(e => e.Id == SelectedTransactionJoinFM.TransactionId);
                    DataWorker.RemoveExpense(toRemove);
                },
                () => SelectedTransactionJoinFM != null);
            }
        }
    }
}
