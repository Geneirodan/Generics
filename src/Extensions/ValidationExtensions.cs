using FluentResults;
using FluentValidation.Results;

namespace Geneirodan.Generics.Extensions;

public static class ValidationExtensions
{
    public static Result ToFluentResult(this ValidationResult result)
    {
        IEnumerable<Error> errors = [];
        
        if (!result.IsValid)
        {
            errors = result.Errors.GroupBy(x => x.PropertyName, (propertyName, validationFailures) =>
            {
                var error = new Error(propertyName);
                var messages = validationFailures.Select(x => new Error(x.ErrorMessage));
                error.Reasons.AddRange(messages);
                return error;
            });
        }
        
        return new Result().WithErrors(errors);
    }


}
