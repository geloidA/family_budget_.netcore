using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace family_budget.Models.DataBase
{
    public static class DataWorker
    {
        static DataWorker()
        {
            using var db = new DataContext();
            Expenses = new ObservableCollection<Expense>(db.Expenses);
            Incomes = new ObservableCollection<Income>(db.Incomes);
            FamilyMembers = new ObservableCollection<FamilyMember>(db.FamilyMembers);
            Users = new ObservableCollection<User>(db.Users);
        }

        public readonly static ObservableCollection<Expense> Expenses;
        public readonly static ObservableCollection<Income> Incomes;
        public readonly static ObservableCollection<FamilyMember> FamilyMembers;
        public readonly static ObservableCollection<User> Users;

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
            using var context = new DataContext();
            context.Users.Add(user);
            context.SaveChanges();
        }
        public static void AddExpense(Expense expense)
        {
            using (var context = new DataContext())
            {
                context.Expenses.Add(expense);
                context.SaveChanges();
            }
            Expenses.Add(expense);
        }
        public static void AddIncome(Income income)
        {
            using (var context = new DataContext())
            {
                context.Incomes.Add(income);
                context.SaveChanges();
            }
            Incomes.Add(income);
        }
        public static void AddFamilyMember(FamilyMember familyMember)
        {
            FamilyMembers.Add(familyMember);
            using var context = new DataContext();
            context.FamilyMembers.Add(familyMember);
            context.SaveChanges();
        }
        public static void RemoveUser(User user)
        {
            Users.Remove(user);
            using var context = new DataContext();
            context.Users.Remove(user);
            context.SaveChanges();
        }
        public static void RemoveExpense(Expense expense)
        {
            Expenses.Remove(expense);
            using var context = new DataContext();
            context.Expenses.Remove(expense);
            context.SaveChanges();
        }
        public static void RemoveIncome(Income income)
        {
            Incomes.Remove(income);
            using var context = new DataContext();
            context.Incomes.Remove(income);
            context.SaveChanges();
        }
        public static void RemoveFamilyMember(FamilyMember familyMember)
        {
            FamilyMembers.Remove(familyMember);
            using var context = new DataContext();
            context.FamilyMembers.Remove(familyMember);
            context.SaveChanges();
        }
        public static void UpdateUser(int toUpdateId, User from)
        {
            using var context = new DataContext();
            var toUpdate = context.Users.Single(u => u.Id == toUpdateId);
            UpdateUserData(toUpdate, from);
            var progUser = Users.Single(u => u.Id == toUpdateId);
            UpdateUserData(progUser, from);
            context.SaveChanges();
        }
        private static void UpdateUserData(User toUpdate, User from)
        {
            toUpdate.FullName = from.FullName;
            toUpdate.Password = from.Password;
            toUpdate.Login = from.Login;
        }
        public static void UpdateIncome(int toUpdateId, Income from)
        {
            using var context = new DataContext();
            var toUpdate = context.Incomes.Single(i => i.Id == toUpdateId);

            IncomeUpdated?.Invoke(toUpdate, from);

            var income = Incomes.Single(i => i.Id == toUpdateId);
            UpdateTransaction(income, from);

            UpdateTransaction(toUpdate, from);
            context.SaveChanges();
        }
        public static void UpdateExpense(int toUpdateId, Expense from)
        {
            using var context = new DataContext();
            var toUpdate = context.Expenses.Single(e => e.Id == toUpdateId);

            ExpenseUpdated?.Invoke(toUpdate, from);

            var expense = Expenses.Single(e => e.Id == toUpdateId);
            UpdateTransaction(expense, from);

            UpdateTransaction(toUpdate, from);
            context.SaveChanges();
        }
        private static void UpdateTransaction(Transaction toUpdate, Transaction from)
        {
            toUpdate.Classification = from.Classification;
            toUpdate.FamilyMemberId = from.FamilyMemberId;
            toUpdate.Date = from.Date;
            toUpdate.Description = from.Description;
            toUpdate.Cost = from.Cost;
        }
        public static void UpdateFamilyMember(int toUpdateId, FamilyMember from)
        {
            var programMember = FamilyMembers.Single(x => x.Id == toUpdateId);
            programMember.FullName = from.FullName;
            programMember.FamilyRole = from.FamilyRole;

            using var contxt = new DataContext();
            var dbMember = contxt.FamilyMembers.Single(x => x.Id == toUpdateId);
            dbMember.FullName = from.FullName;
            dbMember.FamilyRole = from.FamilyRole;
            contxt.SaveChanges();
        }
    }
}