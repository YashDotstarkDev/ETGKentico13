using ETG.Core.Forms;
using FluentValidation;

namespace ETG.Data.Validation.Validators
{
    public class BrochureOrderValidator : AbstractValidator<BrochureOrderItem>
    {
        public BrochureOrderValidator()
        {
            RuleFor(model => model.FirstName).NotNull().NotEmpty()
                .WithMessage("Please enter your first name");

            RuleFor(model => model.LastName).NotNull().NotEmpty()
                .WithMessage("Please enter your last name");

            RuleFor(model => model.Email).NotNull().NotEmpty()
                .WithMessage("Please enter your email address");
            RuleFor(model => model.BrochureOrders).NotNull().NotEmpty()
                .WithMessage("Please add brochure orders");
            //RuleFor(model => model.Message).NotNull().NotEmpty()
            //    .WithMessage("Please enter your question or comments");
        }
    }
}
