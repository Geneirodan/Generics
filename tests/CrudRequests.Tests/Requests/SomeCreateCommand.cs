using Geneirodan.Generics.CrudRequests.Interfaces;
using Geneirodan.Generics.CrudRequests.Tests.Repository;

namespace Geneirodan.Generics.CrudRequests.Tests.Requests;

public record struct SomeCreateCommand(string Data): ICreateCommand<ViewModel>;
