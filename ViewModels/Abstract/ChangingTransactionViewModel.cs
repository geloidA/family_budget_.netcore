using family_budget.Models;
using family_budget.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family_budget.ViewModels.Abstract
{
    internal abstract class ChangingTransactionViewModel : AddingTransactionViewModel
    {
        private protected Transaction _changeTransaction;
        public ChangingTransactionViewModel(Transaction changeTransaction)
        {
            _changeTransaction = changeTransaction;
            Cost = changeTransaction.Cost;
            Date = changeTransaction.Date;
            Description = changeTransaction.Description;
            SelectedFamilyMember = DataWorker.FamilyMembers.FirstOrDefault(m => m.Id == changeTransaction.FamilyMemberId);
            Classification = changeTransaction.Classification;
        }
    }
}
