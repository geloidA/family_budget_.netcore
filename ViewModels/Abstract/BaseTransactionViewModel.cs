﻿using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace family_budget.ViewModels.Abstract
{
    internal abstract class BaseTransactionViewModel : ViewModelBase
    {
        public ObservableCollection<FamilyMember> FamilyMembers { get; set; }
        public FamilyMember SelectedFamilyMember { get; set; }
        public string Classification { get; set; }
        public double Cost { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }

        public BaseTransactionViewModel()
        {
            FamilyMembers = DataWorker.FamilyMembers;
        }

        public virtual ICommand Command { get; }
        public virtual bool IsCanExecute => Cost > 0 && SelectedFamilyMember != null
            && !string.IsNullOrEmpty(Classification);
    }
}
