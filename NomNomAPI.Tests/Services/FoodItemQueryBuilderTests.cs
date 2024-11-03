using Xunit;
using NomNomAPI.Models;
using System.Linq;
using System;

public class FoodItemQueryBuilderTests
{
    private readonly IQueryable<FoodItem> _testData;

    public FoodItemQueryBuilderTests()
    {
        // Setup test data
        _testData = new List<FoodItem>
        {
            new FoodItem
            {
                Id = 1,
                Name = "Vegan Burger",
                StoreId = 1,
                Category = "Fast Food",
                Price = 10.0,
                ExpirationDate = DateTime.Now.AddDays(2),
                IsVegan = true,
                Description = "Delicious vegan burger"
            },
            new FoodItem
            {
                Id = 2,
                Name = "Chicken Sandwich",
                StoreId = 2,
                Category = "Sandwiches",
                Price = 8.0,
                ExpirationDate = DateTime.Now.AddDays(3),
                IsVegan = false,
                Description = "Classic chicken sandwich"
            },
            new FoodItem
            {
                Id = 3,
                Name = "Salad",
                StoreId = 1,
                Category = "Healthy",
                Price = 15.0,
                ExpirationDate = DateTime.Now.AddDays(1),
                IsVegan = true,
                Description = "Fresh garden salad"
            }
        }.AsQueryable();
    }

    [Fact]
    public void WithStoreId_ShouldFilterByStore()
    {
        // Arrange
        var builder = new FoodItemQueryBuilder(_testData);

        // Act
        var result = builder.WithStoreId(1).Build().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.Equal(1, item.StoreId));
    }

    [Fact]
    public void WithCategory_ShouldFilterByCategory()
    {
        // Arrange
        var builder = new FoodItemQueryBuilder(_testData);

        // Act
        var result = builder.WithCategory("Fast Food").Build().ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Fast Food", result[0].Category);
    }

    [Fact]
    public void WithExpirationDate_ShouldFilterByExpirationDate()
    {
        // Arrange
        var builder = new FoodItemQueryBuilder(_testData);
        var targetDate = DateTime.Now.AddDays(2);

        // Act
        var result = builder.WithExpirationDate(targetDate).Build().ToList();

        // Assert
        Assert.All(result, item =>
            Assert.True(item.ExpirationDate.Date <= targetDate.Date));
    }

    [Fact]
    public void WithPriceRange_ShouldFilterByPriceRange()
    {
        // Arrange
        var builder = new FoodItemQueryBuilder(_testData);

        // Act
        var result = builder
            .WithMinPrice(9.0)
            .WithMaxPrice(12.0)
            .Build()
            .ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(10.0, result[0].Price);
    }

    [Fact]
    public void WithName_ShouldFilterByPartialName()
    {
        // Arrange
        var builder = new FoodItemQueryBuilder(_testData);

        // Act
        var result = builder.WithName("burger").Build().ToList();

        // Assert
        Assert.Single(result);
        Assert.Contains("Burger", result[0].Name);
    }

    [Fact]
    public void WithVeganFilter_ShouldFilterVeganItems()
    {
        // Arrange
        var builder = new FoodItemQueryBuilder(_testData);

        // Act
        var result = builder.WithVeganFilter(true).Build().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.True(item.IsVegan));
    }

    [Fact]
    public void ChainedFilters_ShouldApplyAllFilters()
    {
        // Arrange
        var builder = new FoodItemQueryBuilder(_testData);

        // Act
        var result = builder
            .WithStoreId(1)
            .WithVeganFilter(true)
            .WithMinPrice(10.0)
            .Build()
            .ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, item =>
        {
            Assert.Equal(1, item.StoreId);
            Assert.True(item.IsVegan);
            Assert.True(item.Price >= 10.0);
        });
    }

    [Fact]
    public void NullFilters_ShouldNotAffectQuery()
    {
        // Arrange
        var builder = new FoodItemQueryBuilder(_testData);

        // Act
        var result = builder
            .WithStoreId(null)
            .WithCategory(null)
            .WithName(null)
            .WithDescription(null)
            .WithExpirationDate(null)
            .WithMinPrice(null)
            .WithMaxPrice(null)
            .WithVeganFilter(null)
            .WithMinDiscount(null)
            .Build()
            .ToList();

        // Assert
        Assert.Equal(_testData.Count(), result.Count);
    }
}