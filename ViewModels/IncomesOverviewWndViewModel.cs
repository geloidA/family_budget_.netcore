using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.ViewModels.Abstract;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class IncomesOverviewWndViewModel : TransactionOverviewModel
    {
        public IncomesOverviewWndViewModel()
        {
            Transactions = new ObservableCollection<TransactionJoinFM>(DataWorker.Incomes.Join(
                        DataWorker.FamilyMembers,
                        i => i.FamilyMemberId,
                        f => f.Id,
                        (i, f) => new TransactionJoinFM
                        {
                            TransactionId = i.Id,
                            Classification = i.Classification,
                            Cost = i.Cost,
                            Date = i.Date,
                            Description = i.Description,
                            FamilyRole = f.FamilyRole
                        }));
            DataWorker.IncomeUpdated += (toUpdate, from) => TransactionUpdated(toUpdate, from);
            DataWorker.Incomes.CollectionChanged += Incomes_CollectionChanged;
        }

        private void Incomes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newIncome = e.NewItems.Cast<Income>().FirstOrDefault();
                    if(newIncome != null)
                    {
                        Transactions.Add(new TransactionJoinFM
                        {
                            Classification = newIncome.Classification,
                            Cost = newIncome.Cost,
                            Date = newIncome.Date,
                            Description = newIncome.Description,
                            FamilyRole = DataWorker.FamilyMembers.FirstOrDefault(f => f.Id == newIncome.FamilyMemberId)?.FamilyRole,
                            TransactionId = newIncome.Id
                        });
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var oldIncome = e.OldItems.Cast<Income>().FirstOrDefault();
                    if(oldIncome != null)
                        Transactions.Remove(Transactions.FirstOrDefault(t => t.TransactionId == oldIncome.Id));
                    break;
            }
        }

        public override ICommand ChangeTransaction => 
            new DelegateCommand(async () =>
            {
                var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                var selectedIncome = DataWorker.Incomes.FirstOrDefault(i => i.Id == SelectedTransactionJoinFM.TransactionId);

                await rootRegistry.ShowModalPresentation(new ChangingIncomeWndViewModel()
                {
                    ToChange = selectedIncome,
                    Cost = selectedIncome.Cost,
                    Date = selectedIncome.Date,
                    Description = selectedIncome.Description,
                    Classification = selectedIncome.Classification,
                    SelectedFamilyMember = DataWorker.FamilyMembers.FirstOrDefault(m => m.Id == selectedIncome.FamilyMemberId),
                    FamilyMembers = DataWorker.FamilyMembers
                });
            }, () => mainVM.User?.Role == "admin" && SelectedTransactionJoinFM != null);

        public override ICommand DeleteTransaction =>
            new DelegateCommand(() =>
            {
                var selected = SelectedTransactionJoinFM;
                Transactions.Remove(selected);
                var toRemove = DataWorker.Incomes.FirstOrDefault(e => e.Id == selected.TransactionId);
                DataWorker.RemoveIncome(toRemove);
            }, () => mainVM.User?.Role == "admin" && SelectedTransactionJoinFM != null);

        public override ICommand OpenAddingTransactionPresentation =>
            new DelegateCommand(async () =>
            {
                var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                await rootRegistry.ShowModalPresentation(new AddingIncomesWndViewModel());
            },() => mainVM.User?.Role == "admin");
    }
}