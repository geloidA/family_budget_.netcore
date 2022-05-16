using System.Collections.Generic;
using System.Linq;

namespace family_budget.Models.DataBase
{
    public static class DataWorker
    {
        public static IReadOnlyList<Expense> Expenses
        {
            get
            {
                using (var contxt = new DataContext())
                {
                    return contxt.Expenses.ToList();
                }
            }
        }
        public static IReadOnlyList<Income> Incomes
        {
            get
            {
                using (var contxt = new DataContext())
                {
                    return contxt.Incomes.ToList();
                }
            }
        }
        public static IReadOnlyList<FamilyMember> FamilyMembers
        {
            get
            {
                using (var contxt = new DataContext())
                {
                    return contxt.FamilyMembers.ToList();
                }
            }
        }
        public static IReadOnlyList<User> Users
        {
            get
            {
                using (var contxt = new DataContext())
                {
                    return contxt.Users.ToList();
                }
            }
        }
        public static IReadOnlyList<TransactionJoinFM> ExpensesJoinFamilyMembers
        {
            get
            {
                using (var contxt = new DataContext())
                {
                    return contxt.Expenses.Join(
                        contxt.FamilyMembers,
                        e => e.FamilyMemberId,
                        f => f.Id,
                        (e, f) => new TransactionJoinFM
                        {
                            TransactionId = e.Id,
                            Classification = e.Classification,
                            Cost = e.Cost,
                            Date = e.Date,
                            Description = e.Description,
                            FamilyRole = f.FamilyRole
                        })
                        .ToList();
                }
            }
        }

        public static IEnumerable<Transaction> Transactions
        {
            get
            {
                return Expenses.Select(e => (Transaction)e)
                    .Concat(Incomes)
                    .OrderByDescending(t => t.Date);
            }
        }

        public static bool TryFindUser(string login, string password, out User user)
        {
            user = Users.FirstOrDefault(u => u.Login == login && u.Password == password);
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
        public static void RemoveUser(User user)
        {
            using(var context = new DataContext())
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        public static void RemoveExpense(Expense expense)
        {
            using( var context = new DataContext())
            {
                context.Expenses.Remove(expense);
                context.SaveChanges();
            }
        }

        public static void RemoveIncome(Income income)
        {
            using(var context = new DataContext())
            {
                context.Incomes.Remove(income);
                context.SaveChanges();
            }
        }

        public static void RemoveFamilyMember(FamilyMember familyMember)
        {
            using (var context = new DataContext())
            {
                context.FamilyMembers.Remove(familyMember);
                context.SaveChanges();
            }
        }

        public static void UpdateUser(User user, User newUser)
        {
            using(var context = new DataContext())
            {
                user.FullName = newUser.FullName;
                user.Password = newUser.Password;
                user.Login = newUser.Login;
                context.SaveChanges();
            }
        }
    }
}
