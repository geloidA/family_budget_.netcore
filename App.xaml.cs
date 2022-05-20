using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using family_budget.ViewModels;
using family_budget.Views;
using family_budget.Services;

namespace family_budget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry DisplayRootRegistry = new DisplayRootRegistry();
        public readonly MainWndViewModel MainWindowViewModel;

        public App()
        {
            DisplayRootRegistry.RegisterWindowType<MainWndViewModel, MainWindow>();
            DisplayRootRegistry.RegisterWindowType<AddingExpensesWndViewModel, AddingExpensesWnd>();
            DisplayRootRegistry.RegisterWindowType<AddingIncomesWndViewModel, AddingIncomesWnd>();
            DisplayRootRegistry.RegisterWindowType<ExpensesOverviewWndViewModel, ExpensesOverviewWnd>();
            DisplayRootRegistry.RegisterWindowType<IncomesOverviewWndViewModel, IncomesOverviewWnd>();
            DisplayRootRegistry.RegisterWindowType<ChangingExpenseWndViewModel, ChangingExpenseWnd>();
            DisplayRootRegistry.RegisterWindowType<ChangingIncomeWndViewModel, ChangingIncomeWnd>();
            DisplayRootRegistry.RegisterWindowType<AuthorizetionViewModel, AuthorizationWnd>();
            DisplayRootRegistry.RegisterWindowType<RegistrationWndViewModel, RegistrationWnd>();
            MainWindowViewModel = new MainWndViewModel();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await DisplayRootRegistry.ShowModalPresentation(MainWindowViewModel);

            Shutdown();
        }
    }
}
