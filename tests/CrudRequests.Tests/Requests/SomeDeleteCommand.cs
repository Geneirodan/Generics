using Geneirodan.Generics.CrudRequests.Interfaces;

namespace Geneirodan.Generics.CrudRequests.Tests.Requests;

public record struct SomeDeleteCommand(int Id): IDeleteCommand<int>;
