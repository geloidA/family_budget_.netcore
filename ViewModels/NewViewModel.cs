using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using System.Windows.Input;
using System.Collections.ObjectModel;
using family_budget.Models;

namespace family_budget.ViewModels
{
    class NewViewModel : ViewModelBase
    {
        public ObservableCollection<FamilyMember> FamilyMembers { get; set; }

        public NewViewModel()
        {
            FamilyMembers = new ObservableCollection<FamilyMember>(GetFamilyMembers());
        }

        public List<FamilyMember> GetFamilyMembers()
        {
            using(var context = new DataContext())
            {
                return context.FamilyMembers.ToList();
            }
        }
    }
}
