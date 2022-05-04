using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace family_budget.Views
{
    /// <summary>
    /// Interaction logic for AddingIncomesWnd.xaml
    /// </summary>
    public partial class AddingIncomesWnd : Window
    {
        public AddingIncomesWnd()
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
