using ValidPassword.Domain.ValueObjects;

namespace ValidPassword.Domain.Interfaces.Validation
{
    public interface ISelfValidator
    {
        ValidationResult IsValid();
    }
}
