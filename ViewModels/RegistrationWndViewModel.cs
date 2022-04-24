using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using family_budget.Services;

namespace family_budget.ViewModels
{
    class RegistrationWndViewModel : ViewModelBase
    {
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string StatusBar { get; set; }

        PatternChecker passwordChecker;
        PatternChecker loginChecker;

        public RegistrationWndViewModel()
        {
            passwordChecker = new PatternChecker(new Dictionary<string, string>
            {
                { "[0-9]", "Пароль не содержит цифр"},
                { "[a-z]", "Пароль не содержит маленьких букв" },
                { "[!@#$%^]", "Пароль не содержит спецсимвол" },
                { "[A-Z]", "Пароль не содержит больших букв" }
            });

            loginChecker = new PatternChecker(new Dictionary<string, string>
            {
                { "^(?!.*[а-яА-Я]).*$", "Логин не должен содержать буквы кириллицы"},
                { "[a-zA-Z]", "Логин не содержит латинских букв" }
            });

            StatusBar = "Готово";
        }

        public ICommand Registrate
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(IsRegisterDataCorrect(out string message))
                    {
                        var newUser = new User() { FullName = FullName, Login = Login, Password = Password };
                        var isCopy = DataWorker.Users.Any(u => u.Login == newUser.Login);

                        if (!isCopy)
                        {
                            DataWorker.AddUser(newUser);
                            (Application.Current as App).DisplayRootRegistry.HidePresentation(this);
                        }
                        else StatusBar = "Логин занят.";
                    }
                    else StatusBar = message;
                },
                () => !IsRegisterDataEmpty);
            }
        }

        private bool IsRegisterDataCorrect(out string message)
        {
            var result = loginChecker.Check(Login, out string loginMessage) & passwordChecker.Check(Password, out string passwordMessage);
            message = string.Join('\n', loginMessage, passwordMessage);
            return result;
        }
        
        private bool IsRegisterDataEmpty => string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Login);

        public ICommand Cancel => new DelegateCommand(() =>
                    (Application.Current as App).DisplayRootRegistry.HidePresentation(this));        
    }
}
