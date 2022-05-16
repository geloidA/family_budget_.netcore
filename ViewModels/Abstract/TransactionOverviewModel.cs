using DevExpress.Mvvm;
using family_budget.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace family_budget.ViewModels.Abstract
{
    internal abstract class TransactionOverviewModel : ViewModelBase
    {
        public ObservableCollection<TransactionJoinFM> Transactions { get; set; }
        public TransactionJoinFM SelectedTransactionJoinFM { get; set; }

        public virtual ICommand OpenAddingTransactionPresentation { get; }
        public virtual ICommand DeleteTransaction { get; }
        public virtual ICommand ChangeTransaction { get; }
    }
}
