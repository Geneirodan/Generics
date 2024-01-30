using AutoFilterer.Types;
using FluentValidation;
using MockQueryable.Moq;

namespace Geneirodan.Generics.CrudService.Tests.Data;


public class SampleEntity : IEntity<int>
{
    public int Id { get; init; }
    public int Smth { get; init; }
}

public record SampleAddModel(int Smth) : IAddModel<SampleEntity>;

public class SampleAddModelValidator : AbstractValidator<SampleAddModel>
{
    public SampleAddModelValidator() => RuleFor(x => x.Smth).NotEqual(SampleData.MagicalNumber);
}
public record SampleEditModel(int? Smth) : IEditModel<SampleEntity>;


public class SampleEditModelValidator : AbstractValidator<SampleEditModel>
{
    public SampleEditModelValidator() => RuleFor(x => x.Smth).NotEqual(SampleData.MagicalNumber);
}


public record SampleViewModel(int Smth) : IViewModel<SampleEntity>;

internal class Filter : PaginationFilterBase
{
    public int? Id { get; set; }
    public int? Smth { get; set; }
}
public static class SampleData
{
    public const string Message = nameof(Message);
    public const int MagicalNumber = 184;
    internal static readonly SampleEntity sampleEntity = new() { Id = 1, Smth = 2};
    internal static readonly IQueryable<SampleEntity> entities = new List<SampleEntity>
    {
        sampleEntity,
        new(){Id = 2, Smth = 4},
        new(){Id = 3, Smth = 8}
    }.AsQueryable().BuildMockDbSet().Object;
    
    internal static readonly IQueryable<int> validIds = entities.Select(x => x.Id);
    
    
    public static TheoryData<int> Ids => new() { 0, 1, 2, 3, 4, 5 };

    public static TheoryData<IPaginationFilter> Filters =>
        new()
        {
            new Filter { Id = 1 },
            new Filter { Smth = 4 },
            new Filter { Smth = 234 },
            new Filter { Smth = 184 }
        };
    
    public static TheoryData<SampleAddModel> AddModels =>
        new()
        {
            new SampleAddModel(1),
            new SampleAddModel(MagicalNumber)
        };

    public static TheoryData<int, SampleEditModel> EditModels =>
        new()
        {
            { 1, new SampleEditModel(1) },
            { 1, new SampleEditModel(MagicalNumber) },
            { 0, new SampleEditModel(1) },
            { 0, new SampleEditModel(MagicalNumber) }
        };
}
