using AsyncWarehouse.Domain.Models;

namespace AsyncWarehouse.Tests.DomainTests;

public class PalletTests
{
    [Fact]
    public void Constructor_WithZeroOrNegativeCapacity_ThrowsArgumentException()
    {
        // Arrange
        float invalidCapacity = 0f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Pallet(invalidCapacity));
        Assert.Equal("Max capacity must be greater than zero.", exception.Message);
    }

    [Fact]
    public void AddItem_WhenValidItem_AddsSuccessfullyAndUpdatesWeight()
    {
        // Arrange
        var pallet = new Pallet(100f);
        // Так как InventoryItem абстрактный, создаем конкретный класс (Electronics)
        var item = new Electronics 
        { 
            Id = Guid.NewGuid(), 
            Name = "Laptop", 
            Weight = 15f, 
            Price = 1000m, 
            SerialNumber = "123" 
        };

        // Act
        var result = pallet.AddItem(item);

        // Assert
        Assert.True(result);
        Assert.Single(pallet.Items); // Проверяем, что в списке 1 элемент
        Assert.Equal(15f, pallet.TotalWeight); // Проверяем правильный подсчет веса
    }

    [Fact]
    public void AddItem_WhenItemExceedsCapacity_ThrowsArgumentException()
    {
        // Arrange
        var pallet = new Pallet(100f);
        var heavyItem = new Electronics 
        { 
            Id = Guid.NewGuid(), 
            Name = "Fridge", 
            Weight = 150f, // Вес больше лимита палеты!
            Price = 500m, 
            SerialNumber = "456" 
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => pallet.AddItem(heavyItem));
        Assert.Contains("must not exceed pallet capacity", exception.Message);
        Assert.Empty(pallet.Items); // Убеждаемся, что товар НЕ добавился
    }
}