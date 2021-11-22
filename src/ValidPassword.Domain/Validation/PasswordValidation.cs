using System;
using System.Collections.Generic;
using System.Text;

namespace ValidPassword.Domain.Validation
{
    public class PasswordValidation : FiscalBase<string>
    {
        public PasswordValidation()
        {
            var naoPermiteEspacoVazio = new Password.NaoPermiteEspacoVazio();
            var naoPossuirCaracteresRepetidos = new Password.NaoPossuirCaracteresRepetidos();
            var possuiNoveOuMaisCaracteres = new Password.NoveOuMaisCaracteres();
            var umaLetraMaiuscula = new Password.UmaLetraMaiuscula();
            var umaLetraMinuscula = new Password.UmaLetraMinuscula();
            var umCarectereEspecial = new Password.UmCarectereEspecial();
            var umDigito = new Password.UmDigito();

            base.AddRule("naoPermiteEspacoVazio", new Rule<string>(naoPermiteEspacoVazio, ""));
            base.AddRule("naoPossuirCaracteresRepetidos", new Rule<string>(naoPossuirCaracteresRepetidos, ""));
            base.AddRule("possuiNoveOuMaisCaracteres", new Rule<string>(possuiNoveOuMaisCaracteres, ""));
            base.AddRule("umaLetraMaiuscula", new Rule<string>(umaLetraMaiuscula, ""));
            base.AddRule("umaLetraMinuscula", new Rule<string>(umaLetraMinuscula, ""));
            base.AddRule("umCarectereEspecial", new Rule<string>(umCarectereEspecial, ""));
            base.AddRule("umDigito", new Rule<string>(umDigito, ""));
        }
    }
}
