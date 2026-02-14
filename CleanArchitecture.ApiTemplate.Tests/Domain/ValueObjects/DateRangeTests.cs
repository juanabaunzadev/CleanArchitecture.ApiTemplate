using CleanArchitecture.ApiTemplate.Domain.Exceptions;
using CleanArchitecture.ApiTemplate.Domain.ValueObject;

namespace CleanArchitecture.ApiTemplate.Tests.Domain.ValueObjects;

[TestClass]
public class DateRangeTests
{
    [TestMethod]
    public void Constructor_StartDateAfterEndDate_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(-1);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => new DateRange(startDate, endDate));
    }

    [TestMethod]
    public void Constructor_ValidDates_CreatesDateRange()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(1);

        // Act
        var dateRange = new DateRange(startDate, endDate);

        // Assert
        Assert.AreEqual(startDate, dateRange.StartDate);
        Assert.AreEqual(endDate, dateRange.EndDate);
    }

    [TestMethod]
    public void Complete_EndDateAlreadySet_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(1);
        var dateRange = new DateRange(startDate, endDate);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => dateRange.Complete());
    }

    [TestMethod]
    public void Complete_EndDateNotSet_ShouldSetEndDate()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var dateRange = new DateRange(startDate);

        // Act
        dateRange.Complete();

        // Assert
        Assert.IsTrue(dateRange.EndDate.HasValue);
        Assert.IsTrue(dateRange.EndDate.Value >= startDate);
    }
}