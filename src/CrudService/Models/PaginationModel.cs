namespace Geneirodan.Generics.CrudService.Models;

public record PaginationModel<T>(IEnumerable<T> List, int PageCount);
