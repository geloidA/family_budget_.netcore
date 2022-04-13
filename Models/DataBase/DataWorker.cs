using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family_budget.Models.DataBase
{
    public static class DataWorker
    {
        public static List<Expense> Expenses
        {
            get
            {
                using(var contxt = new DataContext())
                {
                    return contxt.Expenses.ToList();
                }
            }
        }
        public static List<Income> Incomes
        {
            get
            {
                using(var contxt = new DataContext())
                {
                    return contxt.Incomes.ToList();
                }
            }
        }
        public static List<FamilyMember> FamilyMembers
        {
            get
            {
                using (var contxt = new DataContext())
                {
                    return contxt.FamilyMembers.ToList();
                }
            }
        }
        public static List<User> Users
        {
            get
            {
                using (var contxt = new DataContext())
                {
                    return contxt.Users.ToList();
                }
            }
        }
    }
}
