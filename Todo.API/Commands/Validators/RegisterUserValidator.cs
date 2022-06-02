using FluentValidation;
using Todo.API.Infrastructure;

namespace Todo.API.Commands.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser>
    {
        public RegisterUserValidator(TodoDbContext dbContext)
        {
            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(u => u.ConfirmPassword)
                .Equal(u => u.Password);

            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(u => u.Email)
                .Custom((value, validationContext) =>
                {
                    if (dbContext.Users.Any(u => u.Email == value))
                    {
                        validationContext.AddFailure("Email", "The Email is taken");
                    }
                });
        }
    }
}