using System.Text.RegularExpressions;
using ValidPassword.Domain.Interfaces.Specification;
using ValidPassword.Domain.ValueObjects;

namespace ValidPassword.Domain.Validation.Password
{
    public class UmaLetraMaiuscula : ISpecification<string>
    {
        public bool IsSatisfiedBy(string password)
        {
            if (Regex.IsMatch(password, RegularExpression.GetPattern(Enums.RegularExp.UmaLetraMaiuscula)))
            {
                return true;
            }

            return false;
        }
    }
}
