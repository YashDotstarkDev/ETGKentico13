using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;

namespace ETG.Web.Validation
{
    public class ValidationProvider<TModel> : IValidationProvider<TModel>
    {
        //private readonly IEventLogRepository _eventLogRepository;

        public ValidationProvider()//IEventLogRepository eventLogRepository)
        {
            //_eventLogRepository = eventLogRepository;
        }

        public ValidationResult Validate(AbstractValidator<TModel> validator, TModel model,
            ModelStateDictionary modelState = null)
        {
            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid)
            {
                return validatorResult;
            }

            //LogValidationErrors(validatorResult.Errors, modelState);

            return validatorResult;
        }
        /*
        private void LogValidationErrors(IEnumerable<ValidationFailure> errors, ModelStateDictionary modelState = null)
        {
            foreach (var failure in errors)
            {
                modelState?.AddModelError(failure.PropertyName, failure.ErrorMessage);

                _eventLogRepository.LogError(nameof(ValidationProvider<TModel>), nameof(Validate),
                    $"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}");
            }
        }*/
    }
}