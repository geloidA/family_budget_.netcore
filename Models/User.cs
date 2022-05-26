using DevExpress.Mvvm;

namespace family_budget.Models
{
    public class User : BindableBase
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
    }
}
