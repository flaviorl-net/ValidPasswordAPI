using ValidPassword.Domain.Interfaces.Specification;

namespace ValidPassword.Domain.Validation.Password
{
    public class NaoPossuirCaracteresRepetidos : ISpecification<string>
    {
        public bool IsSatisfiedBy(string password)
        {
            for (int i = 0; i < password.Length; i++)
            {
                string letra = password.Substring(i, 1);

                for (int j = (i + 1); j < password.Length; j++)
                {
                    string proxLetra = password.Substring(j, 1);

                    if (letra == proxLetra)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
