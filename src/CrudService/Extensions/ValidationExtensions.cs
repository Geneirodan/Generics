using FluentResults;
using FluentValidation.Results;

namespace Geneirodan.Generics.CrudService.Extensions;

public static class ValidationExtensions
{
    public static Result ToFluentResult(this ValidationResult result)
    {
        List<Error> errors = [];
        if (!result.IsValid)
        {
            errors = result.Errors.GroupBy(x => x.PropertyName, (s, failures) =>
            {
                var e = new Error(s);
                e.Reasons.AddRange(failures.Select(x => new Error(x.ErrorMessage)));
                return e;
            }).ToList();
        }
        return errors.Count > 0
            ? Result.Fail(errors)
            : Result.Ok();
    }


}
