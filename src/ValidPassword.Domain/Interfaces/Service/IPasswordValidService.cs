namespace ValidPassword.Domain.Interfaces.Service
{
    public interface IPasswordValidService
    {
        bool PasswordIsValid(string password);
    }
}
