using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace family_budget.Services
{
    internal class PatternChecker
    {
        private Dictionary<string, string> patternsWithMessage;
        /// <summary>
        ///  Patterns and informations to be provided in case of non-compliance
        /// </summary>
        public PatternChecker(Dictionary<string, string> patternsWithMessage)
        {
            this.patternsWithMessage = patternsWithMessage;
        }

        /// <summary>
        /// Check line with patterns
        /// </summary>
        /// <param name="line"></param>
        /// <param name="message">Contains all information about non-compliance</param>
        /// <returns>True if string comply with all pattertn, otherwise false</returns>
        public bool Check(string line, out string message)
        {
            var builder = new StringBuilder();
            foreach (var pattern in patternsWithMessage.Keys)
            {
                if (!Regex.IsMatch(line, pattern))
                    builder.Append(patternsWithMessage[pattern] + " ");
            }
            message = builder.ToString();

            return message.Length == 0;
        }
    }
}
