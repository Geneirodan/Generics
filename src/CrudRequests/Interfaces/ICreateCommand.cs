using FluentResults;

namespace Geneirodan.Generics.CrudRequests.Interfaces;

public interface ICreateCommand<TViewModel> : IRequest<Result<TViewModel>>;
