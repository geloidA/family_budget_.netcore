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
        }
        
        public override ICommand OpenAddingTransactionPresentation
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                    await rootRegistry.ShowModalPresentation(new AddingExpensesWndViewModel());
                }, 
                () => mainVM.User.Role == "admin");
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
                () => mainVM.User.Role == "admin" && SelectedTransactionJoinFM != null);
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
                    SelectedFamilyMember = DataWorker.FamilyMembers.FirstOrDefault(m => m.Id == selectedExpense.FamilyMemberId),
                    FamilyMembers = DataWorker.FamilyMembers
                });
            }, 
            () => mainVM.User.Role == "admin" && SelectedTransactionJoinFM != null);
    }
}
