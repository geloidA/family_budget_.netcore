using DevExpress.Mvvm;
using family_budget.Models.DataBase;
using family_budget.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace family_budget.Models
{
    internal abstract class AddingTransactionModel : ViewModelBase
    {
        private protected MainWndViewModel mainWndViewModel;

        public ObservableCollection<FamilyMember> FamilyMembers { get; set; }
        public FamilyMember SelectedFamilyMember { get; set; }
        public string Classification { get; set; }
        public double Cost { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }

        public AddingTransactionModel()
        {
            mainWndViewModel = (Application.Current as App).MainWindowViewModel;
            FamilyMembers = new ObservableCollection<FamilyMember>(DataWorker.FamilyMembers);
        }

        public virtual ICommand AddTransaction { get; }
        public virtual bool IsCanExecute { get; }
    }
}
