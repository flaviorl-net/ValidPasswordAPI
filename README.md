## ValidPasswordAPI
API para validar senha usando algumas regras especificas.

### Regras
A senha deve possuir:
* Nove digitos ou mais;
* Pelo menos 1 dígito;
* Pelo menos 1 letra minúscula;
* Pelo menos 1 letra maiúscula;
* Pelo menos 1 caractere especial: !@#$%^&*()-+;
* Não é permitido o uso de caracteres repetidos;

### Execução do Projeto
1. Baixe a branch: git clone https://github.com/flaviorl-net/ValidPasswordAPI;
2. Usando o Visual Studio ou Code, execute o projeto;
3. Faça um post para https://localhost:44351/api/v1/user 
* Body: 
{
    "UserName": "usuario",
    "Password": "12345"
}
4. Faça um post para https://localhost:44351/api/v1/user/login
* Body:
{
    "UserName": "usuario",
    "Password": "12345"
}
6. Faça um get para https://localhost:44351/api/v1/validpassword?password=SUASENHA
* Utilize o token da etapa anterior no header Authorization (Bearer)

7. Se sua senha obedecer todas as regras você irá receber true e status code 200, inválida irá retornar false e status code 400 bad resquest.

OBS: use https://localhost:44351/swagger/index.html para ter um overview das operações disponiveis.

### Detalhes da Solução
Utilização do Specification Pattern para permitir a facil inclusão e/ou remoção de regras de validação.
Cada regra para criação da senha, é representada por um objeto/classe dentro do padrão.

A solução foi dividida em 5 camadas/pacotes, conforme segue:
* API: regras e recursos para o funcionamento da API, como por ex: Swagger, Rate Limit, Logging, Authentication, Mapper; A API foi desenvolvida com o .Net Core 3.1;
* Domain: classes para a validação da senha, possui as regras;
* Data: definição da classe context (EFCore) apenas para registro do usuário. No momento apenas em memória;
* Logging: criação de log personalizado, a titulo de exemplo, poderia utilizar Serilog ou outro, gravação apenas em arquivo de texto para exemplificar;
* Token: projeto para a geração do token, utilizado na autenticação via header;

Para a validação da senha (resquest/response) é necessária apenas a camada de API e Domain, as demais camadas servem apenas de apoio, para oferecer outros serviços, como registro do usuário em Data, por exemplo.

#### Domain
Projeto que possui regras de validação, como dito anteriormente utilizando o Specification Pattern.
Para adicionar uma nova regra, crie uma classe conforme exemplo abaixo:

```csharp
public class MinhaRegra : ISpecification<string> //Pode ser uma classe, basta ajustar o padrão.
{
    public bool IsSatisfiedBy(string password)
    {
        if (true)
        {
            return true;
        }

        return false;
    }
}
```

Após isso adicione na classe que herda de FiscalBase:
```csharp
public class PasswordValidation : FiscalBase<string>
{
    public PasswordValidation()
    {
        var minhaRegra = new MinhaRegra();
        
        base.AddRule("minhaRegra", new Rule<string>(minhaRegra, "Mensagem de erro"));
    }
}
```

### Decisão de Projeto
A API, no que se trata ao registro do usuário, funcionalidade que utiliza armazenamento em memória, foi feito com o conceito de Data Driven, o contexto é criado pelo macanismo de injeção de depência direto no controller, para fins de simplificar esse processo, nesse momento.

### Testes
Para a realização dos testes foram utilizadas as seguintes ferramentas e pacotes:
* XUnit;
* FluentAssertions;
* Bogus;
* Moq;
* Newtonsoft;
* Microsoft.AspNetCore.Mvc.Testing;
* Microsoft.NET.Test.Sdk
