using System.ComponentModel;

namespace family_budget.Models
{
    enum FamilyRole
    {
        [Description("Муж")]
        Husband,
        [Description("Жена")]
        Wife,
        [Description("Ребёнок")]
        Children,
        [Description("Общее")]
        All
    }
}
