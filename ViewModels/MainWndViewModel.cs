using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    public class MainWndViewModel : ViewModelBase
    {
        public User User { get; set; }
        public string StatusBar { get; set; }
        public ObservableCollection<Expense> Expense { get; set; }

        public MainWndViewModel()
        {
            Expense = new ObservableCollection<Expense>(DataWorker.Expenses);
        }

        public ICommand OpenAuthorizationPresentation
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var rootRegistry= (Application.Current as App).DisplayRootRegistry;
                    if (!rootRegistry.ViewModels.Any(x => x.GetType() == typeof(AuthorizetionViewModel)))
                    {
                        rootRegistry.ShowPresentation(new AuthorizetionViewModel());
                        StatusBar = "Готово";
                    }
                    else StatusBar = "Окно уже открыто";
                }, () => User == null);
            }
        }
    }
}
