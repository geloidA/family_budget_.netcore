using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Linq;
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
                    var selected = SelectedTransactionJoinFM;
                    Transactions.Remove(selected);
                    var toRemove = DataWorker.Expenses.FirstOrDefault(e => e.Id == selected.TransactionId);
                    DataWorker.RemoveExpense(toRemove);
                },
                () => SelectedTransactionJoinFM != null);
            }
        }

        public override ICommand ChangeTransaction =>
            new DelegateCommand(async () =>
            {
                var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                var selectedExpense = DataWorker.Expenses.FirstOrDefault(e => e.Id == SelectedTransactionJoinFM.TransactionId);

                await rootRegistry.ShowModalPresentation(new ChangingExpenseWndViewModel()
                {
                    ToChange = selectedExpense,
                    Cost = selectedExpense.Cost,
                    Date = selectedExpense.Date,
                    Description = selectedExpense.Description,
                    Classification = selectedExpense.Classification,
                    //TODO:
                    SelectedFamilyMember = DataWorker.FamilyMembers.FirstOrDefault(m => m.Id == selectedExpense.FamilyMemberId),
                    FamilyMembers = new ObservableCollection<FamilyMember>(DataWorker.FamilyMembers)
                });
            });
    }
}
