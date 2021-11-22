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
