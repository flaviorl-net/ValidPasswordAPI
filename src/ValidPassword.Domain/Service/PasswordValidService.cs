using ValidPassword.Domain.Interfaces.Service;

namespace ValidPassword.Domain.Service
{
    public class PasswordValidService : IPasswordValidService
    {
        public bool PasswordIsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return new Validation.PasswordValidation()
                .Validate(password)
                .IsValid;
        }
    }
}
