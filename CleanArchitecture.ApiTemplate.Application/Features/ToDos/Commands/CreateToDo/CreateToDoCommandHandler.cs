using CleanArchitecture.ApiTemplate.Application.Abstractions.Persistence;
using CleanArchitecture.ApiTemplate.Application.Abstractions.Repositories;
using CleanArchitecture.ApiTemplate.Application.Exceptions;
using CleanArchitecture.ApiTemplate.Application.Utilities.Messaging;
using CleanArchitecture.ApiTemplate.Domain.Entities;
using FluentValidation;

namespace CleanArchitecture.ApiTemplate.Application.Features.ToDos.Commands.CreateToDo;

public class CreateToDoCommandHandler : IRequestHandler<CreateToDoCommand, Guid>
{
    private readonly IRepositoryToDo _repositoryToDo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateToDoCommandHandler(
        IRepositoryToDo repositoryToDo,
        IUnitOfWork unitOfWork)
    {
        _repositoryToDo = repositoryToDo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateToDoCommand command, CancellationToken cancellationToken)
    {
        var todo = new ToDo(command.Name, command.Description);
        
        try
        {
            var result = await _repositoryToDo.Add(todo);
            await _unitOfWork.CommitAsync();
            
            return result.Id;
        }
        catch(Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}