using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.API.Queries.Validators
{
    public class GetTasksValidator : AbstractValidator<GetTasks>
    {
        public GetTasksValidator()
        {
            var allowedCategories = new[] { "completed", "notCompleted", "beforeDeadline", "afterDeadline" };

            RuleFor(x => x.Categories)
                .Custom((value, context) =>
                {
                    if (value.Except(allowedCategories).Any())
                        context.AddFailure($"Categories are optional or must be [{string.Join(',', allowedCategories)}]");
                });
        }
    }
}