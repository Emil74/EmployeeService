using EmployeeService.Models.Requests;
using FluentValidation;

namespace EmployeeService.Models.Validators
{
    public class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>
    {
        public AuthenticationRequestValidator()
        {
            RuleFor(x => x.Login)
                .NotNull()
                .Length(7, 25)
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotNull()
                .Length(5, 30);
        }
    }
}
