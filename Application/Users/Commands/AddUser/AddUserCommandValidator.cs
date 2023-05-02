using FluentValidation;
using Application.Common.ServiceInterfaces;

namespace Application.Users.Commands.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator(IMainDbContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Must(x => context.Users.Where(a => a.Name == x).Any())
                .WithMessage("User with same name already exist.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(200)
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
        }
    }
}
