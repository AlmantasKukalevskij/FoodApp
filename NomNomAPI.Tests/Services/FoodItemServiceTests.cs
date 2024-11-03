using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using NomNomAPI.Data;
using NomNomAPI.Models;
using NomNomAPI.Services.FoodItemService;

public class FoodItemServiceTests
{
    private readonly Mock<DataContext> _contextMock;
    private readonly IFoodItemService _foodItemService;
    private readonly Mock<DbSet<FoodItem>> _foodItemsDbSetMock;

    public FoodItemServiceTests()
    {
        _contextMock = new Mock<DataContext>(new DbContextOptions<DataContext>());
        _foodItemsDbSetMock = new Mock<DbSet<FoodItem>>();

        // Setup the property to return the mock DbSet
        _contextMock.Setup(x => x.foodItems)
            .Returns(_foodItemsDbSetMock.Object);

        _foodItemService = new FoodItemService(_contextMock.Object);
    }

    [Fact]
    public async Task DeleteFood_ExistingId_ShouldDeleteAndReturnFood()
    {
        // Arrange
        var foodItem = new FoodItem
        {
            Id = 1,
            Name = "Test Food",
            Price = 10.0
        };

        _foodItemsDbSetMock
            .Setup(d => d.FindAsync(1))
            .ReturnsAsync(foodItem);

        // Act
        var result = await _foodItemService.DeleteFood(1);

        // Assert
        Assert.Equal(foodItem, result);
        _foodItemsDbSetMock.Verify(d => d.Remove(foodItem), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFood_NonExistingId_ShouldThrowException()
    {
        // Arrange
        _foodItemsDbSetMock
            .Setup(d => d.FindAsync(999))
            .ReturnsAsync((FoodItem)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _foodItemService.DeleteFood(999)
        );
        Assert.Equal("Food item not found", exception.Message);
    }
}