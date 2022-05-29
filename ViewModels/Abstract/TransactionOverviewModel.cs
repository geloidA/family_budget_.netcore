using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels.Abstract
{
    internal abstract class TransactionOverviewModel : ViewModelBase
    {
        private protected MainWndViewModel mainVM;

        public SeriesCollection MonthsSeries { get; set; }
        public ObservableCollection<string> Labels { get; set; }
        public Month FirstSelectedMonth { get; set; } = Month.Average;
        public Month SecondSelectedMonth { get; set; } = (Month)DateTime.Now.Month;
        public ObservableCollection<TransactionJoinFM> Transactions { get; set; }
        public TransactionJoinFM SelectedTransactionJoinFM { get; set; }       
        public ObservableValue FirstTransactCostByMonth;
        public ObservableValue SecondTransactCostByMonth;
        private protected double AverageTransactCostByMonth { get; set; }

        public TransactionOverviewModel()
        {
            mainVM = (Application.Current as App).MainWindowViewModel;
            SecondTransactCostByMonth = new ObservableValue();
            FirstTransactCostByMonth = new ObservableValue();
            Labels = new ObservableCollection<string>()
            {
                FirstSelectedMonth.ToString(),
                SecondSelectedMonth.ToString()
            };
        }

        private protected void TransactionUpdated(Transaction toUpdate, Transaction from)
        {
            var toChange = Transactions.FirstOrDefault(t => t.TransactionId == toUpdate.Id);
            if (toChange != null)
            {
                toChange.Classification = from.Classification;
                toChange.Cost = from.Cost;
                toChange.Date = from.Date;
                toChange.Description = from.Description;
                toChange.FamilyRole = DataWorker.FamilyMembers.FirstOrDefault(f => f.Id == from.FamilyMemberId)?.FamilyRole;
            }

            AverageTransactCostByMonth += from.Cost / 12 - toUpdate.Cost / 12;
            UpdateMonthSeries();
        }
        private protected void UpdateMonthSeries()
        {
            MonthsSeries.Clear();

            FirstTransactCostByMonth.Value = FirstSelectedMonth == Month.Average ? AverageTransactCostByMonth 
                : SumOfTransactionsPerMonth(FirstSelectedMonth);
            SecondTransactCostByMonth.Value = SecondSelectedMonth == Month.Average ? AverageTransactCostByMonth
                : SumOfTransactionsPerMonth(SecondSelectedMonth);

            MonthsSeries.Add(new ColumnSeries
            {
                Title = "руб.",
                Values = new ChartValues<ObservableValue> { FirstTransactCostByMonth, SecondTransactCostByMonth }
            });

            Labels[0] = FirstSelectedMonth.ToString();
            Labels[1] = SecondSelectedMonth.ToString();
        }
        private protected double SumOfTransactionsPerMonth(Month month)
        {
            return Transactions.Where(t => t.Date.Month == ((int)month))
                .Sum(t => t.Cost);
        }
        private protected void AddTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                Transactions.Add(new TransactionJoinFM
                {
                    Classification = transaction.Classification,
                    Cost = transaction.Cost,
                    Date = transaction.Date,
                    Description = transaction.Description,
                    FamilyRole = DataWorker.FamilyMembers.FirstOrDefault(f => f.Id == transaction.FamilyMemberId)?.FamilyRole,
                    TransactionId = transaction.Id
                });
            }
        }
        private protected void RemoveTransaction(Transaction transaction)
        {
            if (transaction != null)
                Transactions.Remove(Transactions.FirstOrDefault(t => t.TransactionId == transaction.Id));
        }        

        public virtual ICommand OpenAddingTransactionPresentation { get; }
        public virtual ICommand DeleteTransaction { get; }
        public virtual ICommand ChangeTransaction { get; }
        public virtual ICommand FirstMonthSelectionChanged =>
            new DelegateCommand(() =>
            {
                MonthSelectionChanged(FirstSelectedMonth, FirstTransactCostByMonth, 0);
            });
        public virtual ICommand SecondMonthSelectionChanged =>
            new DelegateCommand(() =>
            {
                MonthSelectionChanged(SecondSelectedMonth, SecondTransactCostByMonth, 1);
            });
        private void MonthSelectionChanged(Month month, ObservableValue value, int labelIndex)
        {
            Labels[labelIndex] = Enum.GetName(month);

            if (month == Month.Average)
                value.Value = AverageTransactCostByMonth;
            else
                value.Value = SumOfTransactionsPerMonth(month);
        }
    }
}
