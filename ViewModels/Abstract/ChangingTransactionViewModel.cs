using family_budget.Models;
using family_budget.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family_budget.ViewModels.Abstract
{
    internal abstract class ChangingTransactionViewModel : BaseTransactionViewModel
    {
        public Transaction ToChange;
    }
}
