using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace family_budget.Views
{
    /// <summary>
    /// Interaction logic for AddingExpensesWnd.xaml
    /// </summary>
    public partial class AddingExpensesWnd : Window
    {
        public AddingExpensesWnd()
        {
            InitializeComponent();
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        private void OnlyNumericTextPasting(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }
    }
}
