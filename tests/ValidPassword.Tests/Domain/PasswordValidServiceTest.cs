using FluentAssertions;
using ValidPassword.Domain.Service;
using Xunit;

namespace ValidPassword.Tests.Domain
{
    public class PasswordValidServiceTest
    {
        private readonly PasswordValidService _service;

        public PasswordValidServiceTest()
        {
            _service = new PasswordValidService();
        }

        [Fact]
        [Trait("Validar Senha", "Senha Válida")]
        public void Senha_Valida()
        {
            //Arrange
            string password = "asdfghjk1Q@";

            //Act
            bool result = _service.PasswordIsValid(password);

            //Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("asdfghjkq7@")] //Falta Letra Maiuscula
        [InlineData("ASDFGHJKQ7@")] //Falta Letra Minuscula
        [InlineData("asdfghjkqp7")] //Falta Caractere Especial
        [InlineData("A@sdhjkqp")] //Falta Número
        [InlineData("aA7sdhjk@qpa")] //Possui Letra Repetida
        [InlineData("aB1@cder")] //Possui Menos de 9 Caracteres
        public void Senha_Invalida(string password)
        {
            //Act
            bool result = _service.PasswordIsValid(password);

            //Assert
            result.Should().BeFalse();
        }

    }
}
