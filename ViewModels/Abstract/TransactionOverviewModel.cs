using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels.Abstract
{
    internal abstract class TransactionOverviewModel : ViewModelBase
    {
        private protected MainWndViewModel mainVM;
        public TransactionOverviewModel()
        {
            mainVM = (Application.Current as App).MainWindowViewModel;
        }
        public ObservableCollection<TransactionJoinFM> Transactions { get; set; }
        public TransactionJoinFM SelectedTransactionJoinFM { get; set; }

        private protected void TransactionUpdated(Transaction toUpdate, Transaction from)
        {
            var toChange = Transactions.FirstOrDefault(t => t.TransactionId == toUpdate.Id);
            if (toChange != null)
            {
                toChange.Classification = from.Classification;
                toChange.Cost = from.Cost;
                toChange.Date = from.Date;
                toChange.Description = from.Description;
                toChange.FamilyRole = DataWorker.FamilyMembers.FirstOrDefault(f => f.Id == from.FamilyMemberId)?.FamilyRole;
            }
        }

        public virtual ICommand OpenAddingTransactionPresentation { get; }
        public virtual ICommand DeleteTransaction { get; }
        public virtual ICommand ChangeTransaction { get; }
    }
}
