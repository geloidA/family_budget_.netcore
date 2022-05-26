using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class ExpensesOverviewWndViewModel : TransactionOverviewModel
    {
        public ExpensesOverviewWndViewModel()
        {
            Transactions = new ObservableCollection<TransactionJoinFM>(DataWorker.Expenses.Join(
                        DataWorker.FamilyMembers,
                        e => e.FamilyMemberId,
                        f => f.Id,
                        (e, f) => new TransactionJoinFM
                        {
                            TransactionId = e.Id,
                            Classification = e.Classification,
                            Cost = e.Cost,
                            Date = e.Date,
                            Description = e.Description,
                            FamilyRole = f.FamilyRole
                        }));

            AverageTransactCostByMonth = Transactions.GroupBy(m => (Months)m.Date.Month)
                .Select(month => (month.Key, month.Sum(t => t.Cost)))
                .Sum(m => m.Item2) / 12;

            DataWorker.Expenses.CollectionChanged += Expenses_CollectionChanged;
            DataWorker.ExpenseUpdated += (toUpdate, from) => TransactionUpdated(toUpdate, from);
        }

        private void Expenses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newExpense = e.NewItems.Cast<Expense>().FirstOrDefault();
                    if (newExpense != null)
                    {
                        Transactions.Add(new TransactionJoinFM
                        {
                            Classification = newExpense.Classification,
                            Cost = newExpense.Cost,
                            Date = newExpense.Date,
                            Description = newExpense.Description,
                            FamilyRole = DataWorker.FamilyMembers.FirstOrDefault(f => f.Id == newExpense.FamilyMemberId)?.FamilyRole,
                            TransactionId = newExpense.Id
                        });
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var oldExpense = e.OldItems.Cast<Expense>().FirstOrDefault();
                    if (oldExpense != null)
                        Transactions.Remove(Transactions.FirstOrDefault(t => t.TransactionId == oldExpense.Id));
                    break;
            }
        }

        public override ICommand OpenAddingTransactionPresentation =>
            new DelegateCommand(async () =>
            {
                var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                await rootRegistry.ShowModalPresentation(new AddingExpensesWndViewModel());
            }, () => mainVM.User?.Role == "admin");

        public override ICommand DeleteTransaction =>
            new DelegateCommand(() =>
            {
                var selected = SelectedTransactionJoinFM;
                Transactions.Remove(selected);
                var toRemove = DataWorker.Expenses.FirstOrDefault(e => e.Id == selected.TransactionId);
                DataWorker.RemoveExpense(toRemove);
            }, () => mainVM.User?.Role == "admin" && SelectedTransactionJoinFM != null);

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
                    SelectedFamilyMember = DataWorker.FamilyMembers.FirstOrDefault(m => m.Id == selectedExpense.FamilyMemberId),
                    FamilyMembers = DataWorker.FamilyMembers
                });
            }, 
            () => mainVM.User?.Role == "admin" && SelectedTransactionJoinFM != null);
    }
}