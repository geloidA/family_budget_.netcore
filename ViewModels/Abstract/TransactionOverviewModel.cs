using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using LiveCharts;
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
        public IEnumerable<Month> Months { get; set; }
        public Month FirstSelectedMonth { get; set; } = (Month)DateTime.Now.Month;
        public Month SecondSelectedMonth { get; set; } = Models.Month.Average;
        public ObservableCollection<TransactionJoinFM> Transactions { get; set; }
        public TransactionJoinFM SelectedTransactionJoinFM { get; set; }
        public double AverageTransactCostByMonth { get; set; }
        public double SumOfTransactionsPerMonth(Month month)
        {
            return Transactions.Where(t => t.Date.Month == ((int)month))
                .Sum(t => t.Cost);
        }

        public TransactionOverviewModel()
        {
            mainVM = (Application.Current as App).MainWindowViewModel;
            Months = Enum.GetValues<Month>();
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

            AverageTransactCostByMonth -= toUpdate.Cost / 12;
            AverageTransactCostByMonth += from.Cost / 12;
        }

        public virtual ICommand OpenAddingTransactionPresentation { get; }
        public virtual ICommand DeleteTransaction { get; }
        public virtual ICommand ChangeTransaction { get; }
        public virtual ICommand FirstMonthSelectionChanged { get; }
        public virtual ICommand SecondMonthSelectionChanged { get; }
    }
}
