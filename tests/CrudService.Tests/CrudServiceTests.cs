using AutoFilterer.Extensions;
using FluentAssertions;
using FluentResults;
using Geneirodan.Generics.CrudService.Tests.Data;
using Geneirodan.Generics.Repository.Abstractions;
using Geneirodan.Generics.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Moq;
using static Geneirodan.Generics.CrudService.Tests.Data.SampleData;


namespace Geneirodan.Generics.CrudService.Tests;

public class CrudServiceTests
{
    private readonly ICrudService<SampleEntity, SampleViewModel, int> _service;
    private readonly Mock<IRepository<SampleEntity, int>> _repository;

    public CrudServiceTests()
    {
        _repository = new Mock<IRepository<SampleEntity, int>>();
        _repository.Setup(x => x.ConfirmAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _repository.Setup(x => x.GetAll()).ReturnsAsync(entities);
        _service = new CrudService<SampleEntity, SampleViewModel, int>(_repository.Object);
    }
    private static Func<Times> GetTimes(bool b) => b ? Times.Once : Times.Never;

    private void SetupForId(int id) => _repository
        .Setup(x => x.GetAsync(id))
        .Returns(entities.FirstOrDefaultAsync(y => y.Id == id));

    private void SetupForAdd(SampleAddModel model) => _repository
        .Setup(x => x.Add(It.Is<SampleEntity>(y => y.Smth == model.Smth)))
        .Returns(model.Adapt<SampleEntity>());

    private void SetupSaveError() => _repository
        .Setup(x => x.ConfirmAsync(It.IsAny<CancellationToken>()))
        .ThrowsAsync(new Exception(Message));

    [Theory, MemberData(nameof(Ids), MemberType = typeof(SampleData))]
    public async void GetByIdAsync(int id)
    {
        SetupForId(id);
        var viewModel = await _service.GetByIdAsync(id);
        var entity = await entities.FirstOrDefaultAsync(x => x.Id == id);
        viewModel.Should().BeEquivalentTo(entity.Adapt<SampleViewModel>());
    }

    [Theory, MemberData(nameof(Filters), MemberType = typeof(SampleData))]
    public async void GetAsync(IPaginationFilter filter)
    {
        var sampleEntities = entities.ApplyFilter(filter);
        _repository.Setup(x => x.GetAll(filter)).ReturnsAsync(new PaginatedList<SampleEntity>(sampleEntities, 0));
        var model = await _service.GetAsync(filter);
        model.List.Should().BeEquivalentTo(sampleEntities.Adapt<IEnumerable<SampleViewModel>>());
    }

    [Theory, MemberData(nameof(AddModels), MemberType = typeof(SampleData))]
    public async void AddAsync(SampleAddModel model)
    {
        SetupForAdd(model);
        var result = await _service.AddAsync(model);
        result.IsSuccess.Should().BeTrue();
        _repository.Verify(x => x.Add(It.IsAny<SampleEntity>()), Times.Once);
        _repository.Verify(x => x.ConfirmAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, MemberData(nameof(AddModels), MemberType = typeof(SampleData))]
    public async void AddAsync_WithValidation(SampleAddModel model)
    {
        SetupForAdd(model);
        var result = await _service.AddAsync(model, new SampleAddModelValidator());

        var b = model.Smth != MagicalNumber;
        result.IsSuccess.Should().Be(b);

        var times = GetTimes(b);
        _repository.Verify(x => x.Add(It.IsAny<SampleEntity>()), times);
        _repository.Verify(x => x.ConfirmAsync(It.IsAny<CancellationToken>()), times);
    }

    [Theory, MemberData(nameof(AddModels), MemberType = typeof(SampleData))]
    public async void AddAsync_Failed(SampleAddModel model)
    {
        SetupForAdd(model);
        SetupSaveError();
        var result = await _service.AddAsync(model);
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainEquivalentOf(new Error(Message));
        _repository.Verify(x => x.Add(It.IsAny<SampleEntity>()), Times.Once);
        _repository.Verify(x => x.ConfirmAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, MemberData(nameof(EditModels), MemberType = typeof(SampleData))]
    public async void EditAsync(int id, SampleEditModel model)
    {
        SetupForId(id);
        var result = await _service.EditAsync(id, model);
        result.IsSuccess.Should().BeTrue();
        _repository.Verify(x => x.Update(It.IsAny<SampleEntity>()), Times.Once);
        _repository.Verify(x => x.ConfirmAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, MemberData(nameof(EditModels), MemberType = typeof(SampleData))]
    public async void EditAsync_WithValidation(int id, SampleEditModel model)
    {
        SetupForId(id);
        var result = await _service.EditAsync(id, model, new SampleEditModelValidator());

        var b = model.Smth != MagicalNumber;
        result.IsSuccess.Should().Be(b);

        var times = GetTimes(b);
        _repository.Verify(x => x.Update(It.IsAny<SampleEntity>()), times);
        _repository.Verify(x => x.ConfirmAsync(It.IsAny<CancellationToken>()), times);
    }

    [Theory, MemberData(nameof(EditModels), MemberType = typeof(SampleData))]
    public async void EditAsync_Failed(int id, SampleEditModel model)
    {
        SetupForId(id);
        SetupSaveError();
        var result = await _service.EditAsync(id, model);
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainEquivalentOf(new Error(Message));
        _repository.Verify(x => x.Update(It.IsAny<SampleEntity>()), Times.Once);
        _repository.Verify(x => x.ConfirmAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, MemberData(nameof(Ids), MemberType = typeof(SampleData))]
    public async void DeleteAsync(int id)
    {
        SetupForId(id);
        var result = await _service.DeleteAsync(id);
        var b = await validIds.ContainsAsync(id);
        result.IsSuccess.Should().Be(b);

        var times = GetTimes(b);
        _repository.Verify(x => x.Remove(It.IsAny<SampleEntity>()), times);
        _repository.Verify(x => x.ConfirmAsync(It.IsAny<CancellationToken>()), times);
    }

    [Theory, MemberData(nameof(Ids), MemberType = typeof(SampleData))]
    public async void DeleteAsync_Failed(int id)
    {
        SetupForId(id);
        SetupSaveError();
        var result = await _service.DeleteAsync(id);
        result.IsSuccess.Should().BeFalse();

        var b = await validIds.ContainsAsync(id);
        if (b)
            result.Errors.Should().ContainEquivalentOf(new Error(Message));
        else
            result.Should().BeOfType<NotFoundResult>();

        var times = GetTimes(b);
        _repository.Verify(x => x.Remove(It.IsAny<SampleEntity>()), times);
        _repository.Verify(x => x.ConfirmAsync(It.IsAny<CancellationToken>()), times);
    }
}
