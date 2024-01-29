namespace Geneirodan.Generics.Results;

public class ForbidResult : Result
{
    public ForbidResult() => WithError("Forbidden");
}