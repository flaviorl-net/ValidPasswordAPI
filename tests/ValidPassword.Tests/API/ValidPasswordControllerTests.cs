using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ValidPassword.API;
using ValidPassword.API.ViewModel;
using Xunit;
using Moq;
using ValidPassword.Domain.Interfaces.Service;
using System;
using Microsoft.Extensions.Logging;
using ValidPassword.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ValidPassword.Tests.API
{
    public class ValidPasswordControllerTests : IClassFixture<CustomWebApplicationFactory<StartupTest>>
    {
        private readonly CustomWebApplicationFactory<StartupTest> _factory;
        private readonly Faker _faker;
        private readonly UserViewModel _userViewModel;
        private readonly string _token;

        public ValidPasswordControllerTests(CustomWebApplicationFactory<StartupTest> factory)
        {
            _factory = factory; 
            _faker = new Faker();
            _userViewModel = new UserViewModel()
            {
                UserName = _faker.Name.FullName().Replace(" ", "").ToLower(),
                Password = _faker.Internet.Password(7)
            };
            _token = ObterToken().Result;
        }

        [Fact]
        [Trait("Validar Senha", "Senha Válida")]
        public async Task Senha_Valida()
        {
            //Arrange
            var client = _factory.CreateClient();
            string password = "@B7defghi";

            //Act
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await client.GetAsync("/api/v1/validpassword?password=" + password);

            //Assert
            response.EnsureSuccessStatusCode()
                .IsSuccessStatusCode
                .Should()
                .BeTrue();

            bool.TryParse(response.Content.ReadAsStringAsync().Result, out bool resp);

            resp.Should().BeTrue();
        }

        [Theory]
        [Trait("Validar Senha", "Senha Inválida")]
        [InlineData("asdfghjkq7@")] //Falta Letra Maiuscula
        [InlineData("ASDFGHJKQ7@")] //Falta Letra Minuscula
        [InlineData("asdfghjkqp7")] //Falta Caractere Especial
        [InlineData("A@sdhjkqp")] //Falta Número
        [InlineData("aA7sdhjk@qpa")] //Possui Letra Repetida
        [InlineData("aB1@cder")] //Possui Menos de 9 Caracteres
        public async Task Senha_Invalida(string password)
        {
            //Arrange
            var client = _factory.CreateClient();
            
            //Act
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await client.GetAsync("/api/v1/validpassword?password=" + password);

            //Assert
            response.IsSuccessStatusCode
                .Should()
                .BeFalse();

            bool.TryParse(response.Content.ReadAsStringAsync().Result, out bool resp);

            resp.Should().BeFalse();
        }

        [Fact]
        [Trait("Validar Senha", "Erro ao Validar Senha")]
        public void Senha_Erro_Validar_Senha()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<ValidPasswordController>>();
            var passwordValidServiceMock = new Mock<IPasswordValidService>();
            passwordValidServiceMock.Setup(x => x.PasswordIsValid(It.IsAny<string>())).Throws(new Exception("testing..."));

            var controller = new ValidPasswordController(loggerMock.Object, passwordValidServiceMock.Object);

            //Act
            var response = controller.Get(_faker.Random.AlphaNumeric(9));

            //Assert
            var actionResult = Assert.IsType<ActionResult<ReturnValidPasswordViewModel>>(response);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var result = (ReturnValidPasswordViewModel)badRequestObjectResult.Value;

            result.ExecutionReturn.ErrorCode.Should().Be(2);
            result.ExecutionReturn.Message.Should().Be("Error");
        }

        public async Task<string> ObterToken()
        {
            var client = _factory.CreateClient();
            var stringPayload = JsonConvert.SerializeObject(_userViewModel);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            await client.PostAsync("/api/v1/user", content);
            var response = await client.PostAsync("/api/v1/user/login", content);

            var resp = JsonConvert.DeserializeObject<ReturnAuthViewModel>(response.Content.ReadAsStringAsync().Result);
            
            return resp.Token;
        }
    }
}
