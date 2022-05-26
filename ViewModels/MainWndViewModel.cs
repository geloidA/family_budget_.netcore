using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.ViewModels.Abstract;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    public class MainWndViewModel : ViewModelBase
    {
        public User User { get; set; }
        public string StatusBar { get; set; } = "Чтобы работать с системой, нужно авторизоваться";
        public SeriesCollection Expenses { get; set; }
        public SeriesCollection Incomes { get; set; }

        public ObservableCollection<Expense> MonthExpenses { get; set; }
        public double MonthExpensesSum { get; set; }

        private Dictionary<string, ObservableValue> AmountsGroupedExpenses;
        private Dictionary<string, ObservableValue> AmountsGroupedIncomes;

        public MainWndViewModel()
        {
            Initialize();
            DataWorker.Expenses.CollectionChanged += Expenses_CollectionChanged;
            DataWorker.Incomes.CollectionChanged += Incomes_CollectionChanged;
            DataWorker.ExpenseUpdated += DataWorker_ExpenseUpdated;
            DataWorker.IncomeUpdated += DataWorker_IncomeUpdated;
        }

        private void Initialize()
        {
            AmountsGroupedExpenses = new Dictionary<string, ObservableValue>(DataWorker.Expenses.GroupBy(e => e.Classification)
                .Select(g => new KeyValuePair<string, ObservableValue>(g.Key, new ObservableValue(g.Sum(e => e.Cost)))));

            AmountsGroupedIncomes = new Dictionary<string, ObservableValue>(DataWorker.Incomes.GroupBy(e => e.Classification)
                .Select(g => new KeyValuePair<string, ObservableValue>(g.Key, new ObservableValue(g.Sum(e => e.Cost)))));

            Expenses = new SeriesCollection();
            Expenses.AddRange(AmountsGroupedExpenses.Select(pair =>
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

            MonthExpenses = new ObservableCollection<Expense>(DataWorker.Expenses.Where(e =>
            e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year)
                .OrderByDescending(e => e.Date));
            MonthExpensesSum = MonthExpenses.Sum(e => e.Cost);
        }
        private void Expenses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddTransaction(e.NewItems.Cast<Expense>().FirstOrDefault(), AmountsGroupedExpenses,
                        Expenses);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveTransaction(e.OldItems.Cast<Expense>().FirstOrDefault(), AmountsGroupedExpenses,
                        Expenses);
                    break;
            }
        }
        private void Incomes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddTransaction(e.NewItems.Cast<Income>().FirstOrDefault(), AmountsGroupedIncomes,
                        Incomes);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveTransaction(e.OldItems.Cast<Income>().FirstOrDefault(), AmountsGroupedIncomes,
                        Incomes);
                    break;
            }
        }

        private void AddTransaction(Transaction newTransact,
            Dictionary<string, ObservableValue> model,
            SeriesCollection series)
        {
            var isExisted = AddToModel(newTransact, model);

            if (!isExisted)
            {
                series.Add(new PieSeries
                {
                    Title = newTransact.Classification,
                    Values = new ChartValues<ObservableValue> { model[newTransact.Classification] }
                });
            }
        }
        private bool AddToModel(Transaction transaction, Dictionary<string, ObservableValue> model)
        {
            if (model.ContainsKey(transaction.Classification))
            {
                model[transaction.Classification].Value += transaction.Cost;
                return true;
            }
            else
            {
                model.Add(transaction.Classification, new ObservableValue(transaction.Cost));
                return false;
            }
        }

        private void RemoveTransaction(Transaction transact,
            Dictionary<string, ObservableValue> model,
            SeriesCollection series)
        {
            if (transact != null)
            {
                var isGroupeOver = RemoveFromModel(transact, model);

                if (isGroupeOver)
                {
                    var group = series.FirstOrDefault(g => g.Title == transact.Classification);
                    if (group != null)
                        series.Remove(group);
                }
            }
        }
        private bool RemoveFromModel(Transaction transact, Dictionary<string, ObservableValue> model)
        {
            model[transact.Classification].Value -= transact.Cost;
            if (model[transact.Classification].Value == 0)
            {
                model.Remove(transact.Classification);
                return true;
            }
            return false;
        }

        private void DataWorker_ExpenseUpdated(Expense toUpdate, Expense from)
        {
            UpdateTransaction(toUpdate, from, AmountsGroupedExpenses, Expenses);
            var first = MonthExpenses.FirstOrDefault();
            if (first == null)
                return;
            else if (toUpdate.Date.Month == first.Date.Month && toUpdate.Date.Year == first.Date.Year)
                UpdateMonthTransactions(toUpdate, from);
        }

        private void UpdateMonthTransactions(Expense toUpdate, Expense from)
        {
            if (from.Date.Month != toUpdate.Date.Month)
                MonthExpenses.Remove(toUpdate);
        }

        private void DataWorker_IncomeUpdated(Income toUpdate, Income from)
        {
            UpdateTransaction(toUpdate, from, AmountsGroupedIncomes, Incomes);
        }
        private void UpdateTransaction(Transaction toUpdate, Transaction from,
            Dictionary<string, ObservableValue> model,
            SeriesCollection series)
        {
            if (toUpdate.Classification == from.Classification)
            {
                model[toUpdate.Classification].Value -= toUpdate.Cost;
                model[toUpdate.Classification].Value += from.Cost;
            }
            else
            {
                RemoveTransaction(toUpdate, model, series);
                AddTransaction(from, model, series);
            }
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
                    OpenTransactionOverviewPresentation(new ExpensesOverviewWndViewModel());
                }, () => User != null);
            }
        }
        public ICommand OpenIncomesOverviewPresentation
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    OpenTransactionOverviewPresentation(new IncomesOverviewWndViewModel());
                }, () => User != null);
            }
        }
        public ICommand LogOut => new DelegateCommand(() => User = null);

        private void OpenTransactionOverviewPresentation(TransactionOverviewModel transactionOverviewModel)
        {
            var rootRegistry = (Application.Current as App).DisplayRootRegistry;
            if (!rootRegistry.ViewModels.Any(x => x.GetType() == transactionOverviewModel.GetType()))
            {
                rootRegistry.ShowPresentation(transactionOverviewModel);
                StatusBar = "Готово";
            }
            else StatusBar = "Окно уже открыто";
        }
    }
}