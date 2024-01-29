using Geneirodan.Generics.CrudRequests.Handles;
using Geneirodan.Generics.CrudRequests.Tests.Repository;
using Geneirodan.Generics.CrudRequests.Tests.Requests;

namespace Geneirodan.Generics.CrudRequests.Tests.Handles;

public class SomeQueryHandle(ISomeRepository repository) : PaginatedQueryHandle<PaginatedQuery, SomeEntity, int, ViewModel>(repository);
