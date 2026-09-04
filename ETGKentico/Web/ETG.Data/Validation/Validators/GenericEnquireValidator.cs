using ETG.Core.Forms;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Data.Validation.Validators
{
    public class GenericEnquireValidator : AbstractValidator<EnquireItem>
    {
        public GenericEnquireValidator()
        {
            RuleFor(model => model.Firstname).NotNull().NotEmpty()
                .WithMessage("Please enter your first name");

            RuleFor(model => model.Lastname).NotNull().NotEmpty()
                .WithMessage("Please enter your last name");

            //RuleFor(model => model.Email).NotNull().NotEmpty()
            //    .WithMessage("Please enter your email address");

            RuleFor(model => model.Firstname).NotEqual(m => m.Lastname)
                .WithMessage("First and last name cannot be the same");

            //RuleFor(model => model.Message).NotNull().NotEmpty()
            //    .WithMessage("Please enter your question or comments");
        }
    }
}
