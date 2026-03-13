using CleanArchitecture.ApiTemplate.Application.Abstractions.Persistence;
using CleanArchitecture.ApiTemplate.Application.Abstractions.Repositories;
using CleanArchitecture.ApiTemplate.Application.Features.ToDos.Commands.CreateToDo;
using CleanArchitecture.ApiTemplate.Domain.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CleanArchitecture.ApiTemplate.Tests.Application.Features.ToDos;

[TestClass]
public class CreateToDoCommandHandlerTests
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private IRepositoryToDo _repositoryToDo;
    private IUnitOfWork _unitOfWork;
    private CreateToDoCommandHandler _handler;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [TestInitialize]
    public void Setup()
    {
        _repositoryToDo = Substitute.For<IRepositoryToDo>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateToDoCommandHandler(_repositoryToDo, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_ValidCommand_ShouldCreateToDo()
    {
        // Arrange
        var command = new CreateToDoCommand("Test ToDo", "Test Description");
        var toDo = new ToDo(command.Name, command.Description);
        
        _repositoryToDo.Add(Arg.Any<ToDo>()).Returns(toDo);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _repositoryToDo.Received(1).Add(Arg.Any<ToDo>());
        await _unitOfWork.Received(1).CommitAsync();
        Assert.AreEqual(toDo.Id, result);
    }

    [TestMethod]
    public async Task Handle_RepositoryThrowsException_ShouldRollbackUnitOfWork()
    {
        // Arrange
        var command = new CreateToDoCommand("Test ToDo", "Test Description");
        
        _repositoryToDo.Add(Arg.Any<ToDo>()).Throws<Exception>();

        // Act
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

        // Assert
        await _repositoryToDo.Received(1).Add(Arg.Any<ToDo>());
        await _unitOfWork.Received(1).RollbackAsync();
}
}