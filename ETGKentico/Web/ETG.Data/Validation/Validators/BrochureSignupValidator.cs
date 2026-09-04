using ETG.Core.Forms;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Data.Validation.Validators
{
    public class BrochureSignupValidator : AbstractValidator<BrochureSignupItem>
    {
        public BrochureSignupValidator()
        {
            RuleFor(model => model.Name).NotNull().NotEmpty()
                .WithMessage("Please enter your first name");

            RuleFor(model => model.BrochureName).NotNull().NotEmpty();

            RuleFor(model => model.Email).NotNull().NotEmpty()
                .WithMessage("Please enter your email address");

            RuleFor(model => model.Url).NotNull().NotEmpty();
        }
    }
}
