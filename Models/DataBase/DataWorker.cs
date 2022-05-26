using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace family_budget.Models.DataBase
{
    public static class DataWorker
    {
        static DataWorker()
        {
            using (var db = new DataContext())
            {
                Expenses = new ObservableCollection<Expense>(db.Expenses);
                Incomes = new ObservableCollection<Income>(db.Incomes);
                FamilyMembers = new ObservableCollection<FamilyMember>(db.FamilyMembers);
                Users = new ObservableCollection<User>(db.Users);
            }
        }

        public static ObservableCollection<Expense> Expenses;
        public static ObservableCollection<Income> Incomes;
        public static ObservableCollection<FamilyMember> FamilyMembers;
        public static ObservableCollection<User> Users;

        /// <summary>
        /// first argument - value that will be updated, second - data from which info is given 
        /// </summary>
        public static event Action<Expense, Expense> ExpenseUpdated;
        /// <summary>
        /// first argument - value that will be updated, second - data from which info is given 
        /// </summary>
        public static event Action<Income, Income> IncomeUpdated;

        public static bool TryFindUser(string login, string password, out User user)
        {
            user = Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            return user != null;
        }
        public static void AddUser(User user)
        {
            Users.Add(user);
            using (var context = new DataContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        public static void AddExpense(Expense expense)
        {
            Expenses.Add(expense);

            using (var context = new DataContext())
            {
                context.Expenses.Add(expense);
                context.SaveChanges();
            }
        }
        public static void AddIncome(Income income)
        {
            Incomes.Add(income);
            using (var context = new DataContext())
            {
                context.Incomes.Add(income);
                context.SaveChanges();
            }
        }
        public static void AddFamilyMember(FamilyMember familyMember)
        {
            FamilyMembers.Add(familyMember);
            using (var context = new DataContext())
            {
                context.FamilyMembers.Add(familyMember);
                context.SaveChanges();
            }
        }
        public static void RemoveUser(User user)
        {
            Users.Remove(user);
            using (var context = new DataContext())
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
        public static void RemoveExpense(Expense expense)
        {
            Expenses.Remove(expense);
            using (var context = new DataContext())
            {
                context.Expenses.Remove(expense);
                context.SaveChanges();
            }
        }
        public static void RemoveIncome(Income income)
        {
            Incomes.Remove(income);
            using (var context = new DataContext())
            {
                context.Incomes.Remove(income);
                context.SaveChanges();
            }
        }
        public static void RemoveFamilyMember(FamilyMember familyMember)
        {
            FamilyMembers.Remove(familyMember);
            using (var context = new DataContext())
            {
                context.FamilyMembers.Remove(familyMember);
                context.SaveChanges();
            }
        }
        public static void UpdateUser(User toUpdate, User from)
        {
            using (var context = new DataContext())
            {
                toUpdate.FullName = from.FullName;
                toUpdate.Password = from.Password;
                toUpdate.Login = from.Login;
                context.SaveChanges();
            }
        }
        public static void UpdateIncome(int toUpdateId, Income from)
        {
            using (var context = new DataContext())
            {
                var toUpdate = context.Incomes.SingleOrDefault(e => e.Id == toUpdateId);

                IncomeUpdated?.Invoke(toUpdate, from);

                UpdateTransaction(toUpdate, from);
                context.SaveChanges();
            }
        }
        public static void UpdateExpense(int toUpdateId, Expense from)
        {
            using (var context = new DataContext())
            {
                var toUpdate = context.Expenses.SingleOrDefault(e => e.Id == toUpdateId);

                ExpenseUpdated?.Invoke(toUpdate, from);

                UpdateTransaction(toUpdate, from);
                context.SaveChanges();
            }
        }
        private static void UpdateTransaction(Transaction toUpdate, Transaction from)
        {
            toUpdate.Classification = from.Classification;
            toUpdate.FamilyMemberId = from.FamilyMemberId;
            toUpdate.Date = from.Date;
            toUpdate.Description = from.Description;
            toUpdate.Cost = from.Cost;
        }
        public static void UpdateFamilyMember(FamilyMember toUpdate, FamilyMember from)
        {
            using (var contxt = new DataContext())
            {
                toUpdate.FullName = from.FullName;
                toUpdate.FamilyRole = from.FamilyRole;
                contxt.SaveChanges();
            }
        }
    }
}