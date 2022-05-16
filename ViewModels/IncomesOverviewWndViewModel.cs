using family_budget.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using family_budget.Models;
using family_budget.Models.DataBase;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class IncomesOverviewWndViewModel : TransactionOverviewModel
    {
        public IncomesOverviewWndViewModel()
        {
            //TODO: Сделать инициализацию DataInProgram
            Transactions = new ObservableCollection<TransactionJoinFM>();
            SelectedTransactionJoinFM = Transactions.FirstOrDefault();
        }

        //TODO:
        public override ICommand ChangeTransaction => base.ChangeTransaction;
        public override ICommand DeleteTransaction => base.DeleteTransaction;
        public override ICommand OpenAddingTransactionPresentation => base.OpenAddingTransactionPresentation;
    }
}
