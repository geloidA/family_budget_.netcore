using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.Services;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class AddingFamilyMemberViewModel : ViewModelBase
    {
        public string FullName { get; set; }
        public FamilyRole Role { get; set; } = FamilyRole.Husband;

        public ICommand AddFamilyMember
            => new DelegateCommand(() =>
            {
                var familyMember = new FamilyMember { FamilyRole = Role.GetDescription(), FullName = FullName };
                DataWorker.AddFamilyMember(familyMember);
            }, 
            () => !string.IsNullOrEmpty(FullName));
    }
}
