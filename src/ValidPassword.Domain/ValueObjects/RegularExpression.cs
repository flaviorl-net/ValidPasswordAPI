using System.Collections.Generic;
using System.Linq;

namespace ValidPassword.Domain.ValueObjects
{
    public class RegularExpression
    {
        public string Description { get; set; }

        public string Pattern { get; set; }

        public static List<RegularExpression> RE
        {
            get
            {
                return new List<RegularExpression>() {
                    new RegularExpression() { Description = Enums.RegularExp.NoveDigitosOuMais.ToString(), Pattern = @"^(\w*).{9,}$" },
                    new RegularExpression() { Description = Enums.RegularExp.UmaLetraMinuscula.ToString(), Pattern = @"^(?=.*[a-z]).{9,}" },
                    new RegularExpression() { Description = Enums.RegularExp.UmaLetraMaiuscula.ToString(), Pattern = @"^(?=.*[A-Z]).{9,}" },
                    new RegularExpression() { Description = Enums.RegularExp.UmDigito.ToString(), Pattern = @"^(?=.*\d)\S{9,}" },
                    new RegularExpression() { Description = Enums.RegularExp.UmCarectereEspecial.ToString(), Pattern = @"^(?=.*[!@#$%^&*()\-+]).{9,}" },
                    new RegularExpression() { Description = Enums.RegularExp.NaoPermiteEspacoVazio.ToString(), Pattern = @"^(?=.*)\S{9,}" }
                };
            }
        }

        public static string GetPattern(Enums.RegularExp tipoValidacao)
        {
            return RE
                .FirstOrDefault(x => x.Description == tipoValidacao.ToString())
                ?.Pattern;
        }
    }
}
