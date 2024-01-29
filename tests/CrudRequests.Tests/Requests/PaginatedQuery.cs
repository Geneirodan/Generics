using AutoFilterer.Abstractions;
using Geneirodan.Generics.CrudRequests.Interfaces;
using Geneirodan.Generics.CrudRequests.Tests.Repository;

namespace Geneirodan.Generics.CrudRequests.Tests.Requests;

public record struct PaginatedQuery(IPaginationFilter Filter) : IPaginatedQuery<ViewModel>;
