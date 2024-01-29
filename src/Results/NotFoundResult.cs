namespace Geneirodan.Generics.Results;

public class NotFoundResult : Result
{
    public NotFoundResult() => WithError("Not Found");
}
