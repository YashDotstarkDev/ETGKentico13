using ETG.Core.Forms;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Data.Validation.Validators
{
    public class CompetitionAgentValidator : AbstractValidator<CompetitionAgentsItem>
    {
        public CompetitionAgentValidator()
        {
            RuleFor(model => model.FirstName).NotNull().NotEmpty()
                .WithMessage("Please enter your first name");

            RuleFor(model => model.LastName).NotNull().NotEmpty();

            RuleFor(model => model.EmailAddress).NotNull().NotEmpty()
                .WithMessage("Please enter your email address");

        }
    }
}
