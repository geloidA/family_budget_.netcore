using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family_budget.Models
{
    enum Month
    {
        [Description("Янв")]
        Jan = 1,
        [Description("Фев")]
        Feb,
        [Description("Мар")]
        Mar,
        [Description("Апр")]
        Apr,
        [Description("Май")]
        May,
        [Description("Июн")]
        Jun,
        [Description("Июл")]
        Jul,
        [Description("Авг")]
        Aug,
        [Description("Сен")]
        Sep,
        [Description("Окт")]
        Oct,
        [Description("Ноя")]
        Nov,
        [Description("Дек")]
        Dec,
        [Description("Среднее")]
        Average
    }
}
