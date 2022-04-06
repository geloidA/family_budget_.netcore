using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family_budget.Models
{
    class FamilyMember
    {
        public int Id { get; set; }
        public int FullName { get; set; }
        public FamilyRole Role { get; set; }
    }
}
