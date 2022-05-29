using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace family_budget.Services
{
    internal static class ObjectExtentions
    {
        public static string GetDescription(this object value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
