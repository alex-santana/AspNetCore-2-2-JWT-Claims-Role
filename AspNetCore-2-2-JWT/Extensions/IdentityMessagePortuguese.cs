using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore_2_2_JWT.Extensions
{
    public class IdentityMessagePortuguese : IdentityErrorDescriber
    {
        //sobrescrendo do metodo padrão das mensagens do Identity
        public override IdentityError DefaultError()
        {
            return new IdentityError { Code = nameof(DefaultError), Description = $"Ocorreu um erro. Não se preocupe nossa equipe, em breve, analisará." };
        
        }
        public override IdentityError PasswordMismatch()
        {
            return new IdentityError { Code = nameof(PasswordMismatch), Description = $"Token inválido" };
        }

        //Outra forma, mais limpa, com expressao chamando um método padrão para retorno do IdentityError
        public override IdentityError InvalidToken() => IdentityMessageError(nameof(InvalidToken), $"Token inválido");

        public override IdentityError PasswordRequiresNonAlphanumeric() => IdentityMessageError(nameof(PasswordRequiresNonAlphanumeric), $"A senha deverá ter ao menos um caracter não alphanumerico.");
        private IdentityError IdentityMessageError(string code, string description) => new IdentityError { Code = code, Description = description };


        /*
         * Para sobrescrever outros IdentityError, observe a key da mensagem de erro no retorno da API 
         *Exemplo: 
         * 
         *  "PasswordRequiresDigit": ["Passwords must have at least one digit ('0'-'9')."]
         * 
         * Override:
         * 
         * public override IdentityError PasswordRequiresDigit() => IdentityMessageError(nameof(PasswordRequiresDigit), $"MENSAGEM CORRESPONDENTE");
         * 
         */
    }
}
