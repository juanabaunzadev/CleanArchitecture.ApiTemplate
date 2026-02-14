using CleanArchitecture.ApiTemplate.Domain.Entities;
using CleanArchitecture.ApiTemplate.Domain.Enums;
using CleanArchitecture.ApiTemplate.Domain.Exceptions;

namespace CleanArchitecture.ApiTemplate.Tests.Domain.Entities;

[TestClass]
public class ToDoTests
{
    [TestMethod]
    public void Constructor_NullOrEmptyName_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() => new ToDo(null!));
        Assert.Throws<BusinessRuleException>(() => new ToDo(string.Empty));
        Assert.Throws<BusinessRuleException>(() => new ToDo("   "));
    }

    [TestMethod]
    public void Constructor_ValidParameters_CreatesToDo()
    {
        // Arrange
        var name = "Test ToDo";
        var description = "This is a test ToDo item.";
        
        // Act
        var toDo = new ToDo(name, description);

        // Assert
        Assert.AreNotEqual(Guid.Empty, toDo.Id);
        Assert.AreEqual(name, toDo.Name);
        Assert.AreEqual(description, toDo.Description);
        Assert.AreEqual(ToDoStatus.NotStarted, toDo.Status);
        Assert.IsTrue(toDo.CreatedDate <= DateTime.UtcNow);
    }

    [TestMethod]
    public void MarkAsInProgress_ValidTransition_UpdatesStatusAndTimeline()
    {
        // Arrange
        var toDo = new ToDo("Test ToDo");
        
        // Act
        toDo.MarkAsInProgress();
        
        // Assert
        Assert.AreEqual(ToDoStatus.InProgress, toDo.Status);
        Assert.IsNotNull(toDo.Timeline);
    }

    [TestMethod]
    public void MarkAsInProgress_InvalidTransition_ThrowsBusinessRuleException()
    {
        // Arrange
        var toDo = new ToDo("Test ToDo");

        // Act
        toDo.MarkAsInProgress();
        
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => toDo.MarkAsInProgress());
    }

    [TestMethod]
    public void MarkAsCompleted_ValidTransition_UpdatesStatusAndTimeline()
    {
        // Arrange
        var toDo = new ToDo("Test ToDo");
        toDo.MarkAsInProgress();
        
        // Act
        toDo.MarkAsCompleted();
        
        // Assert
        Assert.AreEqual(ToDoStatus.Completed, toDo.Status);
        Assert.IsNotNull(toDo.Timeline);
        Assert.IsTrue(toDo.Timeline!.EndDate.HasValue);
    }

    [TestMethod]
    public void MarkAsCompleted_InvalidTransition_ThrowsBusinessRuleException()
    {
        // Arrange
        var toDo = new ToDo("Test ToDo");

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => toDo.MarkAsCompleted());
    }
}