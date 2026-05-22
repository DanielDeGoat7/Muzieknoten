using Microsoft.AspNetCore.Identity;

namespace Piano.Identity
{
    public class DutchIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Het wachtwoord moet minimaal {length} tekens bevatten."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Het wachtwoord moet minimaal één hoofdletter bevatten."
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "Het wachtwoord moet minimaal één cijfer bevatten."
            };
        }

        public override IdentityError DuplicateUserName(string email)
        {
            return new IdentityError { Code = nameof(DuplicateUserName), Description = $"E-mailadres '{email}' is al in gebruik." };
        }
    }
}