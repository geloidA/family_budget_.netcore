using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels.Abstract
{
    internal abstract class AddingTransactionViewModel : ViewModelBase
    {
        private protected MainWndViewModel mainWndViewModel;

        public ObservableCollection<FamilyMember> FamilyMembers { get; set; }
        public FamilyMember SelectedFamilyMember { get; set; }
        public string Classification { get; set; }
        public double Cost { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }

        public AddingTransactionViewModel()
        {
            mainWndViewModel = (Application.Current as App).MainWindowViewModel;
            FamilyMembers = new ObservableCollection<FamilyMember>(DataWorker.FamilyMembers);
        }

        public virtual ICommand Command { get; }
        public virtual bool IsCanExecute { get; }
    }
}
