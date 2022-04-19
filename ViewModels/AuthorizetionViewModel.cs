using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using System;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using family_budget.Services;

namespace family_budget.ViewModels
{
    class AuthorizetionViewModel : ViewModelBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string StatusBar { get; set; }
        public ICommand Authorize
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (DataWorker.TryFindUser(Login, Password, out User user))
                    {
                        var app = Application.Current as App;
                        app.MainWindowViewModel.User = user;
                        app.DisplayRootRegistry.HidePresentation(this);
                    }
                    else StatusBar = "Неверный логин или пароль";
                });
            }
        }

        public ICommand OpenRegistrationPresentation
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                    if (!rootRegistry.ViewModels.Any(x => x.GetType() == typeof(RegistrationWndViewModel)))
                    {
                        rootRegistry.ShowPresentation(new RegistrationWndViewModel());
                        StatusBar = "Готово";
                    }
                    else StatusBar = "Окно уже открыто";
                });
            }
        }
    }
}
