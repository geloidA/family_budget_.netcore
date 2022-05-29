﻿using DevExpress.Mvvm;
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

        public ObservableCollection<MemberCosts> FamilyMembers { get; set; }            
        public MemberCosts SelectedFamilyMember { get; set; }
        public ObservableCollection<Expense> MonthExpenses { get; set; }
        public double MonthExpensesSum { get; set; }

        private Dictionary<string, ObservableValue> AmountsGroupedExpenses;
        private Dictionary<string, ObservableValue> AmountsGroupedIncomes;

        public MainWndViewModel()
        {
            Initialize();
            FamilyMembers = new ObservableCollection<MemberCosts>(FamilyMembersCosts);
            DataWorker.Expenses.CollectionChanged += Expenses_CollectionChanged;
            DataWorker.Incomes.CollectionChanged += Incomes_CollectionChanged;
            DataWorker.ExpenseUpdated += DataWorker_ExpenseUpdated;
            DataWorker.IncomeUpdated += DataWorker_IncomeUpdated;
            DataWorker.FamilyMembers.CollectionChanged += FamilyMembers_CollectionChanged;
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
        private IEnumerable<MemberCosts> FamilyMembersCosts
        {
            get
            {
                var incomes = DataWorker.Incomes.GroupBy(income => income.FamilyMemberId);
                var expenses = DataWorker.Expenses.GroupBy(expense => expense.FamilyMemberId);
                foreach (var familyMember in DataWorker.FamilyMembers)
                {
                    var incomeCost = incomes.FirstOrDefault(i => i.Key == familyMember.Id)
                        ?.Sum(i => i.Cost);
                    var expenseCost = expenses.FirstOrDefault(i => i.Key == familyMember.Id)
                        ?.Sum(e => e.Cost);
                    yield return new MemberCosts
                    {
                        Member = familyMember,
                        Incomes = incomeCost ?? 0,
                        Expenses = expenseCost ?? 0
                    };
                }
            }
        }
        private void Expenses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newExpense = e.NewItems.Cast<Expense>().FirstOrDefault();
                    AddTransaction(newExpense, AmountsGroupedExpenses,
                            Expenses);
                    AddToMonthExpenses(newExpense);
                    FamilyMembers.FirstOrDefault(x => x.Member.Id == newExpense.FamilyMemberId).Expenses += newExpense.Cost;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var oldExpense = e.OldItems.Cast<Expense>().FirstOrDefault();
                    RemoveTransaction(oldExpense, AmountsGroupedExpenses,
                        Expenses);
                    RemoveFromMonthExpenses(oldExpense);
                    FamilyMembers.FirstOrDefault(x => x.Member.Id == oldExpense.FamilyMemberId).Expenses -= oldExpense.Cost;
                    break;
            }
        }
        private void Incomes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var income = e.NewItems.Cast<Income>().FirstOrDefault();
                    AddTransaction(income, AmountsGroupedIncomes,
                        Incomes);
                    FamilyMembers.FirstOrDefault(x => x.Member.Id == income.FamilyMemberId).Incomes += income.Cost;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var oldIncome = e.OldItems.Cast<Income>().FirstOrDefault();
                    RemoveTransaction(oldIncome, AmountsGroupedIncomes,
                        Incomes);
                    FamilyMembers.FirstOrDefault(x => x.Member.Id == oldIncome.FamilyMemberId).Incomes -= oldIncome.Cost;
                    break;
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
        private void FamilyMembers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddToFamilyMembers(e.NewItems.Cast<FamilyMember>().FirstOrDefault());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveFromFamilyMembers(e.OldItems.Cast<FamilyMember>().FirstOrDefault());
                    break;
            }
        }
        private void AddToFamilyMembers(FamilyMember member)
        {
            FamilyMembers.Add(new MemberCosts { Expenses = 0, Incomes = 0, Member = member });
        }
        private void RemoveFromFamilyMembers(FamilyMember familyMember)
        {
            FamilyMembers.Remove(FamilyMembers.FirstOrDefault(x => x.Member.Id == familyMember.Id));
        }
        private void DataWorker_ExpenseUpdated(Expense toUpdate, Expense from)
        {
            UpdateTransaction(toUpdate, from, AmountsGroupedExpenses, Expenses);
            var first = MonthExpenses.FirstOrDefault();
            if (first != null)
            {
                if (toUpdate.Date.Month == first.Date.Month && toUpdate.Date.Year == first.Date.Year)
                    UpdateMonthExpenses(toUpdate, from);
            }
            UpdateFamilyMember(toUpdate, from, "Expenses");
        }

        private void UpdateTransaction(Transaction toUpdate, Transaction from,
            Dictionary<string, ObservableValue> model,
            SeriesCollection series)
        {
            if (toUpdate.Classification == from.Classification)
            {
                model[toUpdate.Classification].Value += from.Cost - toUpdate.Cost;
            }
            else
            {
                RemoveTransaction(toUpdate, model, series);
                AddTransaction(from, model, series);
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
        private void UpdateMonthExpenses(Expense toUpdate, Expense from)
        {
            if (from.Date.Month != toUpdate.Date.Month)
                MonthExpenses.Remove(toUpdate);
            else
            {
                var toChange = MonthExpenses.SingleOrDefault(e => e.Id == toUpdate.Id);
                MonthExpensesSum += from.Cost - toUpdate.Cost;
                if(toChange != null)
                {
                    toChange.Date = from.Date;
                    toChange.Classification = from.Classification;
                    toChange.Cost = from.Cost;
                    toChange.Description = from.Description;
                    toChange.FamilyMemberId = from.FamilyMemberId;
                }                
            }
        }
        private void UpdateFamilyMember(Transaction toUpdate, Transaction from, string propertyName)
        {
            if (toUpdate.FamilyMemberId == from.FamilyMemberId)
            {
                var dif = from.Cost - toUpdate.Cost;
                if (dif != 0)
                {
                    var target = FamilyMembers.FirstOrDefault(x => x.Member.Id == from.FamilyMemberId);
                    var targetCost = target.GetType().GetProperty(propertyName).GetValue(target);
                    target.GetType().GetProperty(propertyName).SetValue(target, (double)targetCost + dif, null);
                }
            }
            else
            {
                var oldMember = FamilyMembers.FirstOrDefault(x => x.Member.Id == toUpdate.FamilyMemberId);
                var oldMemberCost = oldMember.GetType().GetProperty(propertyName).GetValue(oldMember);
                oldMember.GetType().GetProperty(propertyName).SetValue(oldMember, (double)oldMemberCost - toUpdate.Cost, null);
                var newMember = FamilyMembers.FirstOrDefault(x => x.Member.Id == from.FamilyMemberId);
                var newMemberCost = newMember.GetType().GetProperty(propertyName).GetValue(newMember);
                newMember.GetType().GetProperty(propertyName).SetValue(newMember, (double)newMemberCost + from.Cost, null);
            }
        }
        private void AddToMonthExpenses(Expense newExpense)
        {
            if (newExpense.Date.Month == DateTime.Now.Month)
            {
                MonthExpenses.Add(newExpense);
                MonthExpensesSum += newExpense.Cost;
            }
        }
        private void RemoveFromMonthExpenses(Expense oldExpense)
        {
            if (oldExpense.Date.Month == DateTime.Now.Month)
            {
                MonthExpenses.Remove(oldExpense);
                MonthExpensesSum -= oldExpense.Cost;
            }
        }
        private void DataWorker_IncomeUpdated(Income toUpdate, Income from)
        {
            UpdateTransaction(toUpdate, from, AmountsGroupedIncomes, Incomes);
            UpdateFamilyMember(toUpdate, from, "Incomes");
        }
        

        public ICommand OpenAuthorizationPresentation
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    OpenPresentaion(new AuthorizetionViewModel());
                }, () => User == null);
            }
        }
        public ICommand OpenAddFamilyMemberPresentation
            => new DelegateCommand(() =>
            {
                OpenPresentaion(new AddingFamilyMemberViewModel());
            }, () => User != null);
        public ICommand OpenExpensesOverviewPresentation
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    OpenPresentaion(new ExpensesOverviewWndViewModel());
                }, () => User != null);
            }
        }
        public ICommand OpenIncomesOverviewPresentation
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    OpenPresentaion(new IncomesOverviewWndViewModel());
                }, () => User != null);
            }
        }
        private void OpenPresentaion(object viewModel)
        {
            var rootRegistry = (Application.Current as App).DisplayRootRegistry;
            if (!rootRegistry.ViewModels.Any(x => x.GetType() == viewModel.GetType()))
            {
                rootRegistry.ShowPresentation(viewModel);
                StatusBar = "Готово";
            }
            else StatusBar = "Окно уже открыто";
        }
        public ICommand LogOut => new DelegateCommand(() =>
        {
            User = null;
            (Application.Current as App).MainWindowViewModel.StatusBar = "Чтобы работать с системой, нужно авторизоваться";
        });
        public ICommand RemoveFamilyMember
            => new DelegateCommand(() =>
            {
                var toRemove = SelectedFamilyMember.Member;
                if (toRemove != null)
                    DataWorker.RemoveFamilyMember(toRemove);
            }, 
            () => SelectedFamilyMember != null && User != null);
    }
}