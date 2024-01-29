using Geneirodan.Generics.CrudRequests.Handles;
using Geneirodan.Generics.CrudRequests.Tests.Repository;
using Geneirodan.Generics.CrudRequests.Tests.Requests;

namespace Geneirodan.Generics.CrudRequests.Tests.Handles;

public class SomeDeleteCommandHandle(ISomeRepository repository) : DeleteCommandHandle<SomeDeleteCommand, SomeEntity, int>(repository);
