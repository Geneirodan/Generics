using Geneirodan.Generics.CrudRequests.Handles;
using Geneirodan.Generics.CrudRequests.Tests.Repository;
using Geneirodan.Generics.CrudRequests.Tests.Requests;

namespace Geneirodan.Generics.CrudRequests.Tests.Handles;

public class SomeUpdateCommandHandle(ISomeRepository repository) : UpdateCommandHandle<SomeUpdateCommand, SomeEntity, int, ViewModel>(repository);
