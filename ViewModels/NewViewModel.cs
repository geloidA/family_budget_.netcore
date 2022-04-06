using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    class NewViewModel : ViewModelBase
    {
        public int Clicks { get; set; }

        public ICommand ClicksAdd
        {
            get
            {
                return new DelegateCommand(() => Clicks++);
            }
        }
    }
}
