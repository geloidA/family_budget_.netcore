using System.Collections.Generic;
using System.Linq;

namespace family_budget.Models.DataBase
{
    public static class DataWorker
    {
        public static List<Expense> Expenses
        {
            get
            {
                using (var contxt = new DataContext())
                {
                    return contxt.Expenses.ToList();
                }
            }
        }
        public static List<Income> Incomes
        {
            get
            {
                using (var contxt = new DataContext())
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
        public static bool TryFindUser(string login, string password, out User user)
        {
            user = Users.Find(u => u.Login == login && u.Password == password);
            return user != null;
        }
        public static void AddUser(User user)
        {
            using (var context = new DataContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        public static void AddExpense(Expense expense)
        {
            using (var context = new DataContext())
            {
                context.Expenses.Add(expense);
                context.SaveChanges();
            }
        }
        public static void AddIncome(Income income)
        {
            using (var context = new DataContext())
            {
                context.Incomes.Add(income);
                context.SaveChanges();
            }
        }
        public static void AddFamilyMember(FamilyMember familyMember)
        {
            using (var context = new DataContext())
            {
                context.FamilyMembers.Add(familyMember);
                context.SaveChanges();
            }
        }
    }
}
