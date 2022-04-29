using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    public class MainWndViewModel : ViewModelBase
    {
        public User User { get; set; }
        public string StatusBar { get; set; } = "Готово";
        public SeriesCollection Expense { get; set; }
        public SeriesCollection Incomes { get; set; }

        private readonly Dictionary<string, ObservableValue> AmountsGroupedExpenses;
        private readonly Dictionary<string, ObservableValue> AmountsGroupedIncomes;

        public MainWndViewModel()
        {
            AmountsGroupedExpenses = new Dictionary<string, ObservableValue>(DataWorker.Expenses.GroupBy(e => e.Classification)
                .Select(g => new KeyValuePair<string, ObservableValue>(g.Key, new ObservableValue(g.Sum(e => e.Value)))));

            AmountsGroupedIncomes = new Dictionary<string, ObservableValue>(DataWorker.Incomes.GroupBy(e => e.Classification)
                .Select(g => new KeyValuePair<string, ObservableValue>(g.Key, new ObservableValue(g.Sum(e => e.Value)))));

            Expense = new SeriesCollection();
            Expense.AddRange(AmountsGroupedExpenses.Select(pair =>
            {
                return new PieSeries
                {
                    Title = pair.Key,
                    Values = new ChartValues<ObservableValue> { pair.Value }
                };
            }));

            Incomes = new SeriesCollection();
            Incomes.AddRange(AmountsGroupedIncomes.Select(pair =>
            {
                return new PieSeries
                {
                    Title = pair.Key,
                    Values = new ChartValues<ObservableValue> { pair.Value }
                };
            }));
        }

        public void AddExpense(Expense newExpense)
        {
            AddTransactionIn(newExpense, AmountsGroupedExpenses);

            Expense.Add(new PieSeries
            {
                Title = newExpense.Classification,
                Values = new ChartValues<ObservableValue> { AmountsGroupedExpenses[newExpense.Classification] }
            });

            DataWorker.AddExpense(newExpense);
        }

        public void AddIncome(Income newIncome)
        {
            AddTransactionIn(newIncome, AmountsGroupedIncomes);

            Incomes.Add(new PieSeries
            {
                Title = newIncome.Classification,
                Values = new ChartValues<ObservableValue> { AmountsGroupedIncomes[newIncome.Classification] }
            });

            DataWorker.AddIncome(newIncome);
        }

        private void AddTransactionIn(Transaction transaction, Dictionary<string, ObservableValue> targetDict)
        {
            if(transaction == null)
                throw new NullReferenceException("Transaction was null");

            if (targetDict.ContainsKey(transaction.Classification))
                targetDict[transaction.Classification].Value += transaction.Value;
            else
                targetDict.Add(transaction.Classification, new ObservableValue(transaction.Value));
        }

        public ICommand OpenAuthorizationPresentation
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                    if (!rootRegistry.ViewModels.Any(x => x.GetType() == typeof(AuthorizetionViewModel)))
                    {
                        rootRegistry.ShowPresentation(new AuthorizetionViewModel());
                        StatusBar = "Готово";
                    }
                    else StatusBar = "Окно уже открыто";
                }, () => User == null);
            }
        }

        public ICommand OpenExpensesOverviewPresentation
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var rootRegistry = (Application.Current as App).DisplayRootRegistry;
                    if (!rootRegistry.ViewModels.Any(x => x.GetType() == typeof(ExpensesOverviewWndViewModel)))
                    {
                        rootRegistry.ShowPresentation(new ExpensesOverviewWndViewModel());
                        StatusBar = "Готово";
                    }
                    else StatusBar = "Окно уже открыто";
                });
            }
        }
    }
}
