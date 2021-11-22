using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ValidPassword.API;
using ValidPassword.API.ViewModel;
using Xunit;
using Bogus;
using Bogus.Extensions.Brazil;

namespace ValidPassword.Tests.API
{
    [TestCaseOrderer("ValidPassword.Tests.PriorityOrderer", "ValidPassword")]
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<StartupTest>>
    {
        private readonly CustomWebApplicationFactory<StartupTest> _factory;
        private readonly Faker _faker;
        private readonly UserViewModel _userViewModel;

        public UserControllerTests(CustomWebApplicationFactory<StartupTest> factory)
        {
            _factory = factory;
            _faker = new Faker();
            _userViewModel = new UserViewModel()
            {
                UserName = _faker.Name.FullName().Replace(" ", "").ToLower(),
                Password = _faker.Internet.Password(7)
            };
        }

        [Fact, TestPriority(0)]
        public async Task Cadastrar_Usuario_Com_Sucesso()
        {
            //Arrange
            var client = _factory.CreateClient();
            var stringPayload = JsonConvert.SerializeObject(_userViewModel);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/v1/user", content);

            //Assert
            response.EnsureSuccessStatusCode()
                .IsSuccessStatusCode
                .Should()
                .BeTrue();

            var resp = JsonConvert.DeserializeObject<ReturnUserViewModel>(response.Content.ReadAsStringAsync().Result);

            resp.ExecutionReturn
                .Message
                .Should()
                .Be("Ok");

            resp.ExecutionReturn
                .ErrorCode
                .Should()
                .Be(0);
        }

        [Theory]
        [InlineData("an", "12345", "Username deve ter no minimo de 5 carateres")]
        [InlineData("abcefghejklmnoprstuvwxyz", "12345", "Username deve ter no máximo de 20 carateres")]
        public async Task Cadastrar_Usuario_UserName_Invalido(string userName, string password, string errorMessage)
        {
            //Arrange
            var client = _factory.CreateClient();
            var user = new UserViewModel() { UserName = userName, Password = password };
            var stringPayload = JsonConvert.SerializeObject(user);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/v1/user", content);

            //Assert
            response.IsSuccessStatusCode
                .Should()
                .BeFalse();

            var resp = JsonConvert.DeserializeObject<BadRequestResponse>(response.Content.ReadAsStringAsync().Result);

            resp.status.Should().Be(400);

            resp.errors.UserName.Should().Equal(errorMessage);
        }

        [Theory]
        [InlineData("antonio", "12", "Password deve ter no minimo de 5 carateres")]
        [InlineData("antonio", "12345678910111213141517", "Password deve ter no máximo de 20 carateres")]
        public async Task Cadastrar_Usuario_PassWord_Invalido(string userName, string password, string errorMessage)
        {
            //Arrange
            var client = _factory.CreateClient();
            var user = new UserViewModel() { UserName = userName, Password = password };
            var stringPayload = JsonConvert.SerializeObject(user);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/v1/user", content);

            //Assert
            response.IsSuccessStatusCode
                .Should()
                .BeFalse();

            var resp = JsonConvert.DeserializeObject<BadRequestResponse>(response.Content.ReadAsStringAsync().Result);

            resp.status.Should().Be(400);

            resp.errors.Password.Should().Equal(errorMessage);
        }

        [Theory]
        [InlineData("ant", "12", "Username deve ter no minimo de 5 carateres", "Password deve ter no minimo de 5 carateres")]
        [InlineData("abcefghejklmnoprstuvwxyz", "12345678910111213141517", "Username deve ter no máximo de 20 carateres", "Password deve ter no máximo de 20 carateres")]
        public async Task Cadastrar_Usuario_UserName_e_PassWord_Invalidos(string userName, string password, string errorMessageUserName, string erroMessagePassWord)
        {
            //Arrange
            var client = _factory.CreateClient();
            var user = new UserViewModel() { UserName = userName, Password = password };
            var stringPayload = JsonConvert.SerializeObject(user);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/v1/user", content);

            //Assert
            response.IsSuccessStatusCode
                .Should()
                .BeFalse();

            var resp = JsonConvert.DeserializeObject<BadRequestResponse>(response.Content.ReadAsStringAsync().Result);

            resp.status.Should().Be(400);

            resp.errors.UserName.Should().Equal(errorMessageUserName);
            resp.errors.Password.Should().Equal(erroMessagePassWord);
        }

        [Fact, TestPriority(1)]
        public async Task Autentica_Usuario_Com_Sucesso()
        {
            //Arrange
            var client = _factory.CreateClient();
            var stringPayload = JsonConvert.SerializeObject(_userViewModel);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            //Act
            await client.PostAsync("/api/v1/user", content);
            var response = await client.PostAsync("/api/v1/user/login", content);

            //Assert
            response.IsSuccessStatusCode
                .Should()
                .BeTrue();

            var resp = JsonConvert.DeserializeObject<ReturnAuthViewModel>(response.Content.ReadAsStringAsync().Result);

            resp.ExecutionReturn
                .Message
                .Should()
                .Be("Ok");

            resp.ExecutionReturn
                .ErrorCode
                .Should()
                .Be(0);

            resp.Token
                .Should()
                .NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Autentica_Usuario_Invalido()
        {
            //Arrange
            var client = _factory.CreateClient();
            var stringPayload = JsonConvert.SerializeObject(new UserViewModel()
            {
                UserName = _faker.Name.FirstName().ToLower(),
                Password = _faker.Internet.Password(7)
            });
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/v1/user/login", content);

            //Assert
            response.IsSuccessStatusCode
                .Should()
                .BeFalse();

            var resp = JsonConvert.DeserializeObject<ReturnAuthViewModel>(response.Content.ReadAsStringAsync().Result);

            resp.ExecutionReturn
                .Message
                .Should()
                .Be("Usuário ou senha inválidos");

            resp.ExecutionReturn
                .ErrorCode
                .Should()
                .Be(1);

            resp.Token
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task Autentica_Usuario_Senha_Nao_Informados()
        {
            //Arrange
            var client = _factory.CreateClient();
            var stringPayload = JsonConvert.SerializeObject(new UserViewModel());
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/v1/user/login", content);

            //Assert
            response.IsSuccessStatusCode
                .Should()
                .BeFalse();

            var resp = JsonConvert.DeserializeObject<BadRequestResponse>(response.Content.ReadAsStringAsync().Result);

            resp.errors.UserName.Should().Equal("O nome do usuário deve ser informado");
            resp.errors.Password.Should().Equal("A senha deve ser informada");
        }
    }
}