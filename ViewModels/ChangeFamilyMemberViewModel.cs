using DevExpress.Mvvm;
using family_budget.Models;
using family_budget.Models.DataBase;
using family_budget.Services;
using System.Windows.Input;

namespace family_budget.ViewModels
{
    internal class ChangeFamilyMemberViewModel
    {
        public string FullName { get; set; }
        public FamilyRole Role { get; set; }
        public FamilyMember ToChange;

        public ICommand Change =>
            new DelegateCommand(() =>
            {
                var updatedMember = new FamilyMember
                {
                    FullName = this.FullName,
                    FamilyRole = Role.GetDescription()
                };
                DataWorker.UpdateFamilyMember(ToChange.Id, updatedMember);
            });
    }
}
