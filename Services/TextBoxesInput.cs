using System.Text.RegularExpressions;
using System.Windows.Input;

namespace family_budget.Services
{
    internal class TextBoxesInput
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        public void OnlyNumericTextPasting(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }
    }
}
