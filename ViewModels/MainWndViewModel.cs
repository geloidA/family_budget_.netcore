using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using System.Windows.Input;
using System.Collections.ObjectModel;
using family_budget.Models;
using family_budget.Models.DataBase;

namespace family_budget.ViewModels
{
    public class MainWndViewModel : ViewModelBase
    {
        public User User { get; set; }
        public ObservableCollection<Expense> Expense { get; set; }

        public MainWndViewModel()
        {
            Expense = new ObservableCollection<Expense>(DataWorker.Expenses);
        }
    }
}
