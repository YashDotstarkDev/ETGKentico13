using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;

namespace ETG.Web.Validation
{
    public interface IValidationProvider<TModel>
    {
        ValidationResult Validate(AbstractValidator<TModel> validator, TModel model,
            ModelStateDictionary modelState = null);
    }
}
