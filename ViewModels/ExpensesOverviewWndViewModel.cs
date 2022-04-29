using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class ExpensesOverviewWndViewModel : ViewModelBase
    {
        public ICommand OpenAddingExpensesPresentation
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                    await rootRegistry.ShowModalPresentation(new AddingExpensesWndViewModel());
                });
            }
        }
    }
}
