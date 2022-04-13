using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family_budget.Models
{
    public class Expense : Transaction
    {
        public override string ToString()
        {
            return string.Format("{0} - {1}руб.\t{2}",this.Classification, this.Value, this.Date.ToShortDateString());
        }
    }
}
