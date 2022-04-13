using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Views;

namespace family_budget.ViewModels
{
    class AuthorizetionViewModel : ViewModelBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public ICommand AuthorizeUser { get; }
        public ICommand RegisterUser { get; set; }

        public AuthorizetionViewModel()
        {
            AuthorizeUser =
                new DelegateCommand(() =>
                {
                    var user = new User { Login = this.Login, Password = this.Password };
                    if (User.Authorize(user))
                        new MainWindow().Show();
                });
            RegisterUser = new DelegateCommand(() => new RegistrationWnd().Show());
        }
    }
}
