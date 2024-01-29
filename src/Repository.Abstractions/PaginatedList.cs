namespace Geneirodan.Generics.Repository.Abstractions;

public record PaginatedList<T>(IEnumerable<T> List, int PageCount);
