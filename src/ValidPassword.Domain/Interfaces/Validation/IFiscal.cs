using ValidPassword.Domain.ValueObjects;

namespace ValidPassword.Domain.Interfaces.Validation
{
    public interface IFiscal<in TEntity>
    {
        ValidationResult Validate(TEntity entity);
    }
}
